# 🗂️ Plugin.BaseTypeExtensions Documentation

Welcome to the **Plugin.BaseTypeExtensions** documentation! This library provides a comprehensive collection of extension methods for .NET base types, designed to enhance productivity, type safety, and code readability in modern .NET applications.

---

## 🚀 Quick Start

### Installation

```bash
# Install from NuGet (when published)
dotnet add package Plugin.BaseTypeExtensions
```

### Basic Usage

```csharp
using Plugin.BaseTypeExtensions;

// String utilities
string result = input.NullIfDud() ?? "default";
string clean = "Hello@World!".RemoveSpecialCharacters(); // "HelloWorld"

// Numeric conversions and calculations
double radians = 45.0.DegreeToRadian();
int clamped = value.Clamp(0, 100);
double percentage = 150.ValueToPercentage(100, 200); // 0.5

// Enum operations with flags
var flags = MyFlags.None.SetFlag(MyFlags.Option1);
string description = MyEnum.Value.GetDescription();
var allValues = EnumExtensions.AllAsArray<MyEnum>();

// Collection operations
items.NullIfEmpty()?.Count(); // Returns null if empty
dictionary.Add(new KeyValuePair<string, int>("key", 42));
list.UpdateFrom(newItems, matcher, onAdd, onRemove);

// Date/Time utilities
DateTime result = 0.5.PercentageToDateTime(start, end);
double progress = current.DateTimeToPercentage(start, end);

// Assembly and reflection helpers
var assembly = instance.GetAssembly();
var typesWithAttribute = assembly.GetTypesWithAttribute<MyAttribute>();
var resourceFile = assembly.MoveManifestResourceToCache("resource.txt");
```

---

## 📚 Core Feature Categories

### � String Extensions

Enhanced string manipulation and validation:

- **Null Safety**: `NullIfDud()` for clean null handling
- **Sanitization**: `RemoveSpecialCharacters()` for input cleaning
- **Validation**: Built-in null and whitespace checking

### 🔢 Numeric Extensions

Powerful numeric operations for all `INumber<T>` types:

- **Angle Conversion**: `DegreeToRadian()`, `RadianToDegree()`
- **Range Operations**: `Clamp()`, `PercentageToValue()`, `ValueToPercentage()`
- **Type Conversion**: `ToByte()`, percentage calculations
- **Range Generation**: `NumericRangeTools.GetRange()` with custom steps

### 🏷️ Enum Extensions

Comprehensive enum manipulation and introspection:

- **Flag Operations**: `SetFlag()`, `UnsetFlag()`, `TweakFlag()`
- **Metadata**: `GetDescription()` with `DescriptionAttribute` support
- **Enumeration**: `AllAsArray<T>()` for all enum values
- **Performance**: Optimized with `MethodImpl.AggressiveInlining`

### 📅 Temporal Extensions

Date and time utilities for `DateTime`, `DateOnly`, and `TimeOnly`:

- **Percentage Mapping**: Convert between dates and percentages
- **Range Calculations**: Progress tracking between date ranges
- **Type-Safe Operations**: Overloaded for different temporal types

### 🗂️ Collection Extensions

Advanced collection manipulation and synchronization:

- **Dictionary Operations**: `Add()`, `Remove()`, `Update()` with KeyValuePair
- **List Management**: Comprehensive list utilities
- **Enumerable Utilities**: `NullIfEmpty()`, `GetOrDefault()`, `Enqueue()` with limits
- **Synchronization**: `UpdateFrom()` with custom comparison and update logic
- **Dictionary Comparison**: `CompareDictionaries()` with transformation support

### 🔍 Reflection & Assembly Extensions

Safe reflection and assembly resource management:

- **Type Discovery**: `GetTypesWithAttribute<T>()` with async support
- **Resource Extraction**: `MoveManifestResourceToDirectory()` with conflict handling
- **Assembly Utilities**: `GetAssembly()`, `GetResourceStream()`
- **Caching**: `MoveManifestResourceToCache()` with configurable cache directory

### ⚖️ Comparison Extensions

Enhanced comparison operations for `IComparable<T>`:

- **Range Checking**: `IsBetween()` with inclusive/exclusive options
- **Fluent Comparisons**: `IsGreaterThan()`, `IsLessThanOrEqual()`, etc.
- **Utility Functions**: `ComparableTools.Min()`, `ComparableTools.Max()`

### 🧮 Additional Utilities

Specialized extensions for other base types:

- **Byte Operations**: Bit manipulation, nibble extraction
- **GUID Utilities**: `GetPlusOne()` for sequential GUIDs
- **Version Extensions**: Version comparison and manipulation
- **Task Extensions**: `TaskCompletionSource` and `Task` utilities
- **Random Extensions**: Enhanced random number generation

---

## 📖 Documentation Sections

| Section | Description |
|---------|-------------|
| [API Reference](./api/Plugin.BaseTypeExtensions.html) | Complete API documentation |

---

## 🎯 Design Principles

- **🛡️ Type Safety**: Generic constraints ensure compile-time safety
- **⚡ Performance**: Optimized with modern .NET features and aggressive inlining
- **🔄 Null Safety**: Explicit null handling with clear behavior
- **📐 Consistency**: Uniform naming and parameter patterns across all extensions
- **🌐 Modern .NET**: Built for .NET 9 with latest C# language features
- **🧪 Thoroughly Tested**: Comprehensive test coverage with edge case handling

---

## 🚀 Advanced Examples

### Working with Numeric Ranges

```csharp
// Generate custom numeric ranges
var range = NumericRangeTools.GetRange(0.0, 10.0, 0.5);
foreach (var value in range)
{
    Console.WriteLine($"Value: {value}");
}

// Percentage calculations with different types
var progress = 75.PercentageToValue(0, 100);     // 75
var normalized = 150.ValueToPercentage(100, 200); // 0.5
var clamped = 150.Clamp(0, 100);                 // 100
```

### Collection Synchronization

```csharp
// Synchronize collections with custom logic
sourceList.UpdateFrom(
    targetList,
    (source, target) => source.Id == target.Id,
    onAdd: item => Console.WriteLine($"Added: {item}"),
    onRemove: item => Console.WriteLine($"Removed: {item}")
);

// Dictionary operations with transformation
var updates = sourceDict.CompareDictionaries(
    (oldKey, newKey) => oldKey == newKey,
    (oldValue, newValue) => oldValue.Equals(newValue),
    oldDict,
    newDict
);
```

### Enum Flag Management

```csharp
[Flags]
public enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}

// Fluent flag operations
var permissions = FilePermissions.None
    .SetFlag(FilePermissions.Read)
    .SetFlag(FilePermissions.Write);

// Get all enum values
var allPermissions = EnumExtensions.AllAsArray<FilePermissions>();
```

### Assembly Resource Management

```csharp
// Extract embedded resources safely
var assembly = Assembly.GetExecutingAssembly();
var resourceFile = await assembly.MoveManifestResourceToCacheAsync(
    "MyApp.Resources.config.json",
    filename: "app-config.json",
    fileAlreadyExistsBehavior: FileAlreadyExistsBehavior.Overwrite
);

// Discover types with attributes
var servicesTypes = assembly.GetTypesWithAttribute<ServiceAttribute>();
foreach (var (attribute, type) in servicesTypes)
{
    Console.WriteLine($"Service: {type.Name}, Config: {attribute.Configuration}");
}
```

---

## 🔧 Development & Testing

- **Framework**: .NET 9.0
- **Testing**: xUnit + FluentAssertions with 90%+ coverage
- **Performance**: Benchmarked with BenchmarkDotNet
- **Build**: `dotnet build -c Release`
- **Test**: `dotnet test -c Release`

---

## 📄 License

This library is licensed under the **MIT License** - see the [LICENSE.md](https://github.com/framinosona/Plugin.BaseTypeExtensions/blob/main/LICENSE.md) file for details.

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests on [GitHub](https://github.com/framinosona/Plugin.BaseTypeExtensions).

---

*Built with ❤️ for modern .NET applications requiring powerful, type-safe base type extensions.*
