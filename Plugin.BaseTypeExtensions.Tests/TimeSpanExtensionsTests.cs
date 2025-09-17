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
}
