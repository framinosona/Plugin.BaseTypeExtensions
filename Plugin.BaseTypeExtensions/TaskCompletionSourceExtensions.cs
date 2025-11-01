namespace Plugin.BaseTypeExtensions;

/// <summary>
/// A family of extension methods that ensure that the <see cref="TaskCompletionSource"/> is properly cleaned up in the case of a timeout either
/// from the cancellation token or the timeout specified.<br/><br/>
///
/// Always bear in mind that if the <see cref="TaskCompletionSource"/> is never completed, its task will never complete. Even though the underlying
/// Task is not actually in a "scheduler" (since TCS tasks are <see href="https://blog.stephencleary.com/2015/04/a-tour-of-task-part-10-promise-tasks.html">Promise Tasks</see>)
/// never completing tasks, of any type, is <see href="https://devblogs.microsoft.com/pfxteam/dont-forget-to-complete-your-tasks/">generally considered a bug</see>.
/// </summary>
public static class TaskCompletionSourceExtensions
{
    /// <summary>
    /// Attempts to complete the <see cref="TaskCompletionSource{T}"/> with a result or an exception.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="tcs">The <see cref="TaskCompletionSource{T}"/> to complete.</param>
    /// <param name="result">The result to set if no exception is provided.</param>
    /// <param name="exception">The exception to set if provided. If null, the result will be used instead.</param>
    /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c> if the task was already completed.</returns>
    public static bool TrySetResultOrException<T>(this TaskCompletionSource<T> tcs, T result, Exception? exception = null)
    {
        ArgumentNullException.ThrowIfNull(tcs);

        if (tcs.Task.IsCompleted)
        {
            return false;
        }

        return exception != null ? tcs.TrySetException(exception) : tcs.TrySetResult(result);
    }

    /// <summary>
    /// Attempts to complete the <see cref="TaskCompletionSource"/> with success or an exception.
    /// </summary>
    /// <param name="tcs">The <see cref="TaskCompletionSource"/> to complete.</param>
    /// <param name="exception">The exception to set if provided. If null, the task will be marked as successfully completed.</param>
    /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c> if the task was already completed.</returns>
    public static bool TrySetResultOrException(this TaskCompletionSource tcs, Exception? exception = null)
    {
        ArgumentNullException.ThrowIfNull(tcs);

        if (tcs.Task.IsCompleted)
        {
            return false;
        }

        return exception != null ? tcs.TrySetException(exception) : tcs.TrySetResult();
    }

}
