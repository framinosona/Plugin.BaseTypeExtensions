namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="DateTime"/> operations, such as range checks and percentage calculations.
/// </summary>
public static class DateTimeExtensions
{

    #region Percentage

    /// <summary>
    /// Converts a percentage (0.0 to 1.0) to a DateTime value within the specified range.
    /// </summary>
    /// <param name="percentage">The percentage value (0.0 to 1.0)</param>
    /// <param name="minBound">The minimum bound of the target range</param>
    /// <param name="maxBound">The maximum bound of the target range</param>
    /// <returns>The calculated DateTime value within the specified range</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minBound is greater than maxBound</exception>
    public static DateTime PercentageToDateTime(this double percentage, DateTime minBound, DateTime maxBound)
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
            _ => minBound + TimeSpan.FromMilliseconds(percentage * (maxBound - minBound).TotalMilliseconds),
        };
    }

    /// <summary>
    /// Converts a DateTime value within a specified range to a percentage.
    /// Values outside the range will produce percentages less than 0.0 or greater than 1.0.
    /// </summary>
    /// <param name="value">The DateTime value to convert</param>
    /// <param name="minBound">The minimum bound of the source range</param>
    /// <param name="maxBound">The maximum bound of the source range</param>
    /// <returns>The percentage representation (can be negative or greater than 1.0 for out-of-bounds values)</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minBound is greater than maxBound</exception>
    /// <exception cref="ArgumentException">Thrown when minBound equals maxBound</exception>
    public static double DateTimeToPercentage(this DateTime value, DateTime minBound, DateTime maxBound)
    {
        if (minBound > maxBound)
        {
            throw new ArgumentOutOfRangeException($"{nameof(minBound)} can't be bigger than {nameof(maxBound)} ({minBound}>{maxBound})");
        }

        var denominator = (maxBound - minBound).TotalMilliseconds;
        if (denominator == 0)
        {
            throw new ArgumentException($"{nameof(maxBound)} and {nameof(minBound)} must not be equal. ({maxBound} == {minBound})");
        }

        return (value - minBound).TotalMilliseconds / denominator;
    }

    #endregion
}
