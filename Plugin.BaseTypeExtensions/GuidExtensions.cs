namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="Guid"/> manipulation.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Returns a new <see cref="Guid"/> that is the next sequential value after the input.
    /// </summary>
    /// <param name="input">The input <see cref="Guid"/>.</param>
    /// <returns>The next <see cref="Guid"/> value.</returns>
    public static Guid GetPlusOne(this Guid input)
    {
        var guidString = input.ToString("N"); // Get hex string without dashes
        var bytes = Convert.FromHexString(guidString);

        // Increment the last byte (treat as big-endian)
        for (var i = bytes.Length - 1; i >= 0; i--)
        {
            if (bytes[i] < 255)
            {
                bytes[i]++;
                break;
            }
            bytes[i] = 0; // Overflow, continue to next byte
        }

        var newGuidString = Convert.ToHexString(bytes);
        return new Guid(newGuidString);
    }

    /// <summary>
    /// Returns a new <see cref="Guid"/> that is the previous sequential value before the input.
    /// </summary>
    /// <param name="input">The input <see cref="Guid"/>.</param>
    /// <returns>The previous <see cref="Guid"/> value.</returns>
    public static Guid GetMinusOne(this Guid input)
    {
        var guidString = input.ToString("N"); // Get hex string without dashes
        var bytes = Convert.FromHexString(guidString);

        // Decrement the last byte (treat as big-endian)
        for (var i = bytes.Length - 1; i >= 0; i--)
        {
            if (bytes[i] > 0)
            {
                bytes[i]--;
                break;
            }
            bytes[i] = 255; // Underflow, continue to next byte
        }

        var newGuidString = Convert.ToHexString(bytes);
        return new Guid(newGuidString);
    }
}
