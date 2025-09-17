namespace Plugin.BaseTypeExtensions.Tests;

public class ComparableToolsTests
{
    [Fact]
    public void Min_FirstValueSmallerThanSecond_ReturnsFirstValue()
    {
        // Arrange
        var value1 = 5;
        var value2 = 10;

        // Act
        var result = ComparableTools.Min(value1, value2);

        // Assert
        result.Should().Be(value1);
    }

    [Fact]
    public void Min_SecondValueSmallerThanFirst_ReturnsSecondValue()
    {
        // Arrange
        var value1 = 15;
        var value2 = 10;

        // Act
        var result = ComparableTools.Min(value1, value2);

        // Assert
        result.Should().Be(value2);
    }

    [Fact]
    public void Min_EqualValues_ReturnsEitherValue()
    {
        // Arrange
        var value1 = 10;
        var value2 = 10;

        // Act
        var result = ComparableTools.Min(value1, value2);

        // Assert
        result.Should().Be(value1); // Should return second value when equal
    }

    [Fact]
    public void Max_FirstValueGreaterThanSecond_ReturnsFirstValue()
    {
        // Arrange
        var value1 = 15;
        var value2 = 10;

        // Act
        var result = ComparableTools.Max(value1, value2);

        // Assert
        result.Should().Be(value1);
    }

    [Fact]
    public void Max_SecondValueGreaterThanFirst_ReturnsSecondValue()
    {
        // Arrange
        var value1 = 5;
        var value2 = 10;

        // Act
        var result = ComparableTools.Max(value1, value2);

        // Assert
        result.Should().Be(value2);
    }

    [Fact]
    public void Max_EqualValues_ReturnsEitherValue()
    {
        // Arrange
        var value1 = 10;
        var value2 = 10;

        // Act
        var result = ComparableTools.Max(value1, value2);

        // Assert
        result.Should().Be(value1); // Should return second value when equal
    }

    [Fact]
    public void Min_WithStrings_ReturnsLexicographicallySmaller()
    {
        // Arrange
        var value1 = "apple";
        var value2 = "banana";

        // Act
        var result = ComparableTools.Min(value1, value2);

        // Assert
        result.Should().Be(value1);
    }

    [Fact]
    public void Max_WithStrings_ReturnsLexicographicallyLarger()
    {
        // Arrange
        var value1 = "apple";
        var value2 = "banana";

        // Act
        var result = ComparableTools.Max(value1, value2);

        // Assert
        result.Should().Be(value2);
    }

    [Fact]
    public void Min_WithDateTime_ReturnsEarlierDate()
    {
        // Arrange
        var date1 = new DateTime(2024, 1, 1);
        var date2 = new DateTime(2024, 1, 2);

        // Act
        var result = ComparableTools.Min(date1, date2);

        // Assert
        result.Should().Be(date1);
    }

    [Fact]
    public void Max_WithDateTime_ReturnsLaterDate()
    {
        // Arrange
        var date1 = new DateTime(2024, 1, 1);
        var date2 = new DateTime(2024, 1, 2);

        // Act
        var result = ComparableTools.Max(date1, date2);

        // Assert
        result.Should().Be(date2);
    }
}
