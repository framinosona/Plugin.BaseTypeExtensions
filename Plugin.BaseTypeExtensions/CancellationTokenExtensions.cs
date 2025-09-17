using System.Runtime.CompilerServices;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="CancellationToken"/> operations and utility functions.
/// </summary>
public static class CancellationTokenExtensions
{
    #region Timeout Operations

    /// <summary>
    /// Creates a new CancellationToken that will be cancelled after the specified timeout.
    /// </summary>
    /// <param name="token">The original CancellationToken.</param>
    /// <param name="timeout">The timeout duration.</param>
    /// <returns>A CancellationToken that combines the original token with a timeout.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when timeout is negative.</exception>
    public static CancellationToken WithTimeout(this CancellationToken token, TimeSpan timeout)
    {
        if (timeout < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout cannot be negative.");
        }

        if (timeout == TimeSpan.Zero)
        {
            return new CancellationToken(canceled: true);
        }

        if (token.IsCancellationRequested)
        {
            return token;
        }

        using var timeoutCts = new CancellationTokenSource(timeout);
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(token, timeoutCts.Token);

        return combinedCts.Token;
    }

    /// <summary>
    /// Creates a new CancellationToken that will be cancelled after the specified timeout in milliseconds.
    /// </summary>
    /// <param name="token">The original CancellationToken.</param>
    /// <param name="timeoutMs">The timeout in milliseconds.</param>
    /// <returns>A CancellationToken that combines the original token with a timeout.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when timeoutMs is negative.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CancellationToken WithTimeout(this CancellationToken token, int timeoutMs)
    {
        return token.WithTimeout(TimeSpan.FromMilliseconds(timeoutMs));
    }

    #endregion

    #region Task Conversion

    /// <summary>
    /// Converts the CancellationToken to a Task that completes when the token is cancelled.
    /// </summary>
    /// <param name="token">The CancellationToken to convert.</param>
    /// <returns>A Task that completes when the token is cancelled.</returns>
    public static Task AsTask(this CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            return Task.FromCanceled(token);
        }

        var tcs = new TaskCompletionSource<bool>();
        token.Register(() => tcs.TrySetCanceled(token));

        return tcs.Task;
    }

    #endregion

    #region Cancellation Utilities

    /// <summary>
    /// Throws an OperationCanceledException if cancellation is requested after the specified timeout.
    /// </summary>
    /// <param name="token">The CancellationToken to check.</param>
    /// <param name="timeout">The timeout duration to wait before throwing.</param>
    /// <exception cref="OperationCanceledException">Thrown when the token is cancelled or timeout expires.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when timeout is negative.</exception>
    public static void ThrowIfCanceledAfter(this CancellationToken token, TimeSpan timeout)
    {
        if (timeout < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout cannot be negative.");
        }

        if (token.IsCancellationRequested)
        {
            token.ThrowIfCancellationRequested();
        }

        if (timeout == TimeSpan.Zero)
        {
            throw new OperationCanceledException("Timeout expired.", token);
        }

        if (token.WaitHandle.WaitOne(timeout))
        {
            token.ThrowIfCancellationRequested();
        }
        else
        {
            throw new OperationCanceledException("Timeout expired.", token);
        }
    }

    /// <summary>
    /// Waits for the CancellationToken to be signaled with a timeout.
    /// </summary>
    /// <param name="token">The CancellationToken to wait for.</param>
    /// <param name="timeoutMs">The timeout in milliseconds.</param>
    /// <returns>True if the token was signaled within the timeout; otherwise, false.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when timeoutMs is negative.</exception>
    public static bool WaitHandle(this CancellationToken token, int timeoutMs)
    {
        if (timeoutMs < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeoutMs), "Timeout cannot be negative.");
        }

        return token.WaitHandle.WaitOne(timeoutMs);
    }

    #endregion
}
