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

    [Fact]
    public async Task WithTimeoutInMs_GenericThrowsOnTimeout()
    {
        var tcs = new TaskCompletionSource<int>();
        Func<Task> action = () => tcs.Task.WithTimeoutInMs(5);
        await action.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WithTimeoutInMs_ThrowsWhenTimeoutNegative()
    {
        var task = Task.CompletedTask;
        Func<Task> action = () => task.WithTimeoutInMs(-1);
        await action.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Timeout must be zero or positive*");
    }

    [Fact]
    public async Task WithTimeoutInMs_GenericThrowsWhenTimeoutNegative()
    {
        var task = Task.FromResult(42);
        Func<Task> action = () => task.WithTimeoutInMs(-1);
        var exception = await action.Should().ThrowAsync<ArgumentException>();
        exception.WithMessage("*Timeout must be zero or positive*")
            .And.ParamName.Should().Be("timeout");
    }

    [Fact]
    public async Task WithTimeoutInMs_NonGenericWorks()
    {
        var task = Task.Delay(1);
        await task.WithTimeoutInMs(1000);
    }

    [Fact]
    public async Task WithTimeoutInMs_NonGenericThrowsOnTimeout()
    {
        var tcs = new TaskCompletionSource();
        Func<Task> action = () => tcs.Task.WithTimeoutInMs(5);
        await action.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WithTimeoutInMs_GenericCompletesSuccessfully()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = await task.WithTimeoutInMs(1000);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task WithTimeoutInMs_GenericZeroTimeout_CompletesImmediately()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = await task.WithTimeoutInMs(0);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task WithTimeoutInMs_NonGenericZeroTimeout_CompletesImmediately()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act & Assert
        await task.WithTimeoutInMs(0); // Should not throw
    }

    [Fact]
    public async Task WithTimeoutInMs_CapsVeryLargeTimeout()
    {
        // Arrange
        var task = Task.FromResult(42);
        const long veryLargeTimeout = 5000000000L; // Larger than max allowed

        // Act & Assert - should not throw ArgumentOutOfRangeException
        var result = await task.WithTimeoutInMs(veryLargeTimeout);
        result.Should().Be(42);
    }

    [Fact]
    public void WithTimeoutInMs_NullTask_ThrowsArgumentNullException()
    {
        // Arrange
        Task? nullTask = null;

        // Act & Assert
        Action act = () => nullTask!.WithTimeoutInMs(1000);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WithTimeoutInMs_GenericNullTask_ThrowsArgumentNullException()
    {
        // Arrange
        Task<int>? nullTask = null;

        // Act & Assert
        Action act = () => nullTask!.WithTimeoutInMs(1000);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task WithOptionalTimeoutInMs_GenericReturnsSameTaskWhenDisabled()
    {
        var task = Task.FromResult(42);
        var result = task.WithOptionalTimeoutInMs(0);
        result.Should().BeSameAs(task);
        (await result).Should().Be(42);
    }

    [Fact]
    public async Task WithOptionalTimeoutInMs_GenericTimesOut()
    {
        var tcs = new TaskCompletionSource<int>();
        Func<Task> action = () => tcs.Task.WithOptionalTimeoutInMs(5);
        await action.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WithOptionalTimeoutInMs_NonGenericReturnsSameTaskWhenDisabled()
    {
        var task = Task.CompletedTask;
        var result = task.WithOptionalTimeoutInMs(0);
        result.Should().BeSameAs(task);
        await result;
    }

    [Fact]
    public async Task WithOptionalTimeoutInMs_NonGenericTimesOut()
    {
        var tcs = new TaskCompletionSource();
        Func<Task> action = () => tcs.Task.WithOptionalTimeoutInMs(5);
        await action.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WithOptionalTimeoutInMs_GenericNegativeTimeout_ReturnsSameTask()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = task.WithOptionalTimeoutInMs(-100);

        // Assert
        result.Should().BeSameAs(task);
        (await result).Should().Be(42);
    }

    [Fact]
    public async Task WithOptionalTimeoutInMs_NonGenericNegativeTimeout_ReturnsSameTask()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act
        var result = task.WithOptionalTimeoutInMs(-100);

        // Assert
        result.Should().BeSameAs(task);
        await result; // Should complete successfully
    }

    [Fact]
    public void WithOptionalTimeoutInMs_GenericNullTask_ThrowsArgumentNullException()
    {
        // Arrange
        Task<int>? nullTask = null;

        // Act & Assert
        Action act = () => nullTask!.WithOptionalTimeoutInMs(1000);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WithOptionalTimeoutInMs_NonGenericNullTask_ThrowsArgumentNullException()
    {
        // Arrange
        Task? nullTask = null;

        // Act & Assert
        Action act = () => nullTask!.WithOptionalTimeoutInMs(1000);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task WithTimeoutInMs_WithCancellationToken_RespectsToken()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var cts = new CancellationTokenSource();

        // Act
        var timeoutTask = tcs.Task.WithTimeoutInMs(10000, cts.Token);
        cts.Cancel();

        // Assert
        Func<Task> act = () => timeoutTask;
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task WithOptionalTimeoutInMs_WithCancellationToken_RespectsToken()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var cts = new CancellationTokenSource();

        // Act
        var timeoutTask = tcs.Task.WithOptionalTimeoutInMs(10000, cts.Token);
        cts.Cancel();

        // Assert
        Func<Task> act = () => timeoutTask;
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
