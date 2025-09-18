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

    // Additional comprehensive tests for better coverage
    [Fact]
    public void IsValidEmail_InvalidEmails_ReturnsFalse()
    {
        // Test various invalid email formats
        "plainaddress".IsValidEmail().Should().BeFalse();
        "@missingdomain.com".IsValidEmail().Should().BeFalse();
        "missing@.com".IsValidEmail().Should().BeFalse();
        "missing domain@example.com".IsValidEmail().Should().BeFalse();
        "".IsValidEmail().Should().BeFalse();
        " ".IsValidEmail().Should().BeFalse();
    }

    [Fact]
    public void IsValidEmail_ValidEmails_ReturnsTrue()
    {
        "test@example.com".IsValidEmail().Should().BeTrue();
        "user.name@domain.co.uk".IsValidEmail().Should().BeTrue();
        "test+tag@example.org".IsValidEmail().Should().BeTrue();
    }

    [Fact]
    public void IsValidUrl_InvalidUrls_ReturnsFalse()
    {
        "not-a-url".IsValidUrl().Should().BeFalse();
        "http://".IsValidUrl().Should().BeFalse();
        "".IsValidUrl().Should().BeFalse();
        " ".IsValidUrl().Should().BeFalse();
        ((string?)null).IsValidUrl().Should().BeFalse();
    }

    [Fact]
    public void IsValidUrl_ValidUrls_ReturnsTrue()
    {
        "https://www.example.com".IsValidUrl().Should().BeTrue();
        "http://test.org/path?param=value".IsValidUrl().Should().BeTrue();
        "ftp://files.example.com/file.txt".IsValidUrl().Should().BeTrue();
    }

    [Fact]
    public void IsNumeric_EdgeCases_ReturnsCorrectly()
    {
        "3.14159".IsNumeric().Should().BeTrue();
        "-123.45".IsNumeric().Should().BeTrue();
        "1e10".IsNumeric().Should().BeTrue();
        "1E-5".IsNumeric().Should().BeTrue(); // Scientific notation
        "0.0".IsNumeric().Should().BeTrue();
        "abc123".IsNumeric().Should().BeFalse();
        "123abc".IsNumeric().Should().BeFalse();
        "not-a-number".IsNumeric().Should().BeFalse();
        "".IsNumeric().Should().BeFalse();
        " ".IsNumeric().Should().BeFalse();
    }

    [Fact]
    public void IsInteger_EdgeCases_ReturnsCorrectly()
    {
        "123".IsInteger().Should().BeTrue();
        "-456".IsInteger().Should().BeTrue();
        "0".IsInteger().Should().BeTrue();
        "123.45".IsInteger().Should().BeFalse();
        "1e10".IsInteger().Should().BeFalse();
        "abc".IsInteger().Should().BeFalse();
        "".IsInteger().Should().BeFalse();
        " ".IsInteger().Should().BeFalse();
    }

    [Fact]
    public void ToTitleCase_EdgeCases_HandlesCorrectly()
    {
        "".ToTitleCase().Should().BeEmpty();
        ((string?)null).ToTitleCase().Should().BeEmpty();
        "UPPERCASE".ToTitleCase().Should().Be("Uppercase");
        "lowercase".ToTitleCase().Should().Be("Lowercase");
        "mixed CaSe".ToTitleCase().Should().Be("Mixed Case");
    }

    [Fact]
    public void ToCamelCase_EdgeCases_HandlesCorrectly()
    {
        "".ToCamelCase().Should().BeEmpty();
        ((string?)null).ToCamelCase().Should().BeEmpty();
        "single".ToCamelCase().Should().Be("single");
        "HELLO WORLD".ToCamelCase().Should().Be("helloWorld");
        "hello_world-test".ToCamelCase().Should().Be("helloWorldTest");
        "   spaced   words   ".ToCamelCase().Should().Be("spacedWords");
        "___".ToCamelCase().Should().BeEmpty(); // Only separators
    }

    [Fact]
    public void ToPascalCase_EdgeCases_HandlesCorrectly()
    {
        "".ToPascalCase().Should().BeEmpty();
        ((string?)null).ToPascalCase().Should().BeEmpty();
        "single".ToPascalCase().Should().Be("Single");
        "hello world".ToPascalCase().Should().Be("HelloWorld");
        "hello_world-test".ToPascalCase().Should().Be("HelloWorldTest");
        "___".ToPascalCase().Should().BeEmpty(); // Only separators
    }

    [Fact]
    public void ToKebabCase_EdgeCases_HandlesCorrectly()
    {
        "".ToKebabCase().Should().BeEmpty();
        ((string?)null).ToKebabCase().Should().BeEmpty();
        "single".ToKebabCase().Should().Be("single");
        "Hello World".ToKebabCase().Should().Be("hello-world");
        "hello_world test".ToKebabCase().Should().Be("hello-world-test");
    }

    [Fact]
    public void ToSnakeCase_EdgeCases_HandlesCorrectly()
    {
        "".ToSnakeCase().Should().BeEmpty();
        ((string?)null).ToSnakeCase().Should().BeEmpty();
        "single".ToSnakeCase().Should().Be("single");
        "Hello World".ToSnakeCase().Should().Be("hello_world");
        "hello-world test".ToSnakeCase().Should().Be("hello_world_test");
    }

    [Fact]
    public void Truncate_EdgeCases_HandlesCorrectly()
    {
        "".Truncate(5).Should().BeEmpty();
        ((string?)null).Truncate(5).Should().BeEmpty();
        "short".Truncate(10).Should().Be("short");
        "exactly".Truncate(7).Should().Be("exactly");
        "toolong".Truncate(0).Should().BeEmpty();
        "test".Truncate(-5).Should().BeEmpty();
        "test".Truncate(2, null!).Should().Be("te"); // Null suffix
        "test".Truncate(2, "...").Should().Be(".."); // Suffix longer than max
    }

    [Fact]
    public void PadBoth_EdgeCases_HandlesCorrectly()
    {
        "".PadBoth(5).Should().Be("     "); // Empty string gets padded to full width
        ((string?)null).PadBoth(5).Should().Be("     "); // Null also gets padded to full width
        "test".PadBoth(4).Should().Be("test"); // Already correct length
        "test".PadBoth(3).Should().Be("test"); // Shorter than input
        "a".PadBoth(5, '*').Should().Be("**a**");
        "ab".PadBoth(5).Should().Be(" ab  "); // Even distribution with extra on right
    }

    [Fact]
    public void FromBase64_InvalidInput_ReturnsEmpty()
    {
        "invalid-base64!".FromBase64().Should().BeEmpty();
        "SGVsbG8gV29ybGQ!invalid".FromBase64().Should().BeEmpty();
        "".FromBase64().Should().BeEmpty();
        ((string?)null).FromBase64().Should().BeEmpty();
    }

    [Fact]
    public void FromBase64_ValidInput_DecodesCorrectly()
    {
        "SGVsbG8gV29ybGQ=".FromBase64().Should().Be("Hello World");
    }

    [Fact]
    public void ToBase64_WithDifferentEncodings_WorksCorrectly()
    {
        var text = "Hello 世界";
        var utf8Result = text.ToBase64(System.Text.Encoding.UTF8);
        var unicodeResult = text.ToBase64(System.Text.Encoding.Unicode);

        utf8Result.Should().NotBeEmpty();
        unicodeResult.Should().NotBeEmpty();
        utf8Result.Should().NotBe(unicodeResult); // Different encodings produce different results

        // Verify round-trip
        utf8Result.FromBase64(System.Text.Encoding.UTF8).Should().Be(text);
        unicodeResult.FromBase64(System.Text.Encoding.Unicode).Should().Be(text);
    }

    [Fact]
    public void RemoveWhitespace_VariousWhitespace_RemovesAll()
    {
        "hello\t\r\n world  ".RemoveWhitespace().Should().Be("helloworld");
        "   ".RemoveWhitespace().Should().BeEmpty();
        "".RemoveWhitespace().Should().BeEmpty();
        ((string?)null).RemoveWhitespace().Should().BeEmpty();
    }

    [Fact]
    public void CollapseWhitespace_MultipleSpaces_CollapsesToSingle()
    {
        "hello    world".CollapseWhitespace().Should().Be("hello world");
        "  start   middle   end  ".CollapseWhitespace().Should().Be("start middle end"); // Leading/trailing spaces removed
        "hello\t\t\tworld".CollapseWhitespace().Should().Be("hello world");
        "   ".CollapseWhitespace().Should().Be(""); // All whitespace collapses to empty
    }

    [Fact]
    public void Between_EdgeCases_HandlesCorrectly()
    {
        "hello[world]test".Between("[", "]").Should().Be("world");
        "test".Between("[", "]").Should().BeEmpty(); // Markers not found
        "[test".Between("[", "]").Should().BeEmpty(); // End marker not found
        "test]".Between("[", "]").Should().BeEmpty(); // Start marker not found
        "".Between("[", "]").Should().BeEmpty();
        ((string?)null).Between("[", "]").Should().BeEmpty();
        "test".Between("", "]").Should().BeEmpty(); // Empty start marker
        "test".Between("[", "").Should().BeEmpty(); // Empty end marker
    }

    [Fact]
    public void After_EdgeCases_HandlesCorrectly()
    {
        "hello:world".After(":").Should().Be("world");
        "test".After(":").Should().BeEmpty(); // Marker not found
        "".After(":").Should().BeEmpty();
        ((string?)null).After(":").Should().BeEmpty();
        "test".After("").Should().BeEmpty(); // Empty marker
        "test:".After(":").Should().BeEmpty(); // Nothing after marker
    }

    [Fact]
    public void Before_EdgeCases_HandlesCorrectly()
    {
        "hello:world".Before(":").Should().Be("hello");
        "test".Before(":").Should().BeEmpty(); // Marker not found
        "".Before(":").Should().BeEmpty();
        ((string?)null).Before(":").Should().BeEmpty();
        "test".Before("").Should().BeEmpty(); // Empty marker
        ":test".Before(":").Should().BeEmpty(); // Nothing before marker
    }

    [Fact]
    public void SplitAndTrim_HandlesVariousInput()
    {
        "hello, world , test".SplitAndTrim(',').Should().BeEquivalentTo(["hello", "world", "test"]);
        "".SplitAndTrim(',').Should().BeEmpty();
        ((string?)null).SplitAndTrim(',').Should().BeEmpty();
        "a,,b".SplitAndTrim(',').Should().BeEquivalentTo(["a", "", "b"]);
    }

    [Fact]
    public void SplitAndRemoveEmpty_HandlesVariousInput()
    {
        "hello,,world".SplitAndRemoveEmpty(',').Should().BeEquivalentTo(["hello", "world"]);
        "".SplitAndRemoveEmpty(',').Should().BeEmpty();
        ((string?)null).SplitAndRemoveEmpty(',').Should().BeEmpty();
        ",,,".SplitAndRemoveEmpty(',').Should().BeEmpty();
    }

    [Fact]
    public void JoinWith_HandlesVariousInput()
    {
        new[] { "hello", "world" }.JoinWith(", ").Should().Be("hello, world");
        new string[0].JoinWith(", ").Should().BeEmpty();
        ((IEnumerable<string>?)null!).JoinWith(", ").Should().BeEmpty();
        new[] { "single" }.JoinWith(", ").Should().Be("single");
    }

    [Fact]
    public void Repeat_EdgeCases_HandlesCorrectly()
    {
        "".Repeat(5).Should().BeEmpty();
        ((string?)null).Repeat(5).Should().BeEmpty();
        "test".Repeat(0).Should().BeEmpty();
        "test".Repeat(-1).Should().BeEmpty();
        "test".Repeat(1).Should().Be("test");
        "a".Repeat(3).Should().Be("aaa");
    }

    [Fact]
    public void RandomString_EdgeCases_HandlesCorrectly()
    {
        StringExtensions.RandomString(0).Should().BeEmpty();
        StringExtensions.RandomString(-1).Should().BeEmpty();
        StringExtensions.RandomString(5, "").Should().BeEmpty(); // Empty allowed chars
        StringExtensions.RandomString(5, null!).Should().BeEmpty(); // Null allowed chars

        var result = StringExtensions.RandomString(10, "abc");
        result.Should().HaveLength(10);
        result.Should().MatchRegex("^[abc]+$"); // Should only contain allowed chars
    }

    [Fact]
    public void WordCount_VariousInputs_CountsCorrectly()
    {
        "hello world test".WordCount().Should().Be(3);
        "  hello   world  ".WordCount().Should().Be(2);
        "single".WordCount().Should().Be(1);
        "".WordCount().Should().Be(0);
        "   ".WordCount().Should().Be(0);
        ((string?)null).WordCount().Should().Be(0);
        "word1,word2.word3".WordCount().Should().Be(1); // Non-whitespace separators don't split
    }

    [Fact]
    public void LineCount_VariousInputs_CountsCorrectly()
    {
        "line1\nline2\nline3".LineCount().Should().Be(3); // 2 newlines = 3 lines
        "single line".LineCount().Should().Be(1);
        "".LineCount().Should().Be(1); // Empty string still has 1 line
        ((string?)null).LineCount().Should().Be(0);
        "line1\r\nline2".LineCount().Should().Be(2); // Windows line endings
        "\n\n\n".LineCount().Should().Be(4); // Only newlines
    }
}
