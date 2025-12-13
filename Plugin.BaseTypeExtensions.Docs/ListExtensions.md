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

#### `UpdateFrom<T>(IList<T> output, IEnumerable<T>? addedItems, IEnumerable<T>? removedItems)`
Directly adds and removes specified items from the list.

```csharp
var list = new List<string> { "a", "b", "c" };
var toAdd = new[] { "d", "e" };
var toRemove = new[] { "b" };

list.UpdateFrom(
    addedItems: toAdd,
    removedItems: toRemove
);
// Result: ["a", "c", "d", "e"]
```

#### `UpdateFrom<TInput, TOutput>(IList<TOutput> output, IList<TInput> input, Func<TInput, TOutput, bool> areRepresentingTheSameItem, Func<TInput, TOutput> fromInputTypeToOutputTypeConversion)`
Synchronizes list with type conversion.

```csharp
var viewModels = new List<UserViewModel>();
var dtos = await GetUserDtosFromApi();

viewModels.UpdateFrom(
    input: dtos,
    areRepresentingTheSameItem: (dto, vm) => dto.Id == vm.Id,
    fromInputTypeToOutputTypeConversion: dto => new UserViewModel(dto)
);
```

#### `UpdateFrom<TInput, TOutput>(IList<TOutput> output, IList<TInput> input, Func<TInput, TOutput> fromInputTypeToOutputTypeConversion)`
Simplified synchronization when output type implements IEquatable.

```csharp
var ids = new List<int>();
var users = await GetUsersAsync();

ids.UpdateFrom(
    input: users,
    fromInputTypeToOutputTypeConversion: user => user.Id
);
```

#### `UpdateFrom<T>(IList<T> output, IList<T> input, Func<T, T, bool> areRepresentingTheSameItem)`
Synchronizes lists of the same type with custom comparison.

```csharp
var currentUsers = GetCurrentUsers();
var updatedUsers = GetUpdatedUsers();

currentUsers.UpdateFrom(
    input: updatedUsers,
    areRepresentingTheSameItem: (user1, user2) => user1.Id == user2.Id
);
```

#### `UpdateFrom<T>(IList<T> output, IList<T> input)`
Simple synchronization using default equality comparison.

```csharp
var list1 = new List<int> { 1, 2, 3 };
var list2 = new List<int> { 2, 3, 4 };

list1.UpdateFrom(list2);
// Result: [2, 3, 4]
```

### UpdateFromAsync Methods

Async versions for scenarios involving async operations.

#### `UpdateFromAsync<T>(IList<T> output, IEnumerable<T>? addedItems, IEnumerable<T>? removedItems, Func<T, CancellationToken, Task>? addAction, Func<T, CancellationToken, Task<bool>>? removeAction, CancellationToken cancellationToken)`
Asynchronously adds and removes items with cancellation support.

```csharp
var list = new List<Product>();
var toAdd = new[] { product1, product2 };
var toRemove = new[] { product3 };

await list.UpdateFromAsync(
    addedItems: toAdd,
    removedItems: toRemove,
    addAction: async (product, ct) =>
    {
        await _productService.CreateAsync(product, ct);
        list.Add(product);
    },
    removeAction: async (product, ct) =>
    {
        await _productService.DeleteAsync(product.Id, ct);
        return list.Remove(product);
    },
    cancellationToken: cancellationToken
);
```

#### `UpdateFromAsync<TInput, TOutput>(IList<TOutput> output, IList<TInput> input, Func<TInput, TOutput, bool> areRepresentingTheSameItem, Func<TInput, TOutput> fromInputTypeToOutputTypeConversion, Func<TOutput, CancellationToken, Task> addAction, Func<TOutput, CancellationToken, Task<bool>> removeAction, CancellationToken cancellationToken)`
Asynchronously synchronizes with type conversion.

```csharp
var entities = await dbContext.Orders.ToListAsync();
var dtos = await GetOrdersFromApi();

await entities.UpdateFromAsync(
    input: dtos,
    areRepresentingTheSameItem: (dto, entity) => dto.OrderId == entity.Id,
    fromInputTypeToOutputTypeConversion: dto => new Order { Id = dto.OrderId, Total = dto.Total },
    addAction: async (entity, ct) =>
    {
        dbContext.Orders.Add(entity);
        await dbContext.SaveChangesAsync(ct);
    },
    removeAction: async (entity, ct) =>
    {
        dbContext.Orders.Remove(entity);
        await dbContext.SaveChangesAsync(ct);
        return true;
    },
    cancellationToken: cancellationToken
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
