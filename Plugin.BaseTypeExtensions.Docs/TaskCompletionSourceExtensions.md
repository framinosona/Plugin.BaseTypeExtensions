# TaskCompletionSourceExtensions

Provides enhanced TaskCompletionSource operations.

## Overview

The `TaskCompletionSourceExtensions` class offers utilities for TaskCompletionSource including timeout operations, cancellation handling, and advanced completion patterns.

## API Reference

<xref:Plugin.BaseTypeExtensions.TaskCompletionSourceExtensions>

## Core Methods

### Timeout Operations

#### `SetResultWithTimeout<T>(TaskCompletionSource<T> tcs, T result, TimeSpan timeout)`
Sets the result with a timeout mechanism.

```csharp
var tcs = new TaskCompletionSource<string>();

// Set result with 5-second timeout
tcs.SetResultWithTimeout("Success", TimeSpan.FromSeconds(5));

var result = await tcs.Task; // Will complete with "Success" or timeout
```

### Cancellation Integration

#### `LinkToCancellationToken<T>(TaskCompletionSource<T> tcs, CancellationToken cancellationToken)`
Links the TaskCompletionSource to a cancellation token.

```csharp
var tcs = new TaskCompletionSource<int>();
using var cts = new CancellationTokenSource();

tcs.LinkToCancellationToken(cts.Token);

// Cancel after 3 seconds
cts.CancelAfter(TimeSpan.FromSeconds(3));

try
{
    var result = await tcs.Task;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled");
}
```

## Practical Examples

### Asynchronous Event Handling

```csharp
public class AsyncEventHandler
{
    private TaskCompletionSource<EventArgs> _eventTcs;

    public async Task<EventArgs> WaitForEventAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        _eventTcs = new TaskCompletionSource<EventArgs>();

        // Link to cancellation token
        _eventTcs.LinkToCancellationToken(cancellationToken);

        // Set up timeout
        _eventTcs.SetResultWithTimeout(null, timeout);

        // Subscribe to event
        SomeObject.SomeEvent += OnEventReceived;

        try
        {
            return await _eventTcs.Task;
        }
        finally
        {
            SomeObject.SomeEvent -= OnEventReceived;
        }
    }

    private void OnEventReceived(object sender, EventArgs e)
    {
        _eventTcs?.TrySetResult(e);
    }
}
```

### Producer-Consumer Pattern

```csharp
public class AsyncQueue<T>
{
    private readonly Queue<T> _queue = new();
    private readonly Queue<TaskCompletionSource<T>> _waiters = new();
    private readonly object _lock = new();

    public void Enqueue(T item)
    {
        TaskCompletionSource<T> waiter = null;

        lock (_lock)
        {
            if (_waiters.Count > 0)
            {
                waiter = _waiters.Dequeue();
            }
            else
            {
                _queue.Enqueue(item);
            }
        }

        waiter?.TrySetResult(item);
    }

    public async Task<T> DequeueAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            if (_queue.Count > 0)
            {
                return _queue.Dequeue();
            }
        }

        var tcs = new TaskCompletionSource<T>();
        tcs.LinkToCancellationToken(cancellationToken);
        tcs.SetResultWithTimeout(default(T), timeout);

        lock (_lock)
        {
            _waiters.Enqueue(tcs);
        }

        return await tcs.Task;
    }
}
```

## Best Practices

1. **Resource Cleanup**: Always clean up resources in finally blocks
2. **Exception Handling**: Use TrySetException for proper error propagation
3. **Cancellation Support**: Always support cancellation tokens
4. **Timeout Management**: Set reasonable timeouts for operations
5. **Thread Safety**: Ensure thread-safe access to TaskCompletionSource
