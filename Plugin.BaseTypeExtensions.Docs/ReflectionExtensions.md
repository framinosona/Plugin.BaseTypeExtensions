# ReflectionExtensions

Provides advanced reflection utilities and type operations.

## Overview

The `ReflectionExtensions` class offers powerful reflection utilities for type introspection, member access, attribute discovery, and dynamic operations.

## API Reference

<xref:Plugin.BaseTypeExtensions.ReflectionExtensions>

## Core Methods

### Type Information

#### `GetFriendlyName(Type type)`
Gets a human-readable name for a type, including generic types.

```csharp
var listType = typeof(List<string>);
var friendlyName = listType.GetFriendlyName(); // "List<String>"

var dictionaryType = typeof(Dictionary<int, List<string>>);
var complexName = dictionaryType.GetFriendlyName(); // "Dictionary<Int32, List<String>>"
```

#### `IsNullable(Type type)`
Checks if a type is nullable (reference type or Nullable<T>).

```csharp
bool isNullable1 = typeof(string).IsNullable(); // true (reference type)
bool isNullable2 = typeof(int?).IsNullable(); // true (Nullable<int>)
bool isNullable3 = typeof(int).IsNullable(); // false (value type)
```

#### `GetUnderlyingType(Type type)`
Gets the underlying type for nullable types.

```csharp
var underlyingType = typeof(int?).GetUnderlyingType(); // typeof(int)
var sameType = typeof(string).GetUnderlyingType(); // typeof(string)
```

### Attribute Operations

#### `HasAttribute<T>(MemberInfo member)` where T : Attribute
Checks if a member has a specific attribute.

```csharp
var property = typeof(User).GetProperty("Email");
bool hasRequired = property.HasAttribute<RequiredAttribute>(); // true/false

var method = typeof(Controller).GetMethod("GetUsers");
bool hasAuth = method.HasAttribute<AuthorizeAttribute>();
```

#### `GetAttribute<T>(MemberInfo member)` where T : Attribute
Gets a specific attribute from a member.

```csharp
var property = typeof(User).GetProperty("Name");
var maxLength = property.GetAttribute<MaxLengthAttribute>();
if (maxLength != null)
{
    Console.WriteLine($"Max length: {maxLength.Length}");
}
```

#### `GetAttributes<T>(MemberInfo member)` where T : Attribute
Gets all attributes of a specific type from a member.

```csharp
var method = typeof(Controller).GetMethod("UpdateUser");
var validationAttrs = method.GetAttributes<ValidationAttribute>();
foreach (var attr in validationAttrs)
{
    Console.WriteLine($"Validation: {attr.GetType().Name}");
}
```

### Member Discovery

#### `GetPropertiesWithAttribute<T>(Type type)` where T : Attribute
Gets all properties that have a specific attribute.

```csharp
var jsonProperties = typeof(ApiModel).GetPropertiesWithAttribute<JsonPropertyNameAttribute>();
foreach (var (property, attribute) in jsonProperties)
{
    Console.WriteLine($"Property: {property.Name}, JSON Name: {attribute.Name}");
}
```

#### `GetMethodsWithAttribute<T>(Type type)` where T : Attribute
Gets all methods that have a specific attribute.

```csharp
var endpoints = typeof(ApiController).GetMethodsWithAttribute<HttpGetAttribute>();
foreach (var (method, attribute) in endpoints)
{
    Console.WriteLine($"Endpoint: {method.Name}, Route: {attribute.Template}");
}
```

## Advanced Operations

### Dynamic Invocation

#### `InvokeMethod(object instance, string methodName, params object[] args)`
Dynamically invokes a method on an object.

```csharp
var calculator = new Calculator();
var result = calculator.InvokeMethod("Add", 5, 3); // Calls Add(5, 3)
Console.WriteLine($"Result: {result}");
```

#### `GetPropertyValue(object instance, string propertyName)`
Gets the value of a property dynamically.

```csharp
var user = new User { Name = "John", Email = "john@example.com" };
var name = user.GetPropertyValue("Name"); // "John"
var email = user.GetPropertyValue("Email"); // "john@example.com"
```

#### `SetPropertyValue(object instance, string propertyName, object value)`
Sets the value of a property dynamically.

```csharp
var user = new User();
user.SetPropertyValue("Name", "Jane");
user.SetPropertyValue("Age", 25);
```

### Type Conversion

#### `CanConvertTo(Type sourceType, Type targetType)`
Checks if one type can be converted to another.

```csharp
bool canConvert1 = typeof(string).CanConvertTo(typeof(int)); // false
bool canConvert2 = typeof(int).CanConvertTo(typeof(double)); // true
bool canConvert3 = typeof(string).CanConvertTo(typeof(DateTime)); // true (if parseable)
```

#### `ConvertValue(object value, Type targetType)`
Converts a value to a target type.

```csharp
var stringValue = "123";
var intValue = stringValue.ConvertValue(typeof(int)); // 123

var dateString = "2024-01-01";
var dateValue = dateString.ConvertValue(typeof(DateTime)); // DateTime object
```

## Practical Examples

### Object Mapper

```csharp
public class SimpleObjectMapper
{
    public TTarget Map<TSource, TTarget>(TSource source) where TTarget : new()
    {
        var target = new TTarget();
        var sourceType = typeof(TSource);
        var targetType = typeof(TTarget);

        var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var targetProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite)
            .ToDictionary(p => p.Name, p => p);

        foreach (var sourceProp in sourceProperties)
        {
            if (targetProperties.TryGetValue(sourceProp.Name, out var targetProp))
            {
                var sourceValue = source.GetPropertyValue(sourceProp.Name);
                if (sourceValue != null && targetProp.PropertyType.CanConvertTo(sourceProp.PropertyType))
                {
                    var convertedValue = sourceValue.ConvertValue(targetProp.PropertyType);
                    target.SetPropertyValue(targetProp.Name, convertedValue);
                }
            }
        }

        return target;
    }
}
```

### Validation Framework

```csharp
public class ValidationEngine
{
    public ValidationResult Validate(object instance)
    {
        var type = instance.GetType();
        var errors = new List<string>();

        // Validate properties with validation attributes
        var validatedProperties = type.GetPropertiesWithAttribute<ValidationAttribute>();

        foreach (var (property, validationAttr) in validatedProperties)
        {
            var value = instance.GetPropertyValue(property.Name);
            if (!validationAttr.IsValid(value))
            {
                errors.Add($"{property.Name}: {validationAttr.ErrorMessage}");
            }
        }

        // Check for required properties
        var requiredProperties = type.GetPropertiesWithAttribute<RequiredAttribute>();

        foreach (var (property, _) in requiredProperties)
        {
            var value = instance.GetPropertyValue(property.Name);
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                errors.Add($"{property.Name} is required");
            }
        }

        return new ValidationResult
        {
            IsValid = errors.Count == 0,
            Errors = errors
        };
    }
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}
```

### Configuration Binding

```csharp
public class ConfigurationBinder
{
    private readonly Dictionary<string, string> _configuration;

    public ConfigurationBinder(Dictionary<string, string> configuration)
    {
        _configuration = configuration;
    }

    public T Bind<T>() where T : new()
    {
        var instance = new T();
        var type = typeof(T);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite);

        foreach (var property in properties)
        {
            var configKey = GetConfigurationKey(property);
            if (_configuration.TryGetValue(configKey, out var configValue))
            {
                try
                {
                    var convertedValue = configValue.ConvertValue(property.PropertyType);
                    instance.SetPropertyValue(property.Name, convertedValue);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Failed to bind configuration key '{configKey}' to property '{property.Name}'", ex);
                }
            }
        }

        return instance;
    }

    private string GetConfigurationKey(PropertyInfo property)
    {
        var configAttr = property.GetAttribute<ConfigurationKeyAttribute>();
        return configAttr?.Key ?? property.Name;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class ConfigurationKeyAttribute : Attribute
{
    public string Key { get; }

    public ConfigurationKeyAttribute(string key)
    {
        Key = key;
    }
}
```

### Command Pattern Implementation

```csharp
public class CommandDispatcher
{
    private readonly Dictionary<Type, Func<object, Task>> _handlers = new();

    public void RegisterHandler<TCommand>(Func<TCommand, Task> handler)
    {
        _handlers[typeof(TCommand)] = command => handler((TCommand)command);
    }

    public async Task DispatchAsync(object command)
    {
        var commandType = command.GetType();

        if (_handlers.TryGetValue(commandType, out var handler))
        {
            await handler(command);
        }
        else
        {
            throw new InvalidOperationException($"No handler registered for command type: {commandType.GetFriendlyName()}");
        }
    }

    public void RegisterHandlersFromAssembly(Assembly assembly)
    {
        var handlerTypes = assembly.GetTypesWithAttribute<CommandHandlerAttribute>();

        foreach (var (handlerType, attribute) in handlerTypes)
        {
            var handler = Activator.CreateInstance(handlerType.Type);
            var methods = handlerType.Type.GetMethodsWithAttribute<HandlesAttribute>();

            foreach (var (method, handlesAttr) in methods)
            {
                var commandType = handlesAttr.CommandType;
                var handlerFunc = CreateHandlerFunc(handler, method, commandType);
                _handlers[commandType] = handlerFunc;
            }
        }
    }

    private Func<object, Task> CreateHandlerFunc(object handler, MethodInfo method, Type commandType)
    {
        return async command =>
        {
            var result = handler.InvokeMethod(method.Name, command);
            if (result is Task task)
            {
                await task;
            }
        };
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class CommandHandlerAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class HandlesAttribute : Attribute
{
    public Type CommandType { get; }

    public HandlesAttribute(Type commandType)
    {
        CommandType = commandType;
    }
}
```

## Performance Considerations

- Reflection operations are slower than direct code access
- Cache reflection results (PropertyInfo, MethodInfo) when possible
- Use compiled expressions for frequently accessed members
- Consider using delegates for dynamic method invocation

## Thread Safety

- Type metadata operations are thread-safe
- Property and method invocation should be synchronized if modifying shared state
- Caching reflection results is safe for concurrent read access

## Best Practices

1. **Cache Reflection Results**: Store PropertyInfo and MethodInfo objects for reuse
2. **Error Handling**: Handle reflection exceptions gracefully
3. **Type Safety**: Validate types and parameters before dynamic operations
4. **Performance**: Use direct access when possible, reflection when necessary
5. **Security**: Validate member names and types to prevent security issues
