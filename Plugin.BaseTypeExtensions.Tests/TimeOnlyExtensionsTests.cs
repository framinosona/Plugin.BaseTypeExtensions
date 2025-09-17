namespace Plugin.BaseTypeExtensions.Tests;

public class TimeOnlyExtensionsTests
{
    [Fact]
    public void PercentageToTimeOnly_ZeroPercent_ReturnsStart()
    {
        // Arrange
        var start = new TimeOnly(9, 0, 0); // 9:00 AM
        var end = new TimeOnly(17, 0, 0); // 5:00 PM
        var percentage = 0.0;

        // Act
        var result = percentage.PercentageToTimeOnly(start, end);

        // Assert
        result.Should().Be(start);
    }

    [Fact]
    public void PercentageToTimeOnly_HundredPercent_ReturnsEnd()
    {
        // Arrange
        var start = new TimeOnly(9, 0, 0); // 9:00 AM
        var end = new TimeOnly(17, 0, 0); // 5:00 PM
        var percentage = 1.0;

        // Act
        var result = percentage.PercentageToTimeOnly(start, end);

        // Assert
        result.Should().Be(end);
    }

    [Fact]
    public void PercentageToTimeOnly_FiftyPercent_ReturnsMidpoint()
    {
        // Arrange
        var start = new TimeOnly(9, 0, 0); // 9:00 AM
        var end = new TimeOnly(17, 0, 0); // 5:00 PM (8 hours later)
        var percentage = 0.5;

        // Act
        var result = percentage.PercentageToTimeOnly(start, end);

        // Assert
        result.Should().Be(new TimeOnly(13, 0, 0)); // 1:00 PM (4 hours from start)
    }

    [Fact]
    public void TimeOnlyToPercentage_StartTime_ReturnsZero()
    {
        // Arrange
        var start = new TimeOnly(9, 0, 0);
        var end = new TimeOnly(17, 0, 0);
        var value = start;

        // Act
        var result = value.TimeOnlyToPercentage(start, end);

        // Assert
        result.Should().Be(0.0);
    }

    [Fact]
    public void TimeOnlyToPercentage_EndTime_ReturnsOne()
    {
        // Arrange
        var start = new TimeOnly(9, 0, 0);
        var end = new TimeOnly(17, 0, 0);
        var value = end;

        // Act
        var result = value.TimeOnlyToPercentage(start, end);

        // Assert
        result.Should().Be(1.0);
    }

    [Fact]
    public void TimeOnlyToPercentage_MidpointTime_ReturnsFiftyPercent()
    {
        // Arrange
        var start = new TimeOnly(9, 0, 0); // 9:00 AM
        var end = new TimeOnly(17, 0, 0); // 5:00 PM
        var value = new TimeOnly(13, 0, 0); // 1:00 PM (midpoint)

        // Act
        var result = value.TimeOnlyToPercentage(start, end);

        // Assert
        result.Should().Be(0.5);
    }
}
