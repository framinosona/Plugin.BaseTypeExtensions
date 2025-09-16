using System.Collections;
using System.Collections.Concurrent;
using Plugin.BaseTypeExtensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void NullIfEmpty_WorksForNonGeneric()
    {
        IEnumerable? value = new[] { 1 };
        value.NullIfEmpty().Should().BeSameAs(value);
        ((IEnumerable?)Array.Empty<int>()).NullIfEmpty().Should().BeNull();
        IEnumerable? nullValue = null;
        nullValue.NullIfEmpty().Should().BeNull();
    }

    [Fact]
    public void NullIfEmpty_WorksForGeneric()
    {
        IEnumerable<int>? value = new[] { 1 };
        value.NullIfEmpty().Should().BeSameAs(value);
        ((IEnumerable<int>?)Array.Empty<int>()).NullIfEmpty().Should().BeNull();
        IEnumerable<int>? nullValue = null;
        nullValue.NullIfEmpty().Should().BeNull();
    }

    [Fact]
    public void Enqueue_LimitsQueueSize()
    {
        var queue = new ConcurrentQueue<int>(Enumerable.Range(0, 3));
        queue.Enqueue(3, max: 3);

        queue.Should().HaveCount(3);
        queue.Should().ContainInOrder(1, 2, 3);
    }

    [Fact]
    public void GetOrDefault_ReturnsDefaultsForInvalidIndex()
    {
        var numbers = new[] { 1, 2, 3 };
        numbers.GetOrDefault(-1).Should().Be(default);
        numbers.GetOrDefault(10, defaultValue: 42).Should().Be(42);
    }

    [Fact]
    public void GetOrDefault_ReturnsItemForValidIndex()
    {
        var numbers = new[] { 1, 2, 3 };
        numbers.GetOrDefault(1).Should().Be(2);
    }

    [Fact]
    public void UpdateFrom_AddsAndRemovesItems()
    {
        var output = new List<int> { 1, 3 };
        var input = new List<int> { 1, 2 };
        var added = new List<int>();
        var removed = new List<int>();

        output.UpdateFrom(
            input,
            areRepresentingTheSameItem: (i, o) => i == o,
            addAction: added.Add,
            removeAction: removed.Add);

        added.Should().ContainSingle().Which.Should().Be(2);
        removed.Should().ContainSingle().Which.Should().Be(3);
    }

    [Fact]
    public void PickRandom_ReturnsElement()
    {
        var single = new[] { 42 };
        single.PickRandom().Should().Be(42);

        Action empty = () => Array.Empty<int>().PickRandom();
        empty.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void PickRandom_WithCount_ReturnsExpectedNumber()
    {
        var numbers = Enumerable.Range(0, 5).ToArray();
        var result = numbers.PickRandom(3).ToArray();

        result.Should().HaveCount(3);
        result.Should().OnlyContain(x => numbers.Contains(x));
    }

    [Fact]
    public void Shuffle_PreservesElements()
    {
        var numbers = Enumerable.Range(0, 5).ToArray();
        var shuffled = numbers.Shuffle().ToArray();

        shuffled.Should().BeEquivalentTo(numbers);
    }
}
