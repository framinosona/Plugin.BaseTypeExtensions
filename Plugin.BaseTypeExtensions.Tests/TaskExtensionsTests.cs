using Plugin.BaseTypeExtensions;

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
        await action.Should().ThrowAsync<ArgumentException>();
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
}
