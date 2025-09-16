using System.Numerics;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides utility methods for numeric range conversions, percentage calculations, and value mapping operations.
/// </summary>
/// <remarks>
/// This class contains helper methods for converting between absolute and relative values,
/// percentage calculations, and generating numeric ranges. All methods are optimized for
/// performance and support generic numeric types where applicable.
/// </remarks>
public static class NumericRangeTools
{

    #region Range

    /// <summary>
    /// Generates a sequence of numbers from start to end with a specified step.
    /// </summary>
    /// <typeparam name="T">The numeric type that implements INumber&lt;T&gt;.</typeparam>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <param name="step">The step size between each number in the range.</param>
    /// <returns>An IEnumerable of T values representing the range.</returns>
    /// <remarks>
    /// This method generates a sequence of numbers starting from `start` to `end`, inclusive, with the specified `step`.
    /// If `step` is zero, an empty sequence is returned.
    /// The sequence will include both `start` and `end` if they are reachable with the given step.
    /// </remarks>
    public static IEnumerable<T> GetRange<T>(T start, T end, T step) where T : INumber<T>
    {
        if (step == T.Zero)
        {
            yield break;
        }

        var minimum = T.Min(start, end);
        var maximum = T.Max(start, end);
        var current = minimum;

        while (current <= maximum)
        {
            yield return current;
            current += step;
        }
    }

    /// <summary>
    /// Generates a sequence of numbers from start to end with a specified step.
    /// </summary>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <param name="step">The step size between each number in the range.</param>
    /// <returns>An IEnumerable of double values representing the range.</returns>
    /// <remarks>
    /// This method generates a sequence of numbers starting from `start` to `end`, inclusive, with the specified `step`.
    /// If `step` is zero or NaN, an empty sequence is returned.
    /// The sequence will include both `start` and `end` if they are reachable with the given step.
    /// </remarks>
    public static IEnumerable<double> GetRange(double start, double end, double step)
    {
        if (step is 0 or double.NaN)
        {
            return [];
        }

        var minimum = Math.Min(start, end);
        var maximum = Math.Max(start, end);

        var quotientMin = (int)Math.Floor(minimum / step);
        var quotientMax = (int)Math.Floor(maximum / step);

        var numberOfSteps = quotientMax - quotientMin;

        var steps = Range(0, numberOfSteps).Select(index => (quotientMin + index) * step);

        return steps;
    }

    /// <summary>
    /// Generates a sequence of floating-point numbers from start to end with a specified step.
    /// </summary>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <param name="step">The step size between each number in the range.</param>
    /// <returns>An IEnumerable of float values representing the range.</returns>
    /// <remarks>
    /// This method generates a sequence of floating-point numbers starting from `start` to `end`, inclusive, with the specified `step`.
    /// If `step` is zero or NaN, an empty sequence is returned.
    /// The sequence will include both `start` and `end` if they are reachable with the given step.
    /// </remarks>
    public static IEnumerable<float> GetRange(float start, float end, float step)
    {
        if (step is 0 or float.NaN)
        {
            return [];
        }

        var minimum = Math.Min(start, end);
        var maximum = Math.Max(start, end);

        var quotientMin = (int)Math.Floor(minimum / step);
        var quotientMax = (int)Math.Floor(maximum / step);

        var numberOfSteps = quotientMax - quotientMin;

        var steps = Range(0, numberOfSteps).Select(index => (quotientMin + index) * step);

        return steps;
    }

    /// <summary>
    /// Generates a sequence of DateTime values from start to end with a specified step.
    /// </summary>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <param name="step">The step size between each DateTime in the range.</param>
    /// <returns>An IEnumerable of DateTime values representing the range.</returns>
    /// <remarks>
    /// This method generates a sequence of DateTime values starting from `start` to `end`, inclusive, with the specified `step`.
    /// If `step` is zero, an empty sequence is returned.
    /// The sequence will include both `start` and `end` if they are reachable with the given step.
    /// </remarks>
    public static IEnumerable<DateTime> GetRange(DateTime start, DateTime end, TimeSpan step)
    {
        if (step == TimeSpan.Zero)
        {
            return Array.Empty<DateTime>();
        }

        var minimum = start < end ? start : end;
        var maximum = start < end ? end : start;

        var quotientMin = Math.DivRem(minimum.Ticks, step.Ticks, out var remainderMin);
        var quotientMax = Math.DivRem(maximum.Ticks, step.Ticks, out _);

        var minTime = minimum.AddTicks(-remainderMin);

        var numberOfSteps = (int)Math.Floor((double)(quotientMax - quotientMin));

        var steps = Range(0, numberOfSteps).Select(index => minTime + TimeSpan.FromTicks(index * step.Ticks));

        return steps;
    }

    /// <summary>
    /// Generates a sequence of integers from start to end, inclusive.
    /// </summary>
    /// <param name="startIndex">The start of the range.</param>
    /// <param name="endIndex">The end of the range.</param>
    /// <returns>An IEnumerable of integers representing the range.</returns>
    /// <remarks>
    /// This method generates a sequence of integers starting from `startIndex` to `endIndex`, inclusive.
    /// If `startIndex` is greater than `endIndex`, the sequence will be empty.
    /// </remarks>
    private static IEnumerable<int> Range(int startIndex, int endIndex)
    {
        for (var currentIndex = startIndex; currentIndex <= endIndex; currentIndex++)
        {
            yield return currentIndex;
        }
    }
    #endregion

}
