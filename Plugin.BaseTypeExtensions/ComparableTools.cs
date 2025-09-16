using Plugin.BaseTypeExtensions;

namespace Laerdal.Helpers.Tools;

/// <summary>
/// Provides helper methods for working with types implementing <see cref="IComparable{T}"/>.
/// </summary>
public static class ComparableTools
{
    #region Min/Max

    /// <summary>
    /// Returns the minimum of two values implementing <see cref="IComparable{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type implementing <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>The minimum value of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static T Min<T>(T a, T b) where T : IComparable<T>
    {
        return a.IsLessThan(b) ? a : b;
    }

    /// <summary>
    /// Returns the maximum of two values implementing <see cref="IComparable{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type implementing <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>The maximum value of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.IsGreaterThan(b) ? a : b;
    }

    #endregion
}
