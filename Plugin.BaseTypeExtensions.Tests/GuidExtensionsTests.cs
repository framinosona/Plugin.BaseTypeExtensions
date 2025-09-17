namespace Plugin.BaseTypeExtensions.Tests;

public class GuidExtensionsTests
{
    [Fact]
    public void GetPlusOne_IncrementsGuid()
    {
        // Arrange
        var guid = Guid.Parse("00000000-0000-0000-0000-0000000000FF");
        var expected = Guid.Parse("00000000-0000-0000-0000-000000000100");

        // Act
        var result = guid.GetPlusOne();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GetMinusOne_DecrementsGuid()
    {
        // Arrange
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000100");
        var expected = Guid.Parse("00000000-0000-0000-0000-0000000000FF");

        // Act
        var result = guid.GetMinusOne();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GetPlusOne_OverflowHandling()
    {
        // Arrange
        var guid = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

        // Act
        var result = guid.GetPlusOne();

        // Assert
        result.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000000"));
    }

    [Fact]
    public void GetMinusOne_UnderflowHandling()
    {
        // Arrange
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");

        // Act
        var result = guid.GetMinusOne();

        // Assert
        result.Should().Be(Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"));
    }
}
