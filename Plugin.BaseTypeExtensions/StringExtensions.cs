using System.Text;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for string manipulation and utility functions.
/// </summary>
public static partial class StringExtensions
{
    /// <summary>Returns null if the input is null or whitespace. The given string otherwise. Usage:
    /// <code>
    /// advertisementInfo
    ///     .ServicesGuids
    ///     .Select(x => $"- {x}")
    ///     .StringJoinify("\n")
    ///     .NullIfDud() ?? "[🐟 this looks fishy 🐟 no services found]"
    /// </code>
    /// </summary>
    /// <param name="input">The input string</param>
    /// <returns>Null if the input is null or whitespace; otherwise, the input string.</returns>
    public static string? NullIfDud(this string? input)
    {
        return string.IsNullOrWhiteSpace(input)
        ? null
        : input;
    }

    /// <summary>
    /// Removes special characters from the string, preserving alphanumeric, space, dot, underscore, and hyphen.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The string with special characters removed.</returns>
    public static string RemoveSpecialCharacters(this string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var result = new StringBuilder(input.Length);
        foreach (var c in input)
        {
            if ((c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                (c >= '0' && c <= '9') ||
                c == ' ' ||
                c == '.' ||
                c == '_' ||
                c == '-')
            {
                result.Append(c);
            }
        }
        return result.ToString();
    }
}
