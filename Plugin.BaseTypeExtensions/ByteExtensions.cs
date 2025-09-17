namespace Plugin.BaseTypeExtensions;

/// <summary>
///     Utility method for bytes and byte-arrays.
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    ///     Converts a byte to its bit-string representation. For example '15' -> '00001111'
    /// </summary>
    /// <param name="b">The byte to convert</param>
    /// <returns>The bits-string representation</returns>
    public static string ToBitsString(this byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }

    /// <summary>
    ///     Reverses the bits inside a byte into a new byte. For example '01010010' -> '01001010'
    /// </summary>
    /// <param name="b">The byte to reverse the bits of</param>
    /// <returns>A new byte containing the result</returns>
    public static byte ToReversedByte(this byte b)
    {
        return (byte)(((b * 0x0202020202) & 0x010884422010) % 1023); // https://stackoverflow.com/a/3588105/863651
    }

    /// <summary>
    ///     Creates a new byte from the given byte which has its lower nibble zeroed out.
    ///     For example passing a byte such as 01010110 will generate a new byte 01010000.
    /// </summary>
    /// <param name="b">The byte to operate upon.</param>
    /// <returns>A new byte which is a copy of the given byte but with its lower nibble zeroed out.</returns>
    public static byte KeepOnlyUpperNibbleBits(this byte b)
    {
        return (byte)(b & 0xF0);
    }

    /// <summary>
    ///     Creates a new byte from the given byte which has its upper nibble zeroed out.
    ///     For example passing a byte such as 01010110 will generate a new byte 00000110.
    /// </summary>
    /// <param name="b">The byte to operate upon.</param>
    /// <returns>A new byte which is a copy of the given byte but with its upper nibble zeroed out.</returns>
    public static byte GetLowerNibbleBitsAsNewByte(this byte b)
    {
        return (byte)(b & 0x0F);
    }

    /// <summary>
    ///     Creates a new byte from the given byte which has its upper nibble zeroed out while its
    ///     lower nibble is identical to the upper nibble of the given byte.
    ///     For example passing a byte such as 0101_0110 will generate a new byte 0000_0101.
    /// </summary>
    /// <param name="b">The byte to operate upon.</param>
    /// <returns>
    ///     A new byte which is a copy of the given byte but with its upper nibble zeroed out and
    ///     the lower nibble identical to the upper nibble of the given byte.
    /// </returns>
    public static byte GetUpperNibbleBitsAsNewByte(this byte b)
    {
        return (byte)(b >> 4);
    }
}
