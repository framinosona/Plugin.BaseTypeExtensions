# ⚡ TaskExtensions

Provides advanced async operations and timeout management for Task objects.

## Overview

The `TaskExtensions` class enhances `Task` and `Task<T>` with timeout capabilities, cancellation token integration, and robust async operation patterns.

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
Very large timeout values are automatically capped to prevent integer overflow:

```csharp
// Large timeouts are capped to int.MaxValue - 100
var result = await task.WithTimeoutInMs(int.MaxValue); // Safely capped
```

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

### File Operations

```csharp
public async Task<string> ReadFileWithTimeoutAsync(string path, int timeoutMs = 5000)
{
    var readTask = File.ReadAllTextAsync(path);
    return await readTask.WithTimeoutInMs(timeoutMs);
}
```

### Configurable Timeouts

```csharp
public class ServiceSettings
{
    public int? DefaultTimeoutMs { get; set; } = 30000;
    public int? LongOperationTimeoutMs { get; set; } = null; // No timeout
}

public async Task<T> ExecuteOperationAsync<T>(
    Func<Task<T>> operation,
    ServiceSettings settings,
    CancellationToken cancellationToken = default)
{
    var task = operation();
    return await task.WithTimeoutInMs(settings.DefaultTimeoutMs, cancellationToken);
}
```

## Integration Patterns

### With CancellationTokenExtensions

```csharp
using var cts = new CancellationTokenSource();
var timeoutToken = cts.Token.WithTimeout(TimeSpan.FromSeconds(5));

// Combined timeout approaches
var result = await longTask
    .WithTimeoutInMs(10000, timeoutToken); // Both timeout mechanisms
```

### Retry with Timeout

```csharp
public async Task<T> ExecuteWithRetryAsync<T>(
    Func<Task<T>> operation,
    int maxRetries = 3,
    int timeoutMs = 5000)
{
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            return await operation().WithTimeoutInMs(timeoutMs);
        }
        catch (TimeoutException) when (i < maxRetries - 1)
        {
            await Task.Delay(1000 * (i + 1)); // Exponential backoff
        }
    }

    throw new TimeoutException($"Operation failed after {maxRetries} retries");
}
```

### Parallel Operations with Individual Timeouts

```csharp
public async Task<T[]> ExecuteParallelWithTimeoutsAsync<T>(
    IEnumerable<Func<Task<T>>> operations,
    int timeoutMs = 10000)
{
    var tasks = operations
        .Select(op => op().WithTimeoutInMs(timeoutMs))
        .ToArray();

    return await Task.WhenAll(tasks);
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

## Error Handling Patterns

### Graceful Degradation

```csharp
public async Task<T> TryWithTimeoutAsync<T>(
    Task<T> task,
    int timeoutMs,
    T defaultValue = default)
{
    try
    {
        return await task.WithTimeoutInMs(timeoutMs);
    }
    catch (TimeoutException)
    {
        return defaultValue;
    }
}
```

### Logging and Monitoring

```csharp
public async Task<T> ExecuteWithLoggingAsync<T>(
    Task<T> task,
    int timeoutMs,
    ILogger logger)
{
    var stopwatch = Stopwatch.StartNew();

    try
    {
        var result = await task.WithTimeoutInMs(timeoutMs);
        logger.LogInformation("Operation completed in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
        return result;
    }
    catch (TimeoutException)
    {
        logger.LogWarning("Operation timed out after {TimeoutMs}ms", timeoutMs);
        throw;
    }
    finally
    {
        stopwatch.Stop();
    }
}
```

## Common Anti-Patterns

### ❌ Avoid Multiple Timeout Layers
```csharp
// Don't do this - multiple competing timeouts
await task
    .WithTimeoutInMs(5000)
    .WithTimeoutInMs(3000); // Confusing behavior
```

### ❌ Avoid Very Short Timeouts
```csharp
// Avoid timeouts shorter than expected operation time
await quickTask.WithTimeoutInMs(1); // Likely to always timeout
```

### ✅ Recommended Patterns
```csharp
// Single, appropriate timeout
await task.WithTimeoutInMs(30000);

// Configuration-driven timeouts
await task.WithTimeoutInMs(config.TimeoutMs ?? 10000);

// Progressive timeouts
var timeout = isRetry ? baseTimeout * 2 : baseTimeout;
await task.WithTimeoutInMs(timeout);
```

## Integration with ASP.NET Core

### Controller Actions

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<User>> GetUserAsync(int id, CancellationToken cancellationToken)
{
    var task = userService.GetUserByIdAsync(id);
    var user = await task.WithTimeoutInMs(5000, cancellationToken);

    return user is not null ? Ok(user) : NotFound();
}
```

### Background Services

```csharp
public class BackgroundProcessor : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var processTask = ProcessBatchAsync();
            await processTask.WithTimeoutInMs(60000, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
```

## Best Practices

1. **Choose Appropriate Timeouts**: Base on expected operation duration + buffer
2. **Use Cancellation Tokens**: Always pass through cancellation tokens
3. **Handle Timeout Exceptions**: Provide meaningful error handling
4. **Consider Retry Logic**: Implement exponential backoff for transient failures
5. **Monitor Performance**: Log operation durations for timeout tuning
6. **Test Timeout Scenarios**: Unit test both success and timeout cases
