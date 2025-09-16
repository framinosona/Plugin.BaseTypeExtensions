using Plugin.BaseTypeExtensions;

public class ListExtensionsTests
{
    [Fact]
    public void UpdateFrom_WithCustomConversion_SyncsCollections()
    {
        var output = new List<string> { "1", "3" };
        var input = new List<int> { 1, 2 };

        output.UpdateFrom(input, (i, o) => i.ToString() == o, i => i.ToString());

        output.Should().Equal("1", "2");
    }

    [Fact]
    public void UpdateFrom_WithEquatableConversion_SyncsCollections()
    {
        var output = new List<int> { 1, 3 };
        var input = new List<int> { 1, 2 };

        output.UpdateFrom(input, i => i);

        output.Should().Equal(1, 2);
    }

    [Fact]
    public void UpdateFrom_WithComparer_SyncsCollections()
    {
        var output = new List<int> { 1, 3 };
        var input = new List<int> { 1, 2 };

        output.UpdateFrom(input, (i, o) => i == o);

        output.Should().Equal(1, 2);
    }

    [Fact]
    public void UpdateFrom_DefaultOverload_SyncsCollections()
    {
        var output = new List<int> { 1, 3 };
        var input = new List<int> { 1, 2 };

        output.UpdateFrom(input);

        output.Should().Equal(1, 2);
    }

    [Fact]
    public void Pairs_ReturnsConsecutivePairs()
    {
        var items = new List<int> { 1, 2, 3 };
        var pairs = items.Pairs().ToArray();

        pairs.Should().Equal((1, 2), (2, 3));
    }

    [Fact]
    public void Pairs_ReturnsEmptyWhenInsufficientItems()
    {
        var items = new List<int> { 1 };
        items.Pairs().Should().BeEmpty();
    }
}
