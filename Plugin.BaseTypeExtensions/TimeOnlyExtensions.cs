namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="TimeOnly"/> operations, such as range checks and percentage calculations.
/// </summary>
public static class TimeOnlyExtensions
{

    #region Percentage

    /// <summary>
    /// Converts a percentage (0.0 to 1.0) to a TimeOnly value within the specified range.
    /// </summary>
    /// <param name="percentage">The percentage value (0.0 to 1.0)</param>
    /// <param name="minBound">The minimum bound of the target range</param>
    /// <param name="maxBound">The maximum bound of the target range</param>
    /// <returns>The calculated TimeOnly value within the specified range</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minBound is greater than maxBound</exception>
    public static TimeOnly PercentageToTimeOnly(this double percentage, TimeOnly minBound, TimeOnly maxBound)
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
            _ => minBound.Add(TimeSpan.FromMilliseconds(percentage * (maxBound - minBound).TotalMilliseconds)),
        };
    }

    /// <summary>
    /// Converts a TimeOnly value within a specified range to a percentage (0.0 to 1.0).
    /// </summary>
    /// <param name="value">The TimeOnly value to convert</param>
    /// <param name="minBound">The minimum bound of the source range</param>
    /// <param name="maxBound">The maximum bound of the source range</param>
    /// <returns>The percentage representation (0.0 to 1.0)</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minBound is greater than maxBound</exception>
    public static double TimeOnlyToPercentage(this TimeOnly value, TimeOnly minBound, TimeOnly maxBound)
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

        if (value <= minBound)
        {
            return 0;
        }

        if (value >= maxBound)
        {
            return 1;
        }

        return (value - minBound).TotalMilliseconds / denominator;
    }

    #endregion
}
