using System.Numerics;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for generic numeric operations, including angle conversions, clamping, and percentage calculations.
/// </summary>
public static class NumericExtensions
{
    #region Angles - Generic Numeric Types

    /// <summary>
    /// Converts an angle from degrees to radians for any numeric type.
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements INumber&lt;T&gt;</typeparam>
    /// <param name="angleInDegrees">The angle in degrees</param>
    /// <returns>The angle in radians</returns>
    public static T DegreeToRadian<T>(this T angleInDegrees) where T : INumber<T>
    {
        // Convert Math.PI to type T and perform the calculation
        var pi = T.CreateChecked(Math.PI);
        var divisor = T.CreateChecked(180.0);
        return pi * angleInDegrees / divisor;
    }

    /// <summary>
    /// Converts an angle from radians to degrees for any numeric type.
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements INumber&lt;T&gt;</typeparam>
    /// <param name="angleInRadians">The angle in radians</param>
    /// <returns>The angle in degrees</returns>
    public static T RadianToDegree<T>(this T angleInRadians) where T : INumber<T>
    {
        // Convert 180/PI to type T and perform the calculation
        var multiplier = T.CreateChecked(180.0 / Math.PI);
        return angleInRadians * multiplier;
    }

    #endregion

    #region Percentage - Generic Numeric Types

    /// <summary>
    /// Converts a percentage (0.0 to 1.0) to a value within the specified range for any numeric type.
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements INumber&lt;T&gt;</typeparam>
    /// <param name="percentage">The percentage value (0.0 to 1.0)</param>
    /// <param name="minBound">The minimum bound of the target range</param>
    /// <param name="maxBound">The maximum bound of the target range</param>
    /// <returns>The calculated value within the specified range</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minBound is greater than maxBound</exception>
    public static T PercentageToValue<T>(this T percentage, T minBound, T maxBound) where T : INumber<T>
    {
        if (minBound > maxBound)
        {
            throw new ArgumentOutOfRangeException($"{nameof(minBound)} can't be bigger than {nameof(maxBound)} ({minBound}>{maxBound})");
        }

        var zero = T.Zero;
        var one = T.One;

        return percentage switch
        {
            var p when p >= one => maxBound,
            var p when p <= zero => minBound,
            _ => minBound + percentage * (maxBound - minBound),
        };
    }

    /// <summary>
    /// Converts a value within a range to a percentage (0.0 to 1.0) for any numeric type.
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements INumber&lt;T&gt;</typeparam>
    /// <param name="value">The value to convert</param>
    /// <param name="minBound">The minimum bound of the source range</param>
    /// <param name="maxBound">The maximum bound of the source range</param>
    /// <returns>The percentage representation (0.0 to 1.0)</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minBound is greater than maxBound</exception>
    public static T ValueToPercentage<T>(this T value, T minBound, T maxBound) where T : INumber<T>
    {
        if (minBound > maxBound)
        {
            throw new ArgumentOutOfRangeException($"{nameof(minBound)} can't be bigger than {nameof(maxBound)} ({minBound}>{maxBound})");
        }

        var denominator = maxBound - minBound;

        if (denominator == T.Zero)
        {
            throw new DivideByZeroException($"{nameof(maxBound)} and {nameof(minBound)} must not be equal. ({maxBound} == {minBound})");
        }

        var zero = T.Zero;
        var one = T.One;

        if (value <= minBound)
        {
            return zero;
        }

        if (value >= maxBound)
        {
            return one;
        }

        return (value - minBound) / denominator;
    }

    #endregion

    #region Byte Conversion - Generic Numeric Types

    /// <summary>
    /// Converts a numeric value to a byte, clamping to valid byte range (0-255) for any numeric type.
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements INumber&lt;T&gt;</typeparam>
    /// <param name="input">The numeric value to convert</param>
    /// <returns>A byte value clamped to the range 0-255</returns>
    public static byte ToByte<T>(this T input) where T : INumber<T>
    {
        var zero = T.Zero;
        var maxByte = T.CreateChecked(255);

        if (input < zero)
        {
            return 0;
        }
        if (input > maxByte)
        {
            return 255;
        }

        // Convert to double for rounding, then to byte
        var doubleValue = double.CreateChecked(input);
        return (byte)Math.Round(doubleValue);
    }

    /// <summary>
    /// Converts a percentage (0.0 to 1.0) to a byte value (0-255) for any numeric type.
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements INumber&lt;T&gt;</typeparam>
    /// <param name="percentage">The percentage value (0.0 to 1.0)</param>
    /// <returns>A byte value in the range 0-255</returns>
    public static byte PercentageToByte<T>(this T percentage) where T : INumber<T>
    {
        var minBound = T.Zero;
        var maxBound = T.CreateChecked(255);
        return percentage.PercentageToValue(minBound, maxBound).ToByte();
    }

    #endregion

    #region Clamping - Generic Numeric Types

    /// <summary>
    /// Clamps a numeric value between a minimum and maximum value for any numeric type.
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements INumber&lt;T&gt;</typeparam>
    /// <param name="value">The value to clamp</param>
    /// <param name="min">The minimum allowed value</param>
    /// <param name="max">The maximum allowed value</param>
    /// <returns>The clamped value</returns>
    public static T Clamp<T>(this T value, T min, T max) where T : INumber<T>
    {
        if (min > max)
        {
            throw new ArgumentOutOfRangeException($"{nameof(min)} can't be bigger than {nameof(max)} ({min}>{max})");
        }

        return T.Clamp(value, min, max);
    }

    #endregion

    #region Inversion - Generic Numeric Types

    /// <summary>
    /// Returns the value as-is or inverted based on the isInverted flag for any numeric type.
    /// When inverted, returns maxValue - value. When not inverted, returns the original value.
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements INumber&lt;T&gt;</typeparam>
    /// <param name="value">The value to potentially invert</param>
    /// <param name="isInverted">Whether to apply inversion</param>
    /// <param name="maxValue">The maximum value used for inversion calculation</param>
    /// <returns>The original value if not inverted, or maxValue - value if inverted</returns>
    public static T GetInvertedSafe<T>(this T value, bool isInverted, T maxValue) where T : INumber<T>
    {
        return isInverted ? value : maxValue - value;
    }

    #endregion
}
