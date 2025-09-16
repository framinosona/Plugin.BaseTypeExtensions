using Plugin.BaseTypeExtensions;

public class GuidAndVersionExtensionsTests
{
    [Fact]
    public void GetPlusOne_IncrementsGuid()
    {
        var guid = Guid.Parse("00000000-0000-0000-0000-0000000000FF");
        var expected = Guid.Parse("00000000-0000-0000-0000-000000000100");

        guid.GetPlusOne().Should().Be(expected);
    }

    [Fact]
    public void GetMinusOne_DecrementsGuid()
    {
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000100");
        var expected = Guid.Parse("00000000-0000-0000-0000-0000000000FF");

        guid.GetMinusOne().Should().Be(expected);
    }

    [Fact]
    public void ToFullVersion_ReturnsNullForNull()
    {
        Version? value = null;
        value.ToFullVersion().Should().BeNull();
    }

    [Fact]
    public void ToFullVersion_ReturnsSameInstanceWhenComplete()
    {
        var version = new Version(1, 2, 3, 4);
        version.ToFullVersion().Should().BeSameAs(version);
    }

    [Fact]
    public void ToFullVersion_CompletesMissingFields()
    {
        var result = new Version(2, 5).ToFullVersion();

        result.Should().Be(new Version(2, 5, 0, 0));
    }
}
