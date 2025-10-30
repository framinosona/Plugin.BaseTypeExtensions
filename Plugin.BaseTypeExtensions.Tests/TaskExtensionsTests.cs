namespace Plugin.BaseTypeExtensions.Tests;

public class TaskExtensionsTests
{
    [Fact]
    public void IsDone_ReflectsTaskStatus()
    {
        Task.CompletedTask.IsCompleted.Should().BeTrue();
        Task.FromCanceled(new CancellationToken(true)).IsCompleted.Should().BeTrue();
        Task.FromException(new InvalidOperationException()).IsCompleted.Should().BeTrue();

        var tcs = new TaskCompletionSource();
        tcs.Task.IsCompleted.Should().BeFalse();
    }

    #region WaitAsync<T> tests

    [Fact]
    public async Task WaitAsync_Generic_NullTimeout_ReturnsSameTask()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = task.WaitBetterAsync(null);

        // Assert
        result.Should().BeSameAs(task);
        (await result).Should().Be(42);
    }

    [Fact]
    public async Task WaitAsync_Generic_ZeroTimeout_ReturnsSameTask()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = task.WaitBetterAsync(TimeSpan.Zero);

        // Assert
        result.Should().BeSameAs(task);
        (await result).Should().Be(42);
    }

    [Fact]
    public async Task WaitAsync_Generic_NegativeTimeout_ReturnsSameTask()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = task.WaitBetterAsync(TimeSpan.FromMilliseconds(-1));

        // Assert
        result.Should().BeSameAs(task);
        (await result).Should().Be(42);
    }

    [Fact]
    public async Task WaitAsync_Generic_ValidTimeout_CompletesSuccessfully()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = await task.WaitBetterAsync(TimeSpan.FromSeconds(1));

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task WaitAsync_Generic_ValidTimeout_ThrowsTimeoutException()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();

        // Act & Assert
        Func<Task> act = () => tcs.Task.WaitBetterAsync(TimeSpan.FromMilliseconds(50));
        await act.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WaitAsync_Generic_WithCancellationToken_RespectsToken()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var cts = new CancellationTokenSource();

        // Act
        var waitTask = tcs.Task.WaitBetterAsync(TimeSpan.FromSeconds(10), cts.Token);
        cts.Cancel();

        // Assert
        Func<Task> act = () => waitTask;
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    #endregion

    #region WaitAsync (non-generic) tests

    [Fact]
    public async Task WaitAsync_NonGeneric_NullTimeout_ReturnsSameTask()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act
        var result = task.WaitBetterAsync(null);

        // Assert
        result.Should().BeSameAs(task);
        await result; // Should complete successfully
    }

    [Fact]
    public async Task WaitAsync_NonGeneric_ZeroTimeout_ReturnsSameTask()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act
        var result = task.WaitBetterAsync(TimeSpan.Zero);

        // Assert
        result.Should().BeSameAs(task);
        await result; // Should complete successfully
    }

    [Fact]
    public async Task WaitAsync_NonGeneric_NegativeTimeout_ReturnsSameTask()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act
        var result = task.WaitBetterAsync(TimeSpan.FromMilliseconds(-1));

        // Assert
        result.Should().BeSameAs(task);
        await result; // Should complete successfully
    }

    [Fact]
    public async Task WaitAsync_NonGeneric_ValidTimeout_CompletesSuccessfully()
    {
        // Arrange
        var task = Task.Delay(10); // Short delay

        // Act & Assert
        await task.WaitBetterAsync(TimeSpan.FromSeconds(1)); // Should not throw
    }

    [Fact]
    public async Task WaitAsync_NonGeneric_ValidTimeout_ThrowsTimeoutException()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act & Assert
        Func<Task> act = () => tcs.Task.WaitBetterAsync(TimeSpan.FromMilliseconds(50));
        await act.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WaitAsync_NonGeneric_WithCancellationToken_RespectsToken()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cts = new CancellationTokenSource();

        // Act
        var waitTask = tcs.Task.WaitBetterAsync(TimeSpan.FromSeconds(10), cts.Token);
        cts.Cancel();

        // Assert
        Func<Task> act = () => waitTask;
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    #endregion

    #region Integration and edge case tests

    [Fact]
    public async Task WaitAsync_LargeTimeout_DoesNotThrow()
    {
        // Arrange
        var task = Task.FromResult(42);
        var largeTimeout = TimeSpan.FromDays(365); // Very large timeout

        // Act & Assert - should not throw, even with very large timeouts
        var result = await task.WaitBetterAsync(largeTimeout);
        result.Should().Be(42);
    }

    [Fact]
    public async Task WaitAsync_CancelledTask_PropagatesCancellation()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var cancelledTask = Task.FromCanceled<int>(cts.Token);

        // Act & Assert
        Func<Task> act = () => cancelledTask.WaitBetterAsync(TimeSpan.FromSeconds(1));
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task WaitAsync_FaultedTask_PropagatesException()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        var faultedTask = Task.FromException<int>(exception);

        // Act & Assert
        Func<Task> act = () => faultedTask.WaitBetterAsync(TimeSpan.FromSeconds(1));
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Test exception");
    }

    #endregion
}
