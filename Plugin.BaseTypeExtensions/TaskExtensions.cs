
namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for working with <see cref="Task"/> and <see cref="Task{TResult}"/> objects, including fire-and-forget and timeout patterns.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Determines whether the task has completed, been canceled, or faulted.
    /// </summary>
    /// <param name="input">The task to check.</param>
    /// <returns>True if the task is completed, canceled, or faulted; otherwise, false.</returns>
    [Obsolete("Use Task.IsCompleted instead")]
    public static bool IsDone(this Task input)
    {
        ArgumentNullException.ThrowIfNull(input);
        switch (input.Status)
        {
            case TaskStatus.Created:
            case TaskStatus.WaitingForActivation:
            case TaskStatus.WaitingToRun:
            case TaskStatus.Running:
            case TaskStatus.WaitingForChildrenToComplete:
                return false;
            case TaskStatus.RanToCompletion:
            case TaskStatus.Canceled:
            case TaskStatus.Faulted:
                return true;
            default:
                throw new ArgumentOutOfRangeException(nameof(input), input.Status, "Unexpected TaskStatus value.");
        }
    }

    /// <summary>
    /// Sets up a timeout-monitor on the given task. This is essentially a wrapper around
    /// <see cref="System.Threading.Tasks.Task.WaitAsync(TimeSpan)"/> with one major difference: it doesn't
    /// accept -1 as a means of "wait indefinitely" (we never considered this "shortcut value" to be a good
    /// idea so we just throw an argument-exception for all negative values).
    /// </summary>
    /// <param name="task">The task to monitor.</param>
    /// <param name="timeout">The timeout in milliseconds. Must be zero or positive. Negative values will cause a <see cref="ArgumentException"/> to be thrown.</param>
    /// <param name="cancellationToken">(optional) The cancellation token that co-monitors the waiting mechanism.</param>
    /// <typeparam name="T">The type of the task</typeparam>
    /// <returns>The hybridized task that you can await on</returns>
    /// <exception cref="ArgumentException">Thrown when the timeout is negative.</exception>
    /// <exception cref="TimeoutException">Thrown when the task didn't complete within the specified timeout.</exception>
    /// <remarks>
    /// - <b>If you want to monitor a <see cref="TaskCompletionSource"/> then it's strongly advised that you use <see cref="TaskCompletionSourceExtensions.WaitAndFossilizeTaskOnTimeoutAsync(TaskCompletionSource, long, CancellationToken)"/> instead.</b><br/><br/>
    /// - <b>It's also strongly advised to always link the task you provide with a <see cref="CancellationToken"/> and pass the token to this method so as to minimize the risk of ending up with zombie tasks.</b><br/><br/>
    /// - If you don't link your task with a cancellation token then bear in mind that in the case of a <see cref="TimeoutException">TimeoutException</see> it's imperative that you perform the cleanup (cancellation) of your task yourself!<br/><br/>
    /// Forgetting to do so can (under certain conditions) result in a memory leak and even worst a zombie-promise-task in runtime that can potentially chronically bog-down the performance of your application!
    /// </remarks>
    public static Task<T> WithTimeoutInMs<T>(this Task<T> task, long timeout, CancellationToken cancellationToken = default)
    {
        if (timeout < 0) //note that this deviates from the behaviour of .WaitAsync() which does accept -1 milliseconds for some weird reason
        {
            throw new ArgumentException("Timeout must be zero or positive - negative values are not allowed", nameof(timeout));
        }

        // Task.WaitAsync has a maximum timeout limit - cap very large values to avoid ArgumentOutOfRangeException
        // The maximum is approximately 4294967294 milliseconds (about 49.7 days)
        const long maxAllowedTimeoutMs = 4294967294L; // Max timer duration in milliseconds
        if (timeout > maxAllowedTimeoutMs)
        {
            timeout = maxAllowedTimeoutMs;
        }

        ArgumentNullException.ThrowIfNull(task);
        return task.WaitAsync(TimeSpan.FromMilliseconds(timeout), cancellationToken);
    }

    /// <summary>
    /// Sets up a timeout-monitor on the given task. This is essentially a wrapper around
    /// <see cref="System.Threading.Tasks.Task.WaitAsync(TimeSpan)"/> with one major difference: it doesn't
    /// accept -1 as a means of "wait indefinitely" (we never considered this "shortcut value" to be a good
    /// idea so we just throw an argument-exception for all negative values).
    /// </summary>
    /// <param name="task">The task to monitor.</param>
    /// <param name="timeout">The timeout in milliseconds. Must be zero or positive. Negative values will cause a <see cref="ArgumentException"/> to be thrown.</param>
    /// <returns>The hybridized task that you can await on</returns>
    /// <param name="cancellationToken">(optional) The cancellation token that co-monitors the waiting mechanism.</param>
    /// <exception cref="ArgumentException">Thrown when the timeout is negative.</exception>
    /// <exception cref="TimeoutException">Thrown when the task didn't complete within the specified timeout.</exception>
    /// <remarks>
    /// - <b>If you want to monitor a <see cref="TaskCompletionSource"/> then it's strongly advised that you use <see cref="TaskCompletionSourceExtensions.WaitAndFossilizeTaskOnTimeoutAsync(TaskCompletionSource, long, CancellationToken)"/> instead.</b><br/><br/>
    /// - <b>It's also strongly advised to always link the task you provide with a <see cref="CancellationToken"/> and pass the token to this method so as to minimize the risk of ending up with zombie tasks.</b><br/><br/>
    /// - If you don't link your task with a cancellation token then bear in mind that in the case of a <see cref="TimeoutException">TimeoutException</see> it's imperative that you perform the cleanup (cancellation) of your task yourself!<br/><br/>
    /// Forgetting to do so can (under certain conditions) result in a memory leak and even worst a zombie-promise-task in runtime that can potentially chronically bog-down the performance of your application!
    /// </remarks>
    public static Task WithTimeoutInMs(this Task task, long timeout, CancellationToken cancellationToken = default)
    {
        if (timeout < 0) //note that this deviates from the behaviour of .WaitAsync() which does accept -1 milliseconds for some weird reason
        {
            throw new ArgumentException("Timeout must be zero or positive - negative values are not allowed", nameof(timeout));
        }

        // Task.WaitAsync has a maximum timeout limit - cap very large values to avoid ArgumentOutOfRangeException
        // The maximum is approximately 4294967294 milliseconds (about 49.7 days)
        const long maxAllowedTimeoutMs = 4294967294L; // Max timer duration in milliseconds
        if (timeout > maxAllowedTimeoutMs)
        {
            timeout = maxAllowedTimeoutMs;
        }

        ArgumentNullException.ThrowIfNull(task);
        return task.WaitAsync(TimeSpan.FromMilliseconds(timeout), cancellationToken);
    }

    /// <summary>
    /// Sets up a timeout-monitor on the given task. This is essentially a wrapper around
    /// <see cref="System.Threading.Tasks.Task.WaitAsync(TimeSpan)"/> with one major difference: if the timeout
    /// is zero or negative then it gets interpreted as "wait forever" and the method will just return the task itself.
    /// </summary>
    /// <param name="task">The task to monitor.</param>
    /// <param name="timeout">The timeout in milliseconds. If zero or negative it gets interpreted as "wait forever" and the method will just return the task itself.</param>
    /// <param name="cancellationToken">(optional) The cancellation token that co-monitors the waiting mechanism.</param>
    /// <returns>The hybridized task that you can await on.</returns>
    /// <exception cref="TimeoutException">Thrown when the task didn't complete within the specified timeout.</exception>
    /// <remarks>
    /// - <b>If you want to monitor a <see cref="TaskCompletionSource"/> then it's strongly advised that you use <see cref="TaskCompletionSourceExtensions.WaitAndFossilizeTaskOnTimeoutAsync(TaskCompletionSource, long, CancellationToken)"/> instead.</b><br/><br/>
    /// - <b>It's also strongly advised to always link the task you provide with a <see cref="CancellationToken"/> and pass the token to this method so as to minimize the risk of ending up with zombie tasks.</b><br/><br/>
    /// - If you don't link your task with a cancellation token then bear in mind that in the case of a <see cref="TimeoutException">TimeoutException</see> it's imperative that you perform the cleanup (cancellation) of your task yourself!<br/><br/>
    /// Forgetting to do so can (under certain conditions) result in a memory leak and even worst a zombie-promise-task in runtime that can potentially chronically bog-down the performance of your application!
    /// </remarks>
    public static Task<T> WithOptionalTimeoutInMs<T>(this Task<T> task, long timeout, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        return timeout <= 0
            ? task
            : task.WaitAsync(TimeSpan.FromMilliseconds(timeout), cancellationToken);
    }

    /// <summary>
    /// Sets up a timeout-monitor on the given task. This is essentially a wrapper around
    /// <see cref="System.Threading.Tasks.Task.WaitAsync(TimeSpan)"/> with one major difference: if the timeout
    /// is zero or negative then it gets interpreted as "wait forever" and the method will just return the task itself.
    /// </summary>
    /// <param name="task">The task to monitor.</param>
    /// <param name="timeout">The timeout in milliseconds. If zero or negative it gets interpreted as "wait forever" and the method will just return the task itself.</param>
    /// <param name="cancellationToken">(optional) The cancellation token that co-monitors the waiting mechanism.</param>
    /// <returns>The hybridized task that you can await on.</returns>
    /// <exception cref="TimeoutException">Thrown when the task didn't complete within the specified timeout.</exception>
    /// <remarks>
    /// - <b>If you want to monitor a <see cref="TaskCompletionSource"/> then it's strongly advised that you use <see cref="TaskCompletionSourceExtensions.WaitAndFossilizeTaskOnTimeoutAsync(TaskCompletionSource, long, CancellationToken)"/> instead.</b><br/><br/>
    /// - <b>It's also strongly advised to always link the task you provide with a <see cref="CancellationToken"/> and pass the token to this method so as to minimize the risk of ending up with zombie tasks.</b><br/><br/>
    /// - If you don't link your task with a cancellation token then bear in mind that in the case of a <see cref="TimeoutException">TimeoutException</see> it's imperative that you perform the cleanup (cancellation) of your task yourself!<br/><br/>
    /// Forgetting to do so can (under certain conditions) result in a memory leak and even worst a zombie-promise-task in runtime that can potentially chronically bog-down the performance of your application!
    /// </remarks>
    public static Task WithOptionalTimeoutInMs(this Task task, long timeout, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        return timeout <= 0
            ? task
            : task.WaitAsync(TimeSpan.FromMilliseconds(timeout), cancellationToken);
    }
}
