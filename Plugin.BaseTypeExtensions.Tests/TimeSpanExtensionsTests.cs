namespace Plugin.BaseTypeExtensions.Tests;

public class TimeSpanExtensionsTests
{
    [Fact]
    public void ToReadableString_FormatsCorrectly()
    {
        // Arrange
        var timeSpan = new TimeSpan(1, 2, 3, 4); // 1 day, 2 hours, 3 minutes, 4 seconds

        // Act
        var result = timeSpan.ToReadableString();

        // Assert
        result.Should().Contain("1 day");
        result.Should().Contain("2 hours");
        result.Should().Contain("3 minutes");
        result.Should().Contain("4 seconds");
    }

    [Fact]
    public void ToShortString_FormatsCorrectly()
    {
        // Arrange
        var timeSpan = new TimeSpan(1, 2, 3, 4); // 1 day, 2 hours, 3 minutes, 4 seconds

        // Act
        var result = timeSpan.ToShortString();

        // Assert
        result.Should().Be("1d 2h 3m 4s");
    }

    [Fact]
    public void ToHumanReadable_FormatsCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(25); // 1 day and 1 hour

        // Act
        var result = timeSpan.ToHumanReadable();

        // Assert
        result.Should().Contain("day");
    }

    [Fact]
    public void ToWeeks_CalculatesCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(14); // 2 weeks

        // Act
        var result = timeSpan.ToWeeks();

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public void ToWeeksExact_CalculatesCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(10.5); // 1.5 weeks

        // Act
        var result = timeSpan.ToWeeksExact();

        // Assert
        result.Should().Be(1.5);
    }

    [Fact]
    public void ToBusinessDays_CalculatesCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(7); // 1 week = 5 business days

        // Act
        var result = timeSpan.ToBusinessDays();

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void IsPositive_ReturnsTrueForPositiveTimeSpan()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(1);

        // Act
        var result = timeSpan.IsPositive();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNegative_ReturnsTrueForNegativeTimeSpan()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(-1);

        // Act
        var result = timeSpan.IsNegative();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsZero_ReturnsTrueForZeroTimeSpan()
    {
        // Arrange
        var timeSpan = TimeSpan.Zero;

        // Act
        var result = timeSpan.IsZero();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLessThan_ComparesCorrectly()
    {
        // Arrange
        var timeSpan1 = TimeSpan.FromHours(1);
        var timeSpan2 = TimeSpan.FromHours(2);

        // Act
        var result = timeSpan1.IsLessThan(timeSpan2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsGreaterThan_ComparesCorrectly()
    {
        // Arrange
        var timeSpan1 = TimeSpan.FromHours(2);
        var timeSpan2 = TimeSpan.FromHours(1);

        // Act
        var result = timeSpan1.IsGreaterThan(timeSpan2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Multiply_MultipliesCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(2);
        var factor = 1.5;

        // Act
        var result = timeSpan.Multiply(factor);

        // Assert
        result.Should().Be(TimeSpan.FromHours(3));
    }

    [Fact]
    public void Divide_DividesCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(6);
        var divisor = 2.0;

        // Act
        var result = timeSpan.Divide(divisor);

        // Assert
        result.Should().Be(TimeSpan.FromHours(3));
    }

    [Fact]
    public void Average_CalculatesCorrectly()
    {
        // Arrange
        var timeSpans = new[]
        {
            TimeSpan.FromHours(1),
            TimeSpan.FromHours(2),
            TimeSpan.FromHours(3)
        };

        // Act
        var result = timeSpans.Average();

        // Assert
        result.Should().Be(TimeSpan.FromHours(2));
    }

    [Fact]
    public void RoundToMinutes_RoundsCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromSeconds(90); // 1 minute 30 seconds

        // Act
        var result = timeSpan.RoundToMinutes();

        // Assert
        result.Should().Be(TimeSpan.FromMinutes(2)); // Should round up
    }

    [Fact]
    public void RoundToHours_RoundsCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(90); // 1 hour 30 minutes

        // Act
        var result = timeSpan.RoundToHours();

        // Assert
        result.Should().Be(TimeSpan.FromHours(2)); // Should round up
    }

    [Fact]
    public void RoundToDays_RoundsCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(36); // 1 day 12 hours

        // Act
        var result = timeSpan.RoundToDays();

        // Assert
        result.Should().Be(TimeSpan.FromDays(2)); // Should round up
    }

    [Fact]
    public void RoundTo_RoundsToInterval()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(7); // 7 minutes
        var interval = TimeSpan.FromMinutes(5); // Round to 5-minute intervals

        // Act
        var result = timeSpan.RoundTo(interval);

        // Assert
        result.Should().Be(TimeSpan.FromMinutes(5)); // Should round down to 5
    }

    // Additional edge case tests for better coverage
    [Fact]
    public void ToReadableString_ZeroTimeSpan_ReturnsZeroSeconds()
    {
        // Arrange
        var timeSpan = TimeSpan.Zero;

        // Act
        var result = timeSpan.ToReadableString();

        // Assert
        result.Should().Be("0 seconds");
    }

    [Fact]
    public void ToReadableString_OnlyMilliseconds_ReturnsMilliseconds()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMilliseconds(500);

        // Act
        var result = timeSpan.ToReadableString();

        // Assert
        result.Should().Be("500 milliseconds");
    }

    [Fact]
    public void ToReadableString_OnlyMillisecond_ReturnsSingular()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMilliseconds(1);

        // Act
        var result = timeSpan.ToReadableString();

        // Assert
        result.Should().Be("1 millisecond");
    }

    [Fact]
    public void ToReadableString_SingularValues_UsesSingularForm()
    {
        // Arrange
        var timeSpan = new TimeSpan(1, 1, 1, 1); // 1 day, 1 hour, 1 minute, 1 second

        // Act
        var result = timeSpan.ToReadableString();

        // Assert
        result.Should().Contain("1 day");
        result.Should().Contain("1 hour");
        result.Should().Contain("1 minute");
        result.Should().Contain("1 second");
        result.Should().NotContain("days");
        result.Should().NotContain("hours");
        result.Should().NotContain("minutes");
        result.Should().NotContain("seconds");
    }

    [Fact]
    public void ToShortString_ZeroTimeSpan_ReturnsZeroSeconds()
    {
        // Arrange
        var timeSpan = TimeSpan.Zero;

        // Act
        var result = timeSpan.ToShortString();

        // Assert
        result.Should().Be("0s");
    }

    [Fact]
    public void ToShortString_OnlyMilliseconds_ReturnsMilliseconds()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMilliseconds(250);

        // Act
        var result = timeSpan.ToShortString();

        // Assert
        result.Should().Be("250ms");
    }

    [Fact]
    public void ToHumanReadable_LessThanMinute_ReturnsLessThanMinute()
    {
        // Arrange
        var timeSpan = TimeSpan.FromSeconds(30);

        // Act
        var result = timeSpan.ToHumanReadable();

        // Assert
        result.Should().Be("less than a minute");
    }

    [Fact]
    public void ToHumanReadable_OneMinute_ReturnsSingular()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(1);

        // Act
        var result = timeSpan.ToHumanReadable();

        // Assert
        result.Should().Be("about 1 minute");
    }

    [Fact]
    public void ToHumanReadable_OneHour_ReturnsSingular()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(1);

        // Act
        var result = timeSpan.ToHumanReadable();

        // Assert
        result.Should().Be("about 1 hour");
    }

    [Fact]
    public void ToHumanReadable_OneDay_ReturnsSingular()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(1);

        // Act
        var result = timeSpan.ToHumanReadable();

        // Assert
        result.Should().Be("about 1 day");
    }

    [Fact]
    public void ToHumanReadable_OneMonth_ReturnsSingular()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(30);

        // Act
        var result = timeSpan.ToHumanReadable();

        // Assert
        result.Should().Be("about 1 month");
    }

    [Fact]
    public void ToHumanReadable_OneYear_ReturnsSingular()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(365);

        // Act
        var result = timeSpan.ToHumanReadable();

        // Assert
        result.Should().Be("about 1 year");
    }

    [Fact]
    public void ToHumanReadable_NegativeTimeSpan_UsesAbsoluteValue()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(-2);

        // Act
        var result = timeSpan.ToHumanReadable();

        // Assert
        result.Should().Be("about 2 hours");
    }

    [Fact]
    public void Divide_ByNegativeTwo_ReturnsCorrectResult()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(4);

        // Act
        var result = timeSpan.Divide(-2.0);

        // Assert
        result.Should().Be(TimeSpan.FromHours(-2));
    }

    [Fact]
    public void Average_NullCollection_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<TimeSpan>? nullCollection = null;

        // Act & Assert
        var act = () => nullCollection!.Average();
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Average_EmptyCollection_ThrowsInvalidOperationException()
    {
        // Arrange
        var emptyCollection = new TimeSpan[0];

        // Act & Assert
        var act = () => emptyCollection.Average();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot calculate average of empty collection.");
    }

    [Fact]
    public void Average_SingleItem_ReturnsSameItem()
    {
        // Arrange
        var timeSpans = new[] { TimeSpan.FromHours(3) };

        // Act
        var result = timeSpans.Average();

        // Assert
        result.Should().Be(TimeSpan.FromHours(3));
    }

    [Fact]
    public void RoundTo_ZeroInterval_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(7);
        var zeroInterval = TimeSpan.Zero;

        // Act & Assert
        var act = () => timeSpan.RoundTo(zeroInterval);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Interval must be positive*")
            .And.ParamName.Should().Be("interval");
    }

    [Fact]
    public void RoundTo_NegativeInterval_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(7);
        var negativeInterval = TimeSpan.FromMinutes(-5);

        // Act & Assert
        var act = () => timeSpan.RoundTo(negativeInterval);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Interval must be positive*")
            .And.ParamName.Should().Be("interval");
    }

    [Fact]
    public void RoundTo_ExactMultiple_ReturnsUnchanged()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(10); // Exact multiple of 5
        var interval = TimeSpan.FromMinutes(5);

        // Act
        var result = timeSpan.RoundTo(interval);

        // Assert
        result.Should().Be(timeSpan);
    }

    [Fact]
    public void RoundTo_RoundUp_WorksCorrectly()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(8); // Should round up to 10
        var interval = TimeSpan.FromMinutes(5);

        // Act
        var result = timeSpan.RoundTo(interval);

        // Assert
        result.Should().Be(TimeSpan.FromMinutes(10));
    }

    [Fact]
    public void ToWeeks_PartialWeek_ReturnsFlooredValue()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(10); // 1 week + 3 days = 1 complete week

        // Act
        var result = timeSpan.ToWeeks();

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void ToWeeksExact_PartialWeek_ReturnsDecimalValue()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(10.5); // 1.5 weeks

        // Act
        var result = timeSpan.ToWeeksExact();

        // Assert
        result.Should().BeApproximately(1.5, 0.001);
    }

    [Fact]
    public void ToBusinessDays_ExactWeeks_ReturnsCorrectCount()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(14); // 2 weeks = 10 business days

        // Act
        var result = timeSpan.ToBusinessDays();

        // Assert
        result.Should().Be(10);
    }

    [Fact]
    public void ToBusinessDays_MoreThanFiveRemainingDays_CapsAtFive()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(12); // 1 week + 5 days = 5 + 5 = 10 business days

        // Act
        var result = timeSpan.ToBusinessDays();

        // Assert
        result.Should().Be(10); // 1 week * 5 + min(5, 5)
    }
}
