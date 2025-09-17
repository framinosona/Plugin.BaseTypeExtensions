namespace Plugin.BaseTypeExtensions.Tests;

public class DateOnlyExtensionsTests
{
    [Fact]
    public void PercentageToDateOnly_ZeroPercent_ReturnsStart()
    {
        // Arrange
        var start = new DateOnly(2024, 1, 1);
        var end = new DateOnly(2024, 12, 31);
        var percentage = 0.0;

        // Act
        var result = percentage.PercentageToDateOnly(start, end);

        // Assert
        result.Should().Be(start);
    }

    [Fact]
    public void PercentageToDateOnly_HundredPercent_ReturnsEnd()
    {
        // Arrange
        var start = new DateOnly(2024, 1, 1);
        var end = new DateOnly(2024, 12, 31);
        var percentage = 1.0;

        // Act
        var result = percentage.PercentageToDateOnly(start, end);

        // Assert
        result.Should().Be(end);
    }

    [Fact]
    public void PercentageToDateOnly_FiftyPercent_ReturnsMidpoint()
    {
        // Arrange
        var start = new DateOnly(2024, 1, 1);
        var end = new DateOnly(2024, 1, 11); // 10 days apart
        var percentage = 0.5;

        // Act
        var result = percentage.PercentageToDateOnly(start, end);

        // Assert
        result.Should().Be(new DateOnly(2024, 1, 6)); // 5 days from start
    }

    [Fact]
    public void DateOnlyToPercentage_StartDate_ReturnsZero()
    {
        // Arrange
        var start = new DateOnly(2024, 1, 1);
        var end = new DateOnly(2024, 12, 31);
        var value = start;

        // Act
        var result = value.DateOnlyToPercentage(start, end);

        // Assert
        result.Should().Be(0.0);
    }

    [Fact]
    public void DateOnlyToPercentage_EndDate_ReturnsOne()
    {
        // Arrange
        var start = new DateOnly(2024, 1, 1);
        var end = new DateOnly(2024, 12, 31);
        var value = end;

        // Act
        var result = value.DateOnlyToPercentage(start, end);

        // Assert
        result.Should().Be(1.0);
    }

    [Fact]
    public void DateOnlyToPercentage_MidpointDate_ReturnsFiftyPercent()
    {
        // Arrange
        var start = new DateOnly(2024, 1, 1);
        var end = new DateOnly(2024, 1, 11); // 10 days apart
        var value = new DateOnly(2024, 1, 6); // 5 days from start

        // Act
        var result = value.DateOnlyToPercentage(start, end);

        // Assert
        result.Should().Be(0.5);
    }
}
