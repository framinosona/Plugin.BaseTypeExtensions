namespace Plugin.BaseTypeExtensions.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void NullIfDud_ReturnsNullForWhitespace()
    {
        StringExtensions.NullIfDud("   ").Should().BeNull();
    }

    [Fact]
    public void NullIfDud_ReturnsOriginalForValue()
    {
        var value = "hello";
        StringExtensions.NullIfDud(value).Should().BeSameAs(value);
    }

    [Fact]
    public void RemoveSpecialCharacters_SanitizesString()
    {
        "he!!o-w@rld".RemoveSpecialCharacters().Should().Be("heo-wrld");
    }

    [Fact]
    public void RemoveSpecialCharacters_HandlesNull()
    {
        string? input = null;
        input.RemoveSpecialCharacters().Should().BeEmpty();
    }
}
