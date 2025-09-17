namespace Plugin.BaseTypeExtensions.Tests;

public class ByteExtensionsTests
{
    [Fact]
    public void ToBitsString_ReturnsExpectedRepresentation()
    {
        ((byte)0b_0001_1110).ToBitsString().Should().Be("00011110");
    }

    [Fact]
    public void ToReversedByte_ReversesBits()
    {
        ((byte)0b_0101_0010).ToReversedByte().Should().Be((byte)0b_0100_1010);
    }

    [Fact]
    public void KeepOnlyUpperNibbleBits_MasksLowerBits()
    {
        ((byte)0b_1010_1100).KeepOnlyUpperNibbleBits().Should().Be((byte)0b_1010_0000);
    }

    [Fact]
    public void GetLowerNibbleBitsAsNewByte_ReturnsLowerNibble()
    {
        ((byte)0b_1010_1101).GetLowerNibbleBitsAsNewByte().Should().Be((byte)0b_0000_1101);
    }

    [Fact]
    public void GetUpperNibbleBitsAsNewByte_ShiftsDown()
    {
        ((byte)0b_1111_0001).GetUpperNibbleBitsAsNewByte().Should().Be((byte)0b_0000_1111);
    }
}
