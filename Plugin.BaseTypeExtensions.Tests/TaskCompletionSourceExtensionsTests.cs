namespace Plugin.BaseTypeExtensions.Tests;

public class TaskCompletionSourceExtensionsTests
{
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
