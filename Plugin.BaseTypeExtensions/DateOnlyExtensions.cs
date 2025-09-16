namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="DateOnly"/> operations, such as range checks and percentage calculations.
/// </summary>
public static class DateOnlyExtensions
{

    #region Percentage

    /// <summary>
    /// Converts a percentage (0.0 to 1.0) to a DateOnly value within the specified range.
    /// </summary>
    /// <param name="percentage">The percentage value (0.0 to 1.0)</param>
    /// <param name="minBound">The minimum bound of the target range</param>
    /// <param name="maxBound">The maximum bound of the target range</param>
    /// <returns>The calculated DateOnly value within the specified range</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minBound is greater than maxBound</exception>
    public static DateOnly PercentageToDateOnly(this double percentage, DateOnly minBound, DateOnly maxBound)
    {
        if (minBound > maxBound)
        {
            throw new ArgumentOutOfRangeException($"{nameof(minBound)} can't be bigger than {nameof(maxBound)} ({minBound}>{maxBound})");
        }

        var zero = 0.0;
        var one = 1.0;

        return percentage switch
        {
            var p when p >= one => maxBound,
            var p when p <= zero => minBound,
            _ => minBound.AddDays(Convert.ToInt32(percentage * (maxBound.DayNumber - minBound.DayNumber))),
        };
    }

    /// <summary>
    /// Converts a DateOnly value within a specified range to a percentage (0.0 to 1.0).
    /// </summary>
    /// <param name="value">The DateOnly value to convert</param>
    /// <param name="minBound">The minimum bound of the source range</param>
    /// <param name="maxBound">The maximum bound of the source range</param>
    /// <returns>The percentage representation (0.0 to 1.0)</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minBound is greater than maxBound</exception>
    public static double DateOnlyToPercentage(this DateOnly value, DateOnly minBound, DateOnly maxBound)
    {
        if (minBound > maxBound)
        {
            throw new ArgumentOutOfRangeException($"{nameof(minBound)} can't be bigger than {nameof(maxBound)} ({minBound}>{maxBound})");
        }

        var denominator = maxBound.DayNumber - minBound.DayNumber;
        if (denominator == 0)
        {
            throw new ArgumentException($"{nameof(maxBound)} and {nameof(minBound)} must not be equal. ({maxBound} == {minBound})");
        }

        if (value <= minBound)
        {
            return 0;
        }

        if (value >= maxBound)
        {
            return 1;
        }

        return (double)(value.DayNumber - minBound.DayNumber) / denominator;
    }

    #endregion
}
