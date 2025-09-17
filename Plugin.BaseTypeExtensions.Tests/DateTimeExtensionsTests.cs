namespace Plugin.BaseTypeExtensions.Tests;

public class DateTimeExtensionsTests
{
    [Fact]
    public void PercentageToDateTime_ZeroPercent_ReturnsStart()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(4);
        var percentage = 0.0;

        // Act
        var result = percentage.PercentageToDateTime(start, end);

        // Assert
        result.Should().Be(start);
    }

    [Fact]
    public void PercentageToDateTime_HundredPercent_ReturnsEnd()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(4);
        var percentage = 1.0;

        // Act
        var result = percentage.PercentageToDateTime(start, end);

        // Assert
        result.Should().Be(end);
    }

    [Fact]
    public void PercentageToDateTime_FiftyPercent_ReturnsMidpoint()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(4);
        var percentage = 0.5;

        // Act
        var result = percentage.PercentageToDateTime(start, end);

        // Assert
        result.Should().Be(start.AddHours(2));
    }

    [Fact]
    public void DateTimeToPercentage_StartDateTime_ReturnsZero()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(4);
        var value = start;

        // Act
        var result = value.DateTimeToPercentage(start, end);

        // Assert
        result.Should().Be(0.0);
    }

    [Fact]
    public void DateTimeToPercentage_EndDateTime_ReturnsOne()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(4);
        var value = end;

        // Act
        var result = value.DateTimeToPercentage(start, end);

        // Assert
        result.Should().Be(1.0);
    }

    [Fact]
    public void DateTimeToPercentage_QuarterPoint_ReturnsTwentyFivePercent()
    {
        // Arrange
        var start = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(4);
        var value = start.AddHours(1);

        // Act
        var result = value.DateTimeToPercentage(start, end);

        // Assert
        result.Should().Be(0.25);
    }

    [Fact]
    public void PercentageToDateTime_ValidateBounds_ThrowsOnInvalidRange()
    {
        // Arrange
        var start = new DateTime(2024, 1, 2);
        var end = new DateTime(2024, 1, 1); // end before start
        var percentage = 0.5;

        // Act & Assert
        Action act = () => percentage.PercentageToDateTime(start, end);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void DateTimeToPercentage_ValidateBounds_ThrowsOnInvalidRange()
    {
        // Arrange
        var start = new DateTime(2024, 1, 2);
        var end = new DateTime(2024, 1, 1); // end before start
        var value = new DateTime(2024, 1, 1, 12, 0, 0);

        // Act & Assert
        Action act = () => value.DateTimeToPercentage(start, end);
        act.Should().Throw<ArgumentException>();
    }
}
