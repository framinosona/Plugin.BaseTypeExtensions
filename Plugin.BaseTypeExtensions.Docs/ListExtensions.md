# ListExtensions

Provides specialized operations and utilities for List<T> collections.

## Overview

The `ListExtensions` class offers enhanced list operations including bulk operations, conditional updates, and efficient list manipulation methods.

## API Reference

<xref:Plugin.BaseTypeExtensions.ListExtensions>

## Core Methods

### Bulk Operations

#### `AddRange<T>(List<T> list, params T[] items)`
Adds multiple items to the list efficiently.

```csharp
var list = new List<string> { "a", "b" };
list.AddRange("c", "d", "e");
// Result: ["a", "b", "c", "d", "e"]
```

#### `RemoveRange<T>(List<T> list, params T[] items)`
Removes multiple items from the list.

```csharp
var list = new List<string> { "a", "b", "c", "d" };
list.RemoveRange("b", "d");
// Result: ["a", "c"]
```

### Conditional Operations

#### `AddIf<T>(List<T> list, bool condition, T item)`
Adds item to list only if condition is true.

```csharp
var list = new List<string>();
list.AddIf(includeOptional, "optional-item");
list.AddIf(user.IsAdmin, "admin-feature");
```

#### `RemoveWhere<T>(List<T> list, Func<T, bool> predicate)`
Removes all items matching the predicate.

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
numbers.RemoveWhere(n => n % 2 == 0); // Remove even numbers
// Result: [1, 3, 5]
```

### Update Operations

#### `UpdateFrom<T>(List<T> source, List<T> target, Func<T, T, bool> matcher)`
Synchronizes source list with target list using custom matching logic.

```csharp
var sourceUsers = GetCurrentUsers();
var targetUsers = GetUpdatedUsers();

sourceUsers.UpdateFrom(
    targetUsers,
    (source, target) => source.Id == target.Id,
    onAdd: user => logger.LogInformation($"Added user: {user.Name}"),
    onRemove: user => logger.LogInformation($"Removed user: {user.Name}")
);
```

## Advanced Operations

### Batch Processing

#### `ProcessInBatches<T>(List<T> list, int batchSize, Action<List<T>> processor)`
Processes list items in batches of specified size.

```csharp
var largeList = GetLargeDataset();
largeList.ProcessInBatches(100, batch =>
{
    // Process each batch of 100 items
    ProcessBatch(batch);
});
```

### Safety Operations

#### `SafeAdd<T>(List<T> list, T? item)`
Adds item to list only if it's not null.

```csharp
var list = new List<string>();
list.SafeAdd(GetOptionalValue()); // Only adds if not null
```

#### `SafeRemove<T>(List<T> list, T? item)`
Removes item from list only if it exists.

```csharp
var list = new List<string> { "a", "b", "c" };
list.SafeRemove("b"); // Removes if exists
list.SafeRemove("x"); // No effect if not found
```

## Practical Examples

### Configuration Management

```csharp
public class ConfigurationManager
{
    private readonly List<ConfigItem> _configItems = new();

    public void UpdateConfiguration(List<ConfigItem> newConfig)
    {
        _configItems.UpdateFrom(
            newConfig,
            (existing, updated) => existing.Key == updated.Key,
            onAdd: item => logger.LogInformation($"Added config: {item.Key}"),
            onRemove: item => logger.LogInformation($"Removed config: {item.Key}")
        );
    }

    public void AddConditionalConfigs(bool isProduction, bool enableFeatureX)
    {
        _configItems.AddIf(isProduction, new ConfigItem("Environment", "Production"));
        _configItems.AddIf(enableFeatureX, new ConfigItem("FeatureX", "Enabled"));
    }
}
```

### Data Processing Pipeline

```csharp
public class DataProcessor
{
    public void ProcessLargeDataset(List<DataItem> data)
    {
        // Remove invalid items
        data.RemoveWhere(item => !item.IsValid);

        // Process in manageable batches
        data.ProcessInBatches(1000, batch =>
        {
            var processed = ProcessBatch(batch);
            SaveToDatabase(processed);
        });
    }
}
```

## Performance Considerations

- Bulk operations are optimized for efficiency
- Uses List<T> internal optimizations where possible
- Batch processing prevents memory pressure on large datasets
- Conditional operations avoid unnecessary allocations

## Thread Safety

- Extension methods are thread-safe but List<T> itself is not thread-safe
- Use concurrent collections for thread-safe scenarios
- Consider locking when modifying lists from multiple threads

## Best Practices

1. **Use Bulk Operations**: Prefer bulk operations over individual adds/removes
2. **Batch Large Operations**: Process large lists in batches to manage memory
3. **Validate Input**: Check for null lists and items where appropriate
4. **Consider Capacity**: Pre-size lists when final size is known
5. **Thread Safety**: Use appropriate synchronization for concurrent access
