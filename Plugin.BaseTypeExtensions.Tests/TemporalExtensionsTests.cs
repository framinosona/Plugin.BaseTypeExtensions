namespace Plugin.BaseTypeExtensions.Tests;

public class TemporalExtensionsTests
{
    [Fact]
    public void DateTime_PercentageConversions_Work()
    {
        var start = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(4);

        0.0.PercentageToDateTime(start, end).Should().Be(start);
        1.0.PercentageToDateTime(start, end).Should().Be(end);
        0.5.PercentageToDateTime(start, end).Should().Be(start.AddHours(2));

        start.DateTimeToPercentage(start, end).Should().Be(0);
        end.DateTimeToPercentage(start, end).Should().Be(1);
        start.AddHours(1).DateTimeToPercentage(start, end).Should().Be(0.25);
    }

    [Fact]
    public void DateTime_PercentageConversions_ValidateBounds()
    {
        var start = new DateTime(2024, 1, 1);
        var end = start.AddDays(1);

        Action invalidRange = () => 0.5.PercentageToDateTime(end, start);
        invalidRange.Should().Throw<ArgumentOutOfRangeException>();

        Action equalBounds = () => start.DateTimeToPercentage(start, start);
        equalBounds.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void DateOnly_PercentageConversions_Work()
    {
        var start = new DateOnly(2024, 1, 1);
        var end = start.AddDays(10);

        0.0.PercentageToDateOnly(start, end).Should().Be(start);
        1.0.PercentageToDateOnly(start, end).Should().Be(end);
        0.5.PercentageToDateOnly(start, end).Should().Be(start.AddDays(5));

        start.DateOnlyToPercentage(start, end).Should().Be(0);
        end.DateOnlyToPercentage(start, end).Should().Be(1);
        start.AddDays(2).DateOnlyToPercentage(start, end).Should().Be(0.2);
    }

    [Fact]
    public void DateOnly_PercentageConversions_ValidateBounds()
    {
        var start = new DateOnly(2024, 1, 1);
        var end = start.AddDays(1);

        Action invalidRange = () => 0.5.PercentageToDateOnly(end, start);
        invalidRange.Should().Throw<ArgumentOutOfRangeException>();

        Action equalBounds = () => start.DateOnlyToPercentage(start, start);
        equalBounds.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void TimeOnly_PercentageConversions_Work()
    {
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(12, 0);

        0.0.PercentageToTimeOnly(start, end).Should().Be(start);
        1.0.PercentageToTimeOnly(start, end).Should().Be(end);

        var mid = 0.5.PercentageToTimeOnly(start, end);
        mid.Should().Be(start.AddHours(1));

        start.TimeOnlyToPercentage(start, end).Should().Be(0);
        end.TimeOnlyToPercentage(start, end).Should().Be(1);
        start.AddMinutes(30).TimeOnlyToPercentage(start, end).Should().Be(0.25);
    }

    [Fact]
    public void TimeOnly_PercentageConversions_ValidateBounds()
    {
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(12, 0);

        Action invalidRange = () => 0.5.PercentageToTimeOnly(end, start);
        invalidRange.Should().Throw<ArgumentOutOfRangeException>();

        Action equalBounds = () => start.TimeOnlyToPercentage(start, start);
        equalBounds.Should().Throw<ArgumentException>();
    }
}
