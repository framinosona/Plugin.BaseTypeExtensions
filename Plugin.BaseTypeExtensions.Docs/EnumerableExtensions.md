# EnumerableExtensions

Provides powerful LINQ-style operations and utilities for IEnumerable collections.

## Overview

The `EnumerableExtensions` class offers enhanced collection operations including chunking, batch processing, conditional operations, and advanced LINQ extensions.

## API Reference

<xref:Plugin.BaseTypeExtensions.EnumerableExtensions>

## Collection Synchronization

### UpdateFrom Methods

The `UpdateFrom` family of methods provides powerful synchronization capabilities between collections, allowing you to efficiently add, update, and remove items based on comparison logic.

#### `UpdateFrom<T>(IEnumerable<T> output, IEnumerable<T>? addedItems, IEnumerable<T>? removedItems, Action<T>? addAction, Action<T>? removeAction)`
Directly adds and removes specified items from a collection.

```csharp
var currentItems = new List<string> { "a", "b", "c" };
var toAdd = new[] { "d", "e" };
var toRemove = new[] { "b" };

currentItems.UpdateFrom(
    addedItems: toAdd,
    removedItems: toRemove,
    addAction: item => currentItems.Add(item),
    removeAction: item => currentItems.Remove(item)
);
// Result: ["a", "c", "d", "e"]
```

#### `UpdateFrom<TInput, TOutput>(IEnumerable<TOutput> output, IEnumerable<TInput> input, Func<TInput, TOutput, bool> areRepresentingTheSameItem, Action<TInput> addAction, Action<TOutput> removeAction)`
Synchronizes collections by comparing items and performing add/remove operations.

```csharp
var users = new ObservableCollection<UserViewModel>();
var newUserData = await GetUsersFromApi();

users.UpdateFrom(
    input: newUserData,
    areRepresentingTheSameItem: (apiUser, vmUser) => apiUser.Id == vmUser.Id,
    addAction: apiUser => users.Add(new UserViewModel(apiUser)),
    removeAction: vmUser => users.Remove(vmUser)
);
```

#### `UpdateFrom<TInput, TOutput>(IEnumerable<TOutput> output, IEnumerable<TInput> input, Func<TInput, TOutput, bool> areRepresentingTheSameItem, Func<TInput, TOutput, bool> areRepresentingTheSameValue, Action<TInput> addAction, Action<TOutput, TInput> updateAction, Action<TOutput> removeAction)`
Synchronizes collections with add, update, and remove operations.

```csharp
var viewModels = new ObservableCollection<ProductViewModel>();
var products = await GetProductsFromApi();

viewModels.UpdateFrom(
    input: products,
    areRepresentingTheSameItem: (product, vm) => product.Id == vm.Id,
    areRepresentingTheSameValue: (product, vm) =>
        product.Name == vm.Name && product.Price == vm.Price,
    addAction: product => viewModels.Add(new ProductViewModel(product)),
    updateAction: (vm, product) => vm.UpdateFrom(product),
    removeAction: vm => viewModels.Remove(vm)
);
```

#### `UpdateFrom<TInput, TOutput>(IEnumerable<TOutput> output, IEnumerable<TInput> input, Func<TInput, TOutput, bool> areRepresentingTheSameItem, Func<TInput, TOutput> fromInputTypeToOutputTypeConversion, Action<TOutput> addAction, Action<TOutput> removeAction)`
Synchronizes collections with type conversion.

```csharp
var viewModels = new ObservableCollection<UserViewModel>();
var dtos = await GetUserDtosFromApi();

viewModels.UpdateFrom(
    input: dtos,
    areRepresentingTheSameItem: (dto, vm) => dto.UserId == vm.Id,
    fromInputTypeToOutputTypeConversion: dto => new UserViewModel(dto),
    addAction: vm => viewModels.Add(vm),
    removeAction: vm => viewModels.Remove(vm)
);
```

### UpdateFromAsync Methods

Async versions of UpdateFrom methods for scenarios involving async operations like database access or API calls.

#### `UpdateFromAsync<T>(IEnumerable<T> output, IEnumerable<T>? addedItems, IEnumerable<T>? removedItems, Func<T, CancellationToken, Task>? addAction, Func<T, CancellationToken, Task>? removeAction, CancellationToken cancellationToken)`
Asynchronously adds and removes items with cancellation support.

```csharp
var cacheItems = GetCacheItems();
var itemsToAdd = new[] { item1, item2 };
var itemsToRemove = new[] { item3 };

await cacheItems.UpdateFromAsync(
    addedItems: itemsToAdd,
    removedItems: itemsToRemove,
    addAction: async (item, ct) => await SaveToCacheAsync(item, ct),
    removeAction: async (item, ct) => await RemoveFromCacheAsync(item, ct),
    cancellationToken: cancellationToken
);
```

#### `UpdateFromAsync<TInput, TOutput>(IEnumerable<TOutput> output, IEnumerable<TInput> input, Func<TInput, TOutput, bool> areRepresentingTheSameItem, Func<TInput, CancellationToken, Task> addAction, Func<TOutput, CancellationToken, Task> removeAction, CancellationToken cancellationToken)`
Asynchronously synchronizes collections with comparison logic.

```csharp
var entities = await dbContext.Users.ToListAsync();
var dtos = await GetUpdatedUsersFromApi();

await entities.UpdateFromAsync(
    input: dtos,
    areRepresentingTheSameItem: (dto, entity) => dto.Id == entity.Id,
    addAction: async (dto, ct) =>
    {
        var entity = new User { Id = dto.Id, Name = dto.Name };
        dbContext.Users.Add(entity);
        await dbContext.SaveChangesAsync(ct);
    },
    removeAction: async (entity, ct) =>
    {
        dbContext.Users.Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    },
    cancellationToken: cancellationToken
);
```

#### `UpdateFromAsync<TInput, TOutput>(IEnumerable<TOutput> output, IEnumerable<TInput> input, Func<TInput, TOutput, bool> areRepresentingTheSameItem, Func<TInput, TOutput, bool> areRepresentingTheSameValue, Func<TInput, CancellationToken, Task> addAction, Func<TOutput, TInput, CancellationToken, Task> updateAction, Func<TOutput, CancellationToken, Task> removeAction, CancellationToken cancellationToken)`
Asynchronously synchronizes with add, update, and remove operations.

```csharp
var products = await dbContext.Products.ToListAsync();
var updates = await GetProductUpdatesFromApi();

await products.UpdateFromAsync(
    input: updates,
    areRepresentingTheSameItem: (update, product) => update.Id == product.Id,
    areRepresentingTheSameValue: (update, product) =>
        update.Price == product.Price && update.Stock == product.Stock,
    addAction: async (update, ct) =>
    {
        dbContext.Products.Add(MapToEntity(update));
        await dbContext.SaveChangesAsync(ct);
    },
    updateAction: async (product, update, ct) =>
    {
        product.Price = update.Price;
        product.Stock = update.Stock;
        await dbContext.SaveChangesAsync(ct);
    },
    removeAction: async (product, ct) =>
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(ct);
    },
    cancellationToken: cancellationToken
);
```

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
