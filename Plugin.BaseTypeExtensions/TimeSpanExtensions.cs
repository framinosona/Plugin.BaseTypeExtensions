using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="TimeSpan"/> operations and utility functions.
/// </summary>
public static class TimeSpanExtensions
{
    #region Readable Formatting

    /// <summary>
    /// Converts the TimeSpan to a human-readable string format.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to format.</param>
    /// <returns>A human-readable string representation.</returns>
    public static string ToReadableString(this TimeSpan timeSpan)
    {
        var parts = new List<string>();

        if (timeSpan.Days > 0)
        {
            parts.Add($"{timeSpan.Days} day{(timeSpan.Days == 1 ? "" : "s")}");
        }

        if (timeSpan.Hours > 0)
        {
            parts.Add($"{timeSpan.Hours} hour{(timeSpan.Hours == 1 ? "" : "s")}");
        }

        if (timeSpan.Minutes > 0)
        {
            parts.Add($"{timeSpan.Minutes} minute{(timeSpan.Minutes == 1 ? "" : "s")}");
        }

        if (timeSpan.Seconds > 0)
        {
            parts.Add($"{timeSpan.Seconds} second{(timeSpan.Seconds == 1 ? "" : "s")}");
        }

        if (parts.Count == 0)
        {
            if (timeSpan.Milliseconds > 0)
            {
                return $"{timeSpan.Milliseconds} millisecond{(timeSpan.Milliseconds == 1 ? "" : "s")}";
            }
            return "0 seconds";
        }

        return string.Join(", ", parts);
    }

    /// <summary>
    /// Converts the TimeSpan to a short string format.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to format.</param>
    /// <returns>A short string representation (e.g., "1d 2h 30m").</returns>
    public static string ToShortString(this TimeSpan timeSpan)
    {
        var parts = new List<string>();

        if (timeSpan.Days > 0)
        {
            parts.Add($"{timeSpan.Days}d");
        }

        if (timeSpan.Hours > 0)
        {
            parts.Add($"{timeSpan.Hours}h");
        }

        if (timeSpan.Minutes > 0)
        {
            parts.Add($"{timeSpan.Minutes}m");
        }

        if (timeSpan.Seconds > 0)
        {
            parts.Add($"{timeSpan.Seconds}s");
        }

        if (parts.Count == 0)
        {
            if (timeSpan.Milliseconds > 0)
            {
                return $"{timeSpan.Milliseconds}ms";
            }
            return "0s";
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Converts the TimeSpan to a human-readable format with approximate values.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to format.</param>
    /// <returns>A human-readable string with approximate values (e.g., "about 2 hours").</returns>
    public static string ToHumanReadable(this TimeSpan timeSpan)
    {
        var absoluteSpan = timeSpan.Duration();

        if (absoluteSpan.TotalSeconds < 60)
        {
            return "less than a minute";
        }

        if (absoluteSpan.TotalMinutes < 60)
        {
            var minutes = (int)absoluteSpan.TotalMinutes;
            return $"about {minutes} minute{(minutes == 1 ? "" : "s")}";
        }

        if (absoluteSpan.TotalHours < 24)
        {
            var hours = (int)absoluteSpan.TotalHours;
            return $"about {hours} hour{(hours == 1 ? "" : "s")}";
        }

        if (absoluteSpan.TotalDays < 30)
        {
            var days = (int)absoluteSpan.TotalDays;
            return $"about {days} day{(days == 1 ? "" : "s")}";
        }

        if (absoluteSpan.TotalDays < 365)
        {
            var months = (int)(absoluteSpan.TotalDays / 30);
            return $"about {months} month{(months == 1 ? "" : "s")}";
        }

        var years = (int)(absoluteSpan.TotalDays / 365);
        return $"about {years} year{(years == 1 ? "" : "s")}";
    }

    #endregion

    #region Conversion Helpers

    /// <summary>
    /// Converts the TimeSpan to weeks (integer part only).
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to convert.</param>
    /// <returns>The number of complete weeks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToWeeks(this TimeSpan timeSpan)
    {
        return (int)(timeSpan.TotalDays / 7);
    }

    /// <summary>
    /// Converts the TimeSpan to weeks with decimal precision.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to convert.</param>
    /// <returns>The number of weeks as a double.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ToWeeksExact(this TimeSpan timeSpan)
    {
        return timeSpan.TotalDays / 7.0;
    }

    /// <summary>
    /// Converts the TimeSpan to business days (excluding weekends).
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to convert.</param>
    /// <returns>The approximate number of business days.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToBusinessDays(this TimeSpan timeSpan)
    {
        var totalDays = (int)timeSpan.TotalDays;
        var weeks = totalDays / 7;
        var remainingDays = totalDays % 7;

        // Assume remaining days are business days (approximation)
        return (weeks * 5) + Math.Min(remainingDays, 5);
    }

    #endregion

    #region Validation

    /// <summary>
    /// Determines whether the TimeSpan is positive.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to check.</param>
    /// <returns>True if the TimeSpan is positive; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPositive(this TimeSpan timeSpan)
    {
        return timeSpan > TimeSpan.Zero;
    }

    /// <summary>
    /// Determines whether the TimeSpan is negative.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to check.</param>
    /// <returns>True if the TimeSpan is negative; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNegative(this TimeSpan timeSpan)
    {
        return timeSpan < TimeSpan.Zero;
    }

    /// <summary>
    /// Determines whether the TimeSpan is zero.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to check.</param>
    /// <returns>True if the TimeSpan is zero; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this TimeSpan timeSpan)
    {
        return timeSpan == TimeSpan.Zero;
    }

    /// <summary>
    /// Determines whether the TimeSpan is less than another TimeSpan.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to compare.</param>
    /// <param name="other">The TimeSpan to compare against.</param>
    /// <returns>True if the TimeSpan is less than the other; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThan(this TimeSpan timeSpan, TimeSpan other)
    {
        return timeSpan < other;
    }

    /// <summary>
    /// Determines whether the TimeSpan is greater than another TimeSpan.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to compare.</param>
    /// <param name="other">The TimeSpan to compare against.</param>
    /// <returns>True if the TimeSpan is greater than the other; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThan(this TimeSpan timeSpan, TimeSpan other)
    {
        return timeSpan > other;
    }

    #endregion

    #region Math Operations

    /// <summary>
    /// Multiplies the TimeSpan by a factor.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to multiply.</param>
    /// <param name="factor">The multiplication factor.</param>
    /// <returns>A new TimeSpan that is the result of the multiplication.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Multiply(this TimeSpan timeSpan, double factor)
    {
        return TimeSpan.FromTicks((long)(timeSpan.Ticks * factor));
    }

    /// <summary>
    /// Divides the TimeSpan by a divisor.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to divide.</param>
    /// <param name="divisor">The division divisor.</param>
    /// <returns>A new TimeSpan that is the result of the division.</returns>
    /// <exception cref="DivideByZeroException">Thrown when divisor is zero.</exception>
    public static TimeSpan Divide(this TimeSpan timeSpan, double divisor)
    {
        if (Math.Abs(divisor) < double.Epsilon)
        {
            throw new DivideByZeroException("Cannot divide TimeSpan by zero.");
        }

        return TimeSpan.FromTicks((long)(timeSpan.Ticks / divisor));
    }

    /// <summary>
    /// Calculates the average of a collection of TimeSpans.
    /// </summary>
    /// <param name="timeSpans">The collection of TimeSpans.</param>
    /// <returns>The average TimeSpan.</returns>
    /// <exception cref="ArgumentNullException">Thrown when timeSpans is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the collection is empty.</exception>
    public static TimeSpan Average(this IEnumerable<TimeSpan> timeSpans)
    {
        ArgumentNullException.ThrowIfNull(timeSpans);

        var spans = timeSpans.ToArray();
        if (spans.Length == 0)
        {
            throw new InvalidOperationException("Cannot calculate average of empty collection.");
        }

        var totalTicks = spans.Sum(ts => ts.Ticks);
        return TimeSpan.FromTicks(totalTicks / spans.Length);
    }

    #endregion

    #region Rounding

    /// <summary>
    /// Rounds the TimeSpan to the nearest minute.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to round.</param>
    /// <returns>A TimeSpan rounded to the nearest minute.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan RoundToMinutes(this TimeSpan timeSpan)
    {
        return timeSpan.RoundTo(TimeSpan.FromMinutes(1));
    }

    /// <summary>
    /// Rounds the TimeSpan to the nearest hour.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to round.</param>
    /// <returns>A TimeSpan rounded to the nearest hour.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan RoundToHours(this TimeSpan timeSpan)
    {
        return timeSpan.RoundTo(TimeSpan.FromHours(1));
    }

    /// <summary>
    /// Rounds the TimeSpan to the nearest day.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to round.</param>
    /// <returns>A TimeSpan rounded to the nearest day.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan RoundToDays(this TimeSpan timeSpan)
    {
        return timeSpan.RoundTo(TimeSpan.FromDays(1));
    }

    /// <summary>
    /// Rounds the TimeSpan to the nearest specified interval.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to round.</param>
    /// <param name="interval">The interval to round to.</param>
    /// <returns>A TimeSpan rounded to the nearest interval.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when interval is zero or negative.</exception>
    public static TimeSpan RoundTo(this TimeSpan timeSpan, TimeSpan interval)
    {
        if (interval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be positive.");
        }

        var ticks = timeSpan.Ticks;
        var intervalTicks = interval.Ticks;

        var remainder = ticks % intervalTicks;
        var halfInterval = intervalTicks / 2;

        if (remainder >= halfInterval)
        {
            return TimeSpan.FromTicks(ticks + (intervalTicks - remainder));
        }

        return TimeSpan.FromTicks(ticks - remainder);
    }

    #endregion
}
