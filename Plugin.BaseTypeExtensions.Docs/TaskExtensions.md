# TaskExtensions

Provides advanced async operations and timeout management for Task objects.

## Overview

The `TaskExtensions` class enhances `Task` and `Task<T>` with timeout capabilities, cancellation token integration, and robust async operation patterns.

## API Reference

<xref:Plugin.BaseTypeExtensions.TaskExtensions>

## Core Methods

### Timeout Operations

#### `WithTimeoutInMs<T>(Task<T> task, int timeoutMs, CancellationToken cancellationToken = default)`
Adds timeout to Task<T> with optional cancellation token.

```csharp
// Basic timeout
var result = await longRunningTask.WithTimeoutInMs(5000); // 5 second timeout

// With cancellation token
var result = await apiCall.WithTimeoutInMs(10000, cancellationToken);
```

#### `WithTimeoutInMs(Task task, int timeoutMs, CancellationToken cancellationToken = default)`
Adds timeout to non-generic Task.

```csharp
// Non-generic task timeout
await backgroundTask.WithTimeoutInMs(30000); // 30 second timeout
```

#### `WithTimeoutInMs<T>(Task<T> task, int? optionalTimeoutMs, CancellationToken cancellationToken = default)`
Optional timeout - if null, task runs without timeout.

```csharp
// Configuration-driven timeout
int? configTimeout = GetConfiguredTimeout(); // might be null
var result = await dataTask.WithTimeoutInMs(configTimeout, cancellationToken);
```

## Advanced Features

### Timeout Capping
Very large timeout values are automatically capped to prevent integer overflow.

### Exception Handling
- **TimeoutException**: Thrown when operation exceeds timeout
- **TaskCanceledException**: Thrown when cancellation token is triggered
- **Original exceptions**: Preserved from the underlying task

```csharp
try
{
    var result = await slowTask.WithTimeoutInMs(1000);
}
catch (TimeoutException)
{
    // Handle timeout
}
catch (TaskCanceledException)
{
    // Handle cancellation
}
```

## Practical Examples

### API Call with Timeout

```csharp
public async Task<ApiResponse> CallApiAsync(string endpoint, CancellationToken cancellationToken = default)
{
    using var httpClient = new HttpClient();
    var task = httpClient.GetAsync(endpoint, cancellationToken);

    // 30 second timeout
    var response = await task.WithTimeoutInMs(30000, cancellationToken);

    return await response.Content.ReadFromJsonAsync<ApiResponse>();
}
```

### Database Operations

```csharp
public async Task<User[]> GetUsersAsync(int timeoutMs = 10000)
{
    var query = context.Users.ToListAsync();
    return await query.WithTimeoutInMs(timeoutMs);
}
```

## Performance Considerations

### Efficient Timeout Implementation
- Uses `Task.Delay` with cancellation for timeout mechanism
- No busy waiting or polling
- Proper cleanup of timeout tasks

### Memory Management
- Timeout tasks are properly disposed
- No memory leaks from abandoned timeout operations
- CancellationTokenSource cleanup

### Thread Safety
- All operations are thread-safe
- No shared state between calls
- Safe for concurrent usage

## Best Practices

1. **Choose Appropriate Timeouts**: Base on expected operation duration + buffer
2. **Use Cancellation Tokens**: Always pass through cancellation tokens
3. **Handle Timeout Exceptions**: Provide meaningful error handling
4. **Consider Retry Logic**: Implement exponential backoff for transient failures
5. **Monitor Performance**: Log operation durations for timeout tuning
6. **Test Timeout Scenarios**: Unit test both success and timeout cases
