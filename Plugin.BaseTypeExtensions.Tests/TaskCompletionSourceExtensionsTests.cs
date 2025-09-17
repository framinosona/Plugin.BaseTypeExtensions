namespace Plugin.BaseTypeExtensions.Tests;

public class TaskCompletionSourceExtensionsTests
{
    [Fact]
    public void WaitAndFossilizeTaskOnOptionalTimeoutAsync_ReturnsOriginalTaskWhenDisabled()
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var returnedTask = tcs.WaitAndFossilizeTaskOnOptionalTimeoutAsync(0);
        returnedTask.Should().BeSameAs(tcs.Task);
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnTimeoutAsync_ThrowsAndFossilizesOnTimeout()
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<Task> action = () => tcs.WaitAndFossilizeTaskOnTimeoutAsync(TimeSpan.FromMilliseconds(5));

        await action.Should().ThrowAsync<TimeoutException>();
        tcs.Task.IsFaulted.Should().BeTrue();
        tcs.Task.Exception!.InnerExceptions.Should().ContainSingle(e => e is TimeoutException);
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnTimeoutAsync_LongOverloadTimesOut()
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<Task> action = () => tcs.WaitAndFossilizeTaskOnTimeoutAsync(timeoutInMilliseconds: 5);

        await action.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnTimeoutAsync_CompletesWhenResultArrives()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        var waitTask = tcs.WaitAndFossilizeTaskOnTimeoutAsync(TimeSpan.FromMilliseconds(200));

        _ = Task.Run(async () =>
        {
            await Task.Delay(20);
            tcs.TrySetResult(5);
        });

        (await waitTask).Should().Be(5);
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnTimeoutAsync_CancelsWhenTokenTriggered()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cts = new CancellationTokenSource();
        var waitTask = tcs.WaitAndFossilizeTaskOnTimeoutAsync(TimeSpan.FromMilliseconds(200), cts.Token);
        cts.Cancel();

        Func<Task> action = () => waitTask;
        await action.Should().ThrowAsync<OperationCanceledException>();
        tcs.Task.IsCanceled.Should().BeTrue();
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnTimeoutAsync_NonGenericCancelsWhenTokenTriggered()
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cts = new CancellationTokenSource();
        var waitTask = tcs.WaitAndFossilizeTaskOnTimeoutAsync(TimeSpan.FromMilliseconds(200), cts.Token);
        cts.Cancel();

        Func<Task> action = () => waitTask;
        await action.Should().ThrowAsync<OperationCanceledException>();
        tcs.Task.IsCanceled.Should().BeTrue();
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnTimeoutAsync_NonGenericNegativeThrows()
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<Task> action = () => tcs.WaitAndFossilizeTaskOnTimeoutAsync(TimeSpan.FromMilliseconds(-1));
        await action.Should().ThrowAsync<ArgumentException>();
        tcs.Task.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnTimeoutAsync_GenericLongOverloadTimesOut()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<Task> action = () => tcs.WaitAndFossilizeTaskOnTimeoutAsync(timeoutInMilliseconds: 5);
        await action.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnTimeoutAsync_GenericNegativeThrows()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<Task> action = () => tcs.WaitAndFossilizeTaskOnTimeoutAsync(TimeSpan.FromMilliseconds(-1));
        await action.Should().ThrowAsync<ArgumentException>();
        tcs.Task.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public void WaitAndFossilizeTaskOnOptionalTimeoutAsync_GenericReturnsOriginalTaskWhenDisabled()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        tcs.WaitAndFossilizeTaskOnOptionalTimeoutAsync(TimeSpan.Zero).Should().BeSameAs(tcs.Task);
    }

    [Fact]
    public void WaitAndFossilizeTaskOnOptionalTimeoutAsync_GenericLongOverloadReturnsOriginalTaskWhenDisabled()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        tcs.WaitAndFossilizeTaskOnOptionalTimeoutAsync(timeoutInMilliseconds: 0).Should().BeSameAs(tcs.Task);
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnOptionalTimeoutAsync_GenericTimesOut()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<Task> action = () => tcs.WaitAndFossilizeTaskOnOptionalTimeoutAsync(TimeSpan.FromMilliseconds(5));
        await action.Should().ThrowAsync<TimeoutException>();
        tcs.Task.IsFaulted.Should().BeTrue();
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnOptionalTimeoutAsync_LongOverloadTimesOut()
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<Task> action = () => tcs.WaitAndFossilizeTaskOnOptionalTimeoutAsync(timeoutInMilliseconds: 5);
        await action.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task WaitAndFossilizeTaskOnOptionalTimeoutAsync_GenericLongOverloadTimesOut()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        Func<Task> action = () => tcs.WaitAndFossilizeTaskOnOptionalTimeoutAsync(timeoutInMilliseconds: 5);
        await action.Should().ThrowAsync<TimeoutException>();
    }

    [Fact]
    public async Task TrySetResultOrException_GenericSetsResultOrException()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        tcs.TrySetResultOrException(5).Should().BeTrue();
        (await tcs.Task).Should().Be(5);

        tcs.TrySetResultOrException(3, new InvalidOperationException()).Should().BeFalse();
    }

    [Fact]
    public void TrySetResultOrException_NonGenericSetsResultOrException()
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        tcs.TrySetResultOrException().Should().BeTrue();
        tcs.Task.IsCompletedSuccessfully.Should().BeTrue();

        tcs.TrySetResultOrException(new InvalidOperationException()).Should().BeFalse();
    }
}
