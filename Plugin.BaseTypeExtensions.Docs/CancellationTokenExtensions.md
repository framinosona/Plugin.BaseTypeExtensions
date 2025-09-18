# CancellationTokenExtensions

Provides advanced timeout and async operations for CancellationToken objects.

## Overview

The `CancellationTokenExtensions` class enhances `CancellationToken` with timeout capabilities and task conversion methods for robust async cancellation patterns.

## API Reference

<xref:Plugin.BaseTypeExtensions.CancellationTokenExtensions>

## Core Methods

### Timeout Operations

#### `WithTimeout(CancellationToken token, TimeSpan timeout)`
Creates a new cancellation token that triggers after the specified timeout.

```csharp
using var cts = new CancellationTokenSource();
var timeoutToken = cts.Token.WithTimeout(TimeSpan.FromSeconds(30));

// Use in async operations
await SomeOperationAsync(timeoutToken);
```

### Task Conversion

#### `AsTask(CancellationToken token)`
Converts a CancellationToken to a Task that completes when the token is cancelled.

```csharp
var cancellationTask = cancellationToken.AsTask();

// Use in Task.WhenAny scenarios
var completedTask = await Task.WhenAny(operationTask, cancellationTask);

if (completedTask == cancellationTask)
{
    // Cancellation was requested
    throw new OperationCanceledException();
}
```

## Advanced Patterns

### Combined Cancellation Sources

```csharp
public async Task<T> ExecuteWithMultipleCancellationAsync<T>(
    Func<CancellationToken, Task<T>> operation,
    CancellationToken userToken,
    TimeSpan timeout)
{
    // Combine user cancellation with timeout
    var timeoutToken = userToken.WithTimeout(timeout);

    try
    {
        return await operation(timeoutToken);
    }
    catch (OperationCanceledException)
    {
        if (userToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("Operation was cancelled by user");
        }
        else
        {
            throw new TimeoutException($"Operation timed out after {timeout}");
        }
    }
}
```

## Performance Considerations

### Efficient Token Management
- Properly dispose CancellationTokenSource objects
- Avoid creating unnecessary token hierarchies
- Use token caching for repeated operations

### Memory Management
- Timeout tasks are properly disposed
- No memory leaks from abandoned timeout operations
- Proper resource cleanup

## Best Practices

1. **Always Handle OperationCanceledException**: Distinguish between user cancellation and timeout
2. **Dispose CancellationTokenSource**: Use `using` statements or proper disposal
3. **Reasonable Timeouts**: Choose timeouts based on actual operation characteristics
4. **Propagate Cancellation**: Always pass cancellation tokens through async call chains
5. **Test Cancellation Scenarios**: Unit test both timeout and cancellation paths
6. **Log Cancellation Events**: Track timeout vs user cancellation for monitoring
