namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for types implementing <see cref="IComparable{T}"/> for range checking and comparison operations.
/// </summary>
public static class ComparableExtensions
{
    #region Range Checking

    /// <summary>
    /// Checks if a comparable value is between two bounds.
    /// Uses <see cref="ComparableTools.Min{T}"/> and <see cref="ComparableTools.Max{T}"/> to determine the actual min/max regardless of parameter order.
    /// Then uses the comparison extension methods for readable and consistent logic.
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to check</param>
    /// <param name="bound1">First boundary value (can be min or max)</param>
    /// <param name="bound2">Second boundary value (can be min or max)</param>
    /// <param name="inclusive">Whether to include the boundaries in the range (default: true)</param>
    /// <returns>True if the value is between the bounds; false otherwise</returns>
    /// <example>
    /// <code>
    /// var isInRange = 5.IsBetween(1, 10);        // true (inclusive by default)
    /// var isInRange = 5.IsBetween(10, 1);        // true (order doesn't matter)
    /// var isInRange = 5.IsBetween(1, 5, false);  // false (exclusive)
    /// var isInRange = 5.IsBetween(1, 5, true);   // true (inclusive)
    /// </code>
    /// </example>
    public static bool IsBetween<T>(this T value, T bound1, T bound2, bool inclusive = true) where T : IComparable<T>
    {
        // Use ComparableTools to determine the actual min and max values
        // This allows bounds to be passed in any order
        var min = bound1.IsLessThan(bound2) ? bound1 : bound2;
        var max = bound1.IsGreaterThan(bound2) ? bound1 : bound2;

        // Use our comparison extension methods for consistent and readable logic
        return inclusive
            ? value.IsGreaterThanOrEqual(min) && value.IsLessThanOrEqual(max)  // Include boundaries
            : value.IsGreaterThan(min) && value.IsLessThan(max);               // Exclude boundaries
    }

    #endregion

    #region Comparison Helpers

    /// <summary>
    /// Checks if the current value is greater than another comparable value.
    /// Provides a more readable alternative to <c>value.CompareTo(other) > 0</c>.
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to compare</param>
    /// <param name="other">The value to compare against</param>
    /// <returns>True if <paramref name="value"/> is greater than <paramref name="other"/>; false otherwise</returns>
    /// <example>
    /// <code>
    /// var isGreater = 10.IsGreaterThan(5);  // true
    /// var isGreater = 3.IsGreaterThan(7);   // false
    /// var isGreater = 5.IsGreaterThan(5);   // false (not greater, equal)
    /// </code>
    /// </example>
    public static bool IsGreaterThan<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) > 0;
    }

    /// <summary>
    /// Checks if the current value is greater than or equal to another comparable value.
    /// Provides a more readable alternative to <c>value.CompareTo(other) >= 0</c>.
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to compare</param>
    /// <param name="other">The value to compare against</param>
    /// <returns>True if <paramref name="value"/> is greater than or equal to <paramref name="other"/>; false otherwise</returns>
    /// <example>
    /// <code>
    /// var isGreaterOrEqual = 10.IsGreaterThanOrEqual(5);  // true
    /// var isGreaterOrEqual = 5.IsGreaterThanOrEqual(5);   // true (equal)
    /// var isGreaterOrEqual = 3.IsGreaterThanOrEqual(7);   // false
    /// </code>
    /// </example>
    public static bool IsGreaterThanOrEqual<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) >= 0;
    }

    /// <summary>
    /// Checks if the current value is less than another comparable value.
    /// Provides a more readable alternative to <c>value.CompareTo(other) &lt; 0</c>.
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to compare</param>
    /// <param name="other">The value to compare against</param>
    /// <returns>True if <paramref name="value"/> is less than <paramref name="other"/>; false otherwise</returns>
    /// <example>
    /// <code>
    /// var isLess = 3.IsLessThan(7);   // true
    /// var isLess = 10.IsLessThan(5);  // false
    /// var isLess = 5.IsLessThan(5);   // false (not less, equal)
    /// </code>
    /// </example>
    public static bool IsLessThan<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) < 0;
    }

    /// <summary>
    /// Checks if the current value is less than or equal to another comparable value.
    /// Provides a more readable alternative to <c>value.CompareTo(other) &lt;= 0</c>.
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to compare</param>
    /// <param name="other">The value to compare against</param>
    /// <returns>True if <paramref name="value"/> is less than or equal to <paramref name="other"/>; false otherwise</returns>
    /// <example>
    /// <code>
    /// var isLessOrEqual = 3.IsLessThanOrEqual(7);   // true
    /// var isLessOrEqual = 5.IsLessThanOrEqual(5);   // true (equal)
    /// var isLessOrEqual = 10.IsLessThanOrEqual(5);  // false
    /// </code>
    /// </example>
    public static bool IsLessThanOrEqual<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) <= 0;
    }

    #endregion
}
