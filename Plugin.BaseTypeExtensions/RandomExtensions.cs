namespace Plugin.BaseTypeExtensions;

/// <summary>
///     Provides extension methods for the <see cref="Random"/> class.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    ///     Returns true with a probability of <paramref name="chances"/> out of <paramref name="outOf"/>.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="chances">The number of chances for true.</param>
    /// <param name="outOf">The total number of possible outcomes.</param>
    /// <returns>True with the specified probability; otherwise, false.</returns>
    public static bool NextBool(this Random random, int chances, int outOf)
    {
        if (chances <= 0)
        {
            return false; // If there are no chances, return false
        }
        if (outOf <= 0)
        {
            return false; // If outOf is zero or less, return false to avoid division by zero
        }
        if (outOf < chances)
        {
            return true; // If outOf is less than chances, return true (100% chance)
        }
        ArgumentNullException.ThrowIfNull(random);
#pragma warning disable CA5394 // Do not use insecure randomness
        return random.NextDouble() < (double)chances / outOf;
#pragma warning restore CA5394 // Do not use insecure randomness
    }

    /// <summary>
    ///     Returns a random double value between <paramref name="min"/> (inclusive) and <paramref name="max"/> (exclusive).
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="min">The inclusive lower bound.</param>
    /// <param name="max">The exclusive upper bound.</param>
    /// <returns>A random double value in the specified range.</returns>
    public static double NextDouble(this Random random, double min = 0, double max = 1)
    {
        ArgumentNullException.ThrowIfNull(random);
#pragma warning disable CA5394 // Do not use insecure randomness
        return min + random.NextDouble() * (max - min);
#pragma warning restore CA5394 // Do not use insecure randomness
    }

    /// <summary>
    ///     Returns a random byte value between 0 and 254.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <returns>A random byte value.</returns>
    public static byte NextByte(this Random random)
    {
        ArgumentNullException.ThrowIfNull(random);
#pragma warning disable CA5394 // Do not use insecure randomness
        return ((double)random.Next(0, 255)).ToByte();
#pragma warning restore CA5394 // Do not use insecure randomness
    }
}
