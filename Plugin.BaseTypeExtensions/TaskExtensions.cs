
namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for working with <see cref="Task"/> and <see cref="Task{TResult}"/> objects, including optional timeout patterns.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Waits for the task to complete within an optional timeout period.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to wait for.</param>
    /// <param name="timeout">The optional timeout duration. If null, zero, or negative, the method returns the original task (no timeout).</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// If timeout is null, zero, or negative: returns the original task.
    /// Otherwise: returns a task that completes when the original task completes or when the timeout expires.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="task"/> is null.</exception>
    /// <exception cref="TimeoutException">Thrown when the task doesn't complete within the specified timeout.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
    /// <remarks>
    /// This method provides a convenient way to add optional timeouts to any task. When no timeout is needed,
    /// simply pass null and the original task is returned unchanged. This avoids the overhead of timeout monitoring
    /// when it's not required.
    /// </remarks>
    public static Task<T> WaitBetterAsync<T>(this Task<T> task, TimeSpan? timeout, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (timeout.HasValue == false || timeout.Value <= TimeSpan.Zero)
        {
            return task;
        }

        // Cap very large timeouts to avoid ArgumentOutOfRangeException from Task.WaitAsync
        // The maximum is approximately 4294967294 milliseconds (about 49.7 days)
        var actualTimeout = timeout.Value;
        const long maxAllowedTimeoutMs = 4294967294L;
        if (actualTimeout.TotalMilliseconds > maxAllowedTimeoutMs)
        {
            actualTimeout = TimeSpan.FromMilliseconds(maxAllowedTimeoutMs);
        }

        return task.WaitAsync(actualTimeout, cancellationToken);
    }

    /// <summary>
    /// Waits for the task to complete within an optional timeout period.
    /// </summary>
    /// <param name="task">The task to wait for.</param>
    /// <param name="timeout">The optional timeout duration. If null, zero, or negative, the method returns the original task (no timeout).</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// If timeout is null, zero, or negative: returns the original task.
    /// Otherwise: returns a task that completes when the original task completes or when the timeout expires.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="task"/> is null.</exception>
    /// <exception cref="TimeoutException">Thrown when the task doesn't complete within the specified timeout.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
    /// <remarks>
    /// This method provides a convenient way to add optional timeouts to any task. When no timeout is needed,
    /// simply pass null and the original task is returned unchanged. This avoids the overhead of timeout monitoring
    /// when it's not required.
    /// </remarks>
    public static Task WaitBetterAsync(this Task task, TimeSpan? timeout, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (timeout.HasValue == false || timeout.Value <= TimeSpan.Zero)
        {
            return task;
        }

        // Cap very large timeouts to avoid ArgumentOutOfRangeException from Task.WaitAsync
        // The maximum is approximately 4294967294 milliseconds (about 49.7 days)
        var actualTimeout = timeout.Value;
        const long maxAllowedTimeoutMs = 4294967294L;
        if (actualTimeout.TotalMilliseconds > maxAllowedTimeoutMs)
        {
            actualTimeout = TimeSpan.FromMilliseconds(maxAllowedTimeoutMs);
        }

        return task.WaitAsync(actualTimeout, cancellationToken);
    }

#pragma warning disable CA1031 // Do not catch general exception types
    /// <summary>
    /// Executes the task in a fire-and-forget manner, safely handling exceptions and optionally invoking a handler.
    /// </summary>
    /// <param name="task">The task to execute.</param>
    /// <param name="onException">The action to invoke if an exception occurs.</param>
    public async static void RunAndHandleExceptionAsync(this Task task, Action<Exception> onException)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(onException);
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onException.Invoke(ex);
        }
    }
#pragma warning restore CA1031 // Do not catch general exception types
}
