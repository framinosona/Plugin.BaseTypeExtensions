# AssemblyExtensions

Provides assembly information extraction and embedded resource management utilities.

## Overview

The `AssemblyExtensions` class offers powerful utilities for working with .NET assemblies, including metadata extraction, embedded resource operations, and type discovery.

## API Reference

<xref:Plugin.BaseTypeExtensions.AssemblyExtensions>

## Core Methods

### Assembly Information

#### `GetAssembly(object instance)`
Gets the assembly that contains the specified object's type.

```csharp
var myObject = new MyClass();
var assembly = myObject.GetAssembly();
Console.WriteLine($"Assembly: {assembly.GetName().Name}");
```

#### `GetVersion(Assembly assembly)`
Retrieves the version information from an assembly.

```csharp
var assembly = Assembly.GetExecutingAssembly();
var version = assembly.GetVersion();
Console.WriteLine($"Version: {version}");
```

### Embedded Resource Operations

#### `GetManifestResourceStream(Assembly assembly, string resourceName)`
Gets a stream for reading an embedded resource.

```csharp
var assembly = Assembly.GetExecutingAssembly();
using var stream = assembly.GetManifestResourceStream("MyApp.Resources.config.json");
if (stream != null)
{
    using var reader = new StreamReader(stream);
    var content = reader.ReadToEnd();
}
```

#### `MoveManifestResourceToDirectory(Assembly assembly, string resourceName, string targetDirectory)`
Extracts an embedded resource to a directory.

```csharp
var assembly = Assembly.GetExecutingAssembly();
var filePath = assembly.MoveManifestResourceToDirectory(
    "MyApp.Resources.template.html",
    @"C:\Output"
);
Console.WriteLine($"Resource extracted to: {filePath}");
```

### Type Discovery

#### `GetTypesWithAttribute<T>(Assembly assembly)` where T : Attribute
Finds all types in an assembly that have a specific attribute.

```csharp
var assembly = Assembly.GetExecutingAssembly();
var serviceTypes = assembly.GetTypesWithAttribute<ServiceAttribute>();

foreach (var (attribute, type) in serviceTypes)
{
    Console.WriteLine($"Service: {type.Name}, Lifetime: {attribute.Lifetime}");
}
```

## Advanced Operations

### Resource Management

#### `MoveManifestResourceToCache(Assembly assembly, string resourceName, string filename = null)`
Extracts resource to a temporary cache directory.

```csharp
var assembly = Assembly.GetExecutingAssembly();
var cachedFile = assembly.MoveManifestResourceToCache(
    "MyApp.Resources.data.xml",
    "cached-data.xml"
);

// Use the cached file
var xmlContent = File.ReadAllText(cachedFile);
```

#### `ExtractAllResources(Assembly assembly, string targetDirectory)`
Extracts all embedded resources to a directory.

```csharp
var assembly = Assembly.GetExecutingAssembly();
var extractedFiles = assembly.ExtractAllResources(@"C:\ExtractedResources");

foreach (var file in extractedFiles)
{
    Console.WriteLine($"Extracted: {file}");
}
```

## Practical Examples

### Plugin System

```csharp
public class PluginManager
{
    public void LoadPlugins(string pluginDirectory)
    {
        foreach (var dllFile in Directory.GetFiles(pluginDirectory, "*.dll"))
        {
            var assembly = Assembly.LoadFrom(dllFile);
            var pluginTypes = assembly.GetTypesWithAttribute<PluginAttribute>();

            foreach (var (attribute, type) in pluginTypes)
            {
                var plugin = Activator.CreateInstance(type) as IPlugin;
                RegisterPlugin(plugin, attribute);
            }
        }
    }

    private void RegisterPlugin(IPlugin plugin, PluginAttribute attribute)
    {
        Console.WriteLine($"Loaded plugin: {attribute.Name} v{attribute.Version}");
        // Register plugin with dependency injection container
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class PluginAttribute : Attribute
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string Description { get; set; }
}
```

### Configuration Template System

```csharp
public class ConfigurationTemplateManager
{
    private readonly Assembly _assembly;

    public ConfigurationTemplateManager()
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    public string CreateConfigurationFile(string templateName, string outputPath, Dictionary<string, string> variables)
    {
        // Extract template from embedded resources
        var templatePath = _assembly.MoveManifestResourceToCache(
            $"MyApp.Templates.{templateName}.template",
            $"{templateName}.tmp"
        );

        // Read and process template
        var template = File.ReadAllText(templatePath);
        foreach (var (key, value) in variables)
        {
            template = template.Replace($"{{{{ {key} }}}}", value);
        }

        // Write processed configuration
        File.WriteAllText(outputPath, template);
        return outputPath;
    }

    public string[] GetAvailableTemplates()
    {
        return _assembly.GetManifestResourceNames()
            .Where(name => name.Contains(".Templates.") && name.EndsWith(".template"))
            .Select(name => Path.GetFileNameWithoutExtension(name.Split('.').Last()))
            .ToArray();
    }
}
```

### Service Discovery

```csharp
public class ServiceDiscovery
{
    public void RegisterServices(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            RegisterTransientServices(services, assembly);
            RegisterSingletonServices(services, assembly);
            RegisterScopedServices(services, assembly);
        }
    }

    private void RegisterTransientServices(IServiceCollection services, Assembly assembly)
    {
        var transientTypes = assembly.GetTypesWithAttribute<TransientServiceAttribute>();

        foreach (var (attribute, type) in transientTypes)
        {
            var interfaceType = attribute.InterfaceType ?? type.GetInterfaces().FirstOrDefault();
            if (interfaceType != null)
            {
                services.AddTransient(interfaceType, type);
            }
            else
            {
                services.AddTransient(type);
            }
        }
    }

    private void RegisterSingletonServices(IServiceCollection services, Assembly assembly)
    {
        var singletonTypes = assembly.GetTypesWithAttribute<SingletonServiceAttribute>();

        foreach (var (attribute, type) in singletonTypes)
        {
            var interfaceType = attribute.InterfaceType ?? type.GetInterfaces().FirstOrDefault();
            if (interfaceType != null)
            {
                services.AddSingleton(interfaceType, type);
            }
            else
            {
                services.AddSingleton(type);
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class TransientServiceAttribute : Attribute
{
    public Type InterfaceType { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
public class SingletonServiceAttribute : Attribute
{
    public Type InterfaceType { get; set; }
}
```

### Resource Localization

```csharp
public class LocalizationManager
{
    private readonly Assembly _assembly;
    private readonly Dictionary<string, Dictionary<string, string>> _localizedStrings;

    public LocalizationManager()
    {
        _assembly = Assembly.GetExecutingAssembly();
        _localizedStrings = new Dictionary<string, Dictionary<string, string>>();
        LoadLocalizations();
    }

    private void LoadLocalizations()
    {
        var resourceNames = _assembly.GetManifestResourceNames()
            .Where(name => name.Contains(".Localization.") && name.EndsWith(".json"));

        foreach (var resourceName in resourceNames)
        {
            var cultureName = ExtractCultureFromResourceName(resourceName);
            using var stream = _assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var strings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            _localizedStrings[cultureName] = strings;
        }
    }

    public string GetString(string key, string culture = "en")
    {
        if (_localizedStrings.TryGetValue(culture, out var strings) &&
            strings.TryGetValue(key, out var value))
        {
            return value;
        }

        // Fallback to English
        if (culture != "en" && _localizedStrings.TryGetValue("en", out var fallbackStrings) &&
            fallbackStrings.TryGetValue(key, out var fallbackValue))
        {
            return fallbackValue;
        }

        return key; // Return key if no localization found
    }

    private string ExtractCultureFromResourceName(string resourceName)
    {
        // Extract culture from "MyApp.Localization.en.json"
        var parts = resourceName.Split('.');
        return parts[^2]; // Second to last part
    }
}
```

## Performance Considerations

- Assembly metadata operations are cached by the runtime
- Resource extraction creates temporary files that should be cleaned up
- Type discovery operations can be expensive for large assemblies
- Consider caching results for frequently accessed operations

## Thread Safety

- Assembly metadata operations are thread-safe
- Resource extraction operations should be synchronized if writing to the same directory
- Type discovery results can be safely cached and shared

## Best Practices

1. **Resource Naming**: Use consistent naming conventions for embedded resources
2. **Cleanup**: Dispose of streams and clean up extracted files when no longer needed
3. **Caching**: Cache expensive operations like type discovery
4. **Error Handling**: Handle cases where resources or types are not found
5. **Security**: Validate resource names and output paths to prevent path traversal attacks
