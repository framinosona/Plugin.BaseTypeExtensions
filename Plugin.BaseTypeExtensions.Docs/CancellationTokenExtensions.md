# 🚫 CancellationTokenExtensions

Provides advanced timeout and async operations for CancellationToken objects.

## Overview

The `CancellationTokenExtensions` class enhances `CancellationToken` with timeout capabilities and task conversion methods for robust async cancellation patterns.

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

### Cascading Timeouts

```csharp
public async Task<ProcessingResult> ProcessWithCascadingTimeoutsAsync(
    IEnumerable<ProcessingStep> steps,
    CancellationToken cancellationToken)
{
    var results = new List<StepResult>();
    var remainingTime = TimeSpan.FromMinutes(10); // Total budget

    foreach (var step in steps)
    {
        var stepTimeout = TimeSpan.FromSeconds(remainingTime.TotalSeconds / 2);
        var stepToken = cancellationToken.WithTimeout(stepTimeout);

        var stopwatch = Stopwatch.StartNew();
        var result = await step.ExecuteAsync(stepToken);

        results.Add(result);
        remainingTime -= stopwatch.Elapsed;

        if (remainingTime <= TimeSpan.Zero)
        {
            throw new TimeoutException("Overall processing timeout exceeded");
        }
    }

    return new ProcessingResult(results);
}
```

## Practical Examples

### Web API Request Handling

```csharp
[HttpPost]
public async Task<ActionResult<ProcessingResult>> ProcessDataAsync(
    [FromBody] ProcessingRequest request,
    CancellationToken cancellationToken)
{
    // Add timeout to user's cancellation token
    var timeoutToken = cancellationToken.WithTimeout(TimeSpan.FromMinutes(5));

    try
    {
        var result = await dataProcessor.ProcessAsync(request.Data, timeoutToken);
        return Ok(result);
    }
    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
    {
        return StatusCode(499, "Client disconnected");
    }
    catch (OperationCanceledException)
    {
        return StatusCode(408, "Request timeout");
    }
}
```

### Background Service Coordination

```csharp
public class CoordinatedBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Each iteration has a maximum runtime
            var iterationToken = stoppingToken.WithTimeout(TimeSpan.FromMinutes(30));

            try
            {
                await ProcessBatchAsync(iterationToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Service is stopping
                break;
            }
            catch (OperationCanceledException)
            {
                // Iteration timeout - log and continue
                logger.LogWarning("Batch processing iteration timed out");
            }

            // Wait before next iteration
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}
```

### File Processing with Progress

```csharp
public async Task<ProcessingResult> ProcessLargeFileAsync(
    string filePath,
    IProgress<ProgressReport> progress,
    CancellationToken cancellationToken)
{
    // Overall timeout for file processing
    var timeoutToken = cancellationToken.WithTimeout(TimeSpan.FromHours(2));

    var fileInfo = new FileInfo(filePath);
    long totalBytes = fileInfo.Length;
    long processedBytes = 0;

    using var stream = File.OpenRead(filePath);
    var buffer = new byte[8192];

    while (processedBytes < totalBytes)
    {
        timeoutToken.ThrowIfCancellationRequested();

        var bytesRead = await stream.ReadAsync(buffer, timeoutToken);
        if (bytesRead == 0) break;

        await ProcessChunkAsync(buffer.AsSpan(0, bytesRead), timeoutToken);

        processedBytes += bytesRead;
        progress?.Report(new ProgressReport
        {
            Completed = processedBytes,
            Total = totalBytes
        });
    }

    return new ProcessingResult { ProcessedBytes = processedBytes };
}
```

## Integration with Other Extensions

### With TaskExtensions

```csharp
public async Task<T> ExecuteWithDualTimeoutAsync<T>(
    Task<T> task,
    TimeSpan tokenTimeout,
    int taskTimeoutMs,
    CancellationToken cancellationToken = default)
{
    // Apply timeout to cancellation token
    var timeoutToken = cancellationToken.WithTimeout(tokenTimeout);

    // Apply timeout to task
    return await task.WithTimeoutInMs(taskTimeoutMs, timeoutToken);
}
```

### Advanced Cancellation Patterns

```csharp
public class CancellationManager
{
    private readonly CancellationTokenSource _masterCts = new();

    public CancellationToken CreateScopedToken(TimeSpan timeout)\n    {\n        return _masterCts.Token.WithTimeout(timeout);\n    }\n    \n    public async Task<T> ExecuteWithScopeAsync<T>(\n        Func<CancellationToken, Task<T>> operation,\n        TimeSpan timeout)\n    {\n        var scopedToken = CreateScopedToken(timeout);\n        return await operation(scopedToken);\n    }\n    \n    public void CancelAll()\n    {\n        _masterCts.Cancel();\n    }\n    \n    public void Dispose()\n    {\n        _masterCts.Dispose();\n    }\n}\n```\n\n## Error Handling Strategies\n\n### Timeout vs Cancellation Detection\n\n```csharp\npublic async Task<T> ExecuteWithTimeoutDetectionAsync<T>(\n    Func<CancellationToken, Task<T>> operation,\n    CancellationToken userToken,\n    TimeSpan timeout)\n{\n    using var timeoutCts = new CancellationTokenSource();\n    using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(\n        userToken, timeoutCts.Token);\n    \n    var timeoutTask = Task.Delay(timeout, timeoutCts.Token);\n    var operationTask = operation(combinedCts.Token);\n    \n    var completedTask = await Task.WhenAny(operationTask, timeoutTask);\n    \n    if (completedTask == timeoutTask)\n    {\n        throw new TimeoutException($\"Operation timed out after {timeout}\");\n    }\n    \n    if (userToken.IsCancellationRequested)\n    {\n        throw new OperationCanceledException(\"Operation was cancelled by user\");\n    }\n    \n    return await operationTask;\n}\n```\n\n### Graceful Degradation\n\n```csharp\npublic async Task<T> ExecuteWithFallbackAsync<T>(\n    Func<CancellationToken, Task<T>> primaryOperation,\n    Func<Task<T>> fallbackOperation,\n    TimeSpan primaryTimeout)\n{\n    try \n    {\n        var timeoutToken = CancellationToken.None.WithTimeout(primaryTimeout);\n        return await primaryOperation(timeoutToken);\n    }\n    catch (OperationCanceledException)\n    {\n        // Primary operation timed out, try fallback\n        return await fallbackOperation();\n    }\n}\n```\n\n## Performance Considerations\n\n### Efficient Token Management\n- Properly dispose CancellationTokenSource objects\n- Avoid creating unnecessary token hierarchies\n- Use token caching for repeated operations\n\n```csharp\npublic class TokenCache\n{\n    private readonly ConcurrentDictionary<TimeSpan, CancellationToken> _tokenCache = new();\n    private readonly CancellationTokenSource _masterCts = new();\n    \n    public CancellationToken GetTokenWithTimeout(TimeSpan timeout)\n    {\n        return _tokenCache.GetOrAdd(timeout, t => _masterCts.Token.WithTimeout(t));\n    }\n    \n    public void InvalidateCache()\n    {\n        _tokenCache.Clear();\n    }\n}\n```\n\n### Memory Management\n\n```csharp\n// ✅ Good - Proper disposal\npublic async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation)\n{\n    using var cts = new CancellationTokenSource();\n    var timeoutToken = cts.Token.WithTimeout(TimeSpan.FromMinutes(5));\n    return await operation(timeoutToken);\n}\n\n// ❌ Avoid - No disposal\npublic async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation)\n{\n    var cts = new CancellationTokenSource(); // Will leak!\n    var timeoutToken = cts.Token.WithTimeout(TimeSpan.FromMinutes(5));\n    return await operation(timeoutToken);\n}\n```\n\n## Testing Patterns\n\n### Unit Testing Cancellation\n\n```csharp\n[Fact]\npublic async Task Operation_WithTimeout_ThrowsOnTimeout()\n{\n    // Arrange\n    var operation = async (CancellationToken ct) => \n    {\n        await Task.Delay(TimeSpan.FromSeconds(10), ct);\n        return \"completed\";\n    };\n    \n    // Act & Assert\n    var timeoutToken = CancellationToken.None.WithTimeout(TimeSpan.FromMilliseconds(100));\n    \n    await Assert.ThrowsAsync<OperationCanceledException>(\n        () => operation(timeoutToken));\n}\n\n[Fact]\npublic async Task AsTask_WhenTokenCancelled_CompletesTask()\n{\n    // Arrange\n    using var cts = new CancellationTokenSource();\n    var cancellationTask = cts.Token.AsTask();\n    \n    // Act\n    cts.Cancel();\n    \n    // Assert\n    await Assert.ThrowsAsync<OperationCanceledException>(\n        () => cancellationTask);\n}\n```\n\n## Best Practices\n\n1. **Always Handle OperationCanceledException**: Distinguish between user cancellation and timeout\n2. **Dispose CancellationTokenSource**: Use `using` statements or proper disposal\n3. **Reasonable Timeouts**: Choose timeouts based on actual operation characteristics\n4. **Propagate Cancellation**: Always pass cancellation tokens through async call chains\n5. **Test Cancellation Scenarios**: Unit test both timeout and cancellation paths\n6. **Log Cancellation Events**: Track timeout vs user cancellation for monitoring\n7. **Avoid Token Proliferation**: Don't create unnecessary token hierarchies
