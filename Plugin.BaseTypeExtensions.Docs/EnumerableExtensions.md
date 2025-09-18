# EnumerableExtensions

Provides powerful LINQ-style operations and utilities for IEnumerable collections.

## Overview

The `EnumerableExtensions` class offers enhanced collection operations including chunking, batch processing, conditional operations, and advanced LINQ extensions.

## API Reference

<xref:Plugin.BaseTypeExtensions.EnumerableExtensions>

## Batch Processing

### Chunking Operations

#### `ChunkBy<T>(IEnumerable<T> source, int size)`
Splits collection into chunks of specified size.

```csharp
var numbers = Enumerable.Range(1, 10);
var chunks = numbers.ChunkBy(3);
// Result: [[1,2,3], [4,5,6], [7,8,9], [10]]

foreach (var chunk in chunks)
{
    Console.WriteLine($"Chunk: [{string.Join(", ", chunk)}]");
}
```

### Batch Processing

#### `ProcessInBatches<T>(IEnumerable<T> source, int batchSize, Action<IEnumerable<T>> processor)`
Processes collection in batches with specified action.

```csharp
var largeDataset = GetLargeDataset();

// Process 100 items at a time
largeDataset.ProcessInBatches(100, batch =>
{
    // Process batch efficiently
    var results = ProcessBatchInDatabase(batch);
    logger.LogInformation($"Processed {batch.Count()} items");
});
```

## Conditional Operations

### WhereIf Extension

#### `WhereIf<T>(IEnumerable<T> source, bool condition, Func<T, bool> predicate)`
Applies filter only if condition is true.

```csharp
var users = GetUsers();
var includeInactive = GetUserPreference();

var filteredUsers = users
    .WhereIf(!includeInactive, u => u.IsActive)
    .WhereIf(hasAdminRights, u => u.Department == "IT");
```

### SelectIf Extension

#### `SelectIf<T, TResult>(IEnumerable<T> source, bool condition, Func<T, TResult> selector, Func<T, TResult> alternativeSelector)`
Conditional selection with fallback.

```csharp
var users = GetUsers();
var showFullName = GetDisplayPreference();

var displayNames = users.SelectIf(
    showFullName,
    u => $"{u.FirstName} {u.LastName}",
    u => u.FirstName
);
```

## Advanced LINQ Operations

### Distinct Operations

#### `DistinctBy<T, TKey>(IEnumerable<T> source, Func<T, TKey> keySelector)`
Returns distinct elements by specified key.

```csharp
var users = GetUsers();
var uniqueByEmail = users.DistinctBy(u => u.Email);
var uniqueByDepartment = users.DistinctBy(u => u.Department);
```

### Safety Extensions

#### `EmptyIfNull<T>(IEnumerable<T>? source)`
Returns empty collection if source is null.

```csharp
IEnumerable<string>? potentiallyNull = GetOptionalData();
var safeEnumerable = potentiallyNull.EmptyIfNull();

// Safe to enumerate without null checks
foreach (var item in safeEnumerable)
{
    ProcessItem(item);
}
```

#### `IsNullOrEmpty<T>(IEnumerable<T>? source)`
Checks if collection is null or empty.

```csharp
var items = GetItems();
if (items.IsNullOrEmpty())
{
    DisplayNoItemsMessage();
    return;
}

ProcessItems(items);
```

## Aggregation Operations

### Statistical Functions

#### `MaxBy<T, TComparable>(IEnumerable<T> source, Func<T, TComparable> selector)`
Finds element with maximum value by selector.

```csharp
var employees = GetEmployees();
var highestPaid = employees.MaxBy(e => e.Salary);
var oldestEmployee = employees.MaxBy(e => e.Age);
```

#### `MinBy<T, TComparable>(IEnumerable<T> source, Func<T, TComparable> selector)`
Finds element with minimum value by selector.

```csharp
var products = GetProducts();
var cheapestProduct = products.MinBy(p => p.Price);
var newestProduct = products.MinBy(p => p.CreatedDate);
```

## Performance Considerations

### Deferred Execution
- Most operations use deferred execution (lazy evaluation)
- Chain operations efficiently without intermediate collections
- Materialize with `.ToList()` or `.ToArray()` when needed

### Memory Efficiency
```csharp
// ✅ Efficient - deferred execution
var result = largeDataset
    .Where(x => x.IsActive)
    .Select(x => x.Name)
    .Take(10);

// ❌ Inefficient - immediate materialization
var result = largeDataset
    .Where(x => x.IsActive).ToList() // Unnecessary materialization
    .Select(x => x.Name).ToList()    // Another unnecessary materialization
    .Take(10);
```

## Thread Safety

- Extension methods are thread-safe (no shared state)
- Be cautious with underlying collections that may not be thread-safe
- Consider using concurrent collections for parallel processing

## Best Practices

1. **Use Deferred Execution**: Chain operations before materializing
2. **Handle Null Collections**: Use `EmptyIfNull()` for safety
3. **Optimize Batch Sizes**: Choose appropriate batch sizes for your data
4. **Validate Input**: Check for null or empty collections when appropriate
5. **Consider Memory Usage**: Be mindful of materializing large collections
6. **Use Appropriate Extensions**: Choose the right extension for your use case
