namespace Plugin.BaseTypeExtensions.Tests;

public class VersionExtensionsTests
{
    [Fact]
    public void ToFullVersion_NullVersion_ReturnsNull()
    {
        // Arrange
        Version? version = null;

        // Act
        var result = version.ToFullVersion();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ToFullVersion_AlreadyFullVersion_ReturnsSameInstance()
    {
        // Arrange
        var version = new Version(1, 2, 3, 4);

        // Act
        var result = version.ToFullVersion();

        // Assert
        result.Should().BeSameAs(version);
    }

    [Fact]
    public void ToFullVersion_TwoComponentVersion_ReturnsFullVersion()
    {
        // Arrange
        var version = new Version(1, 2);

        // Act
        var result = version.ToFullVersion();

        // Assert
        result.Should().NotBeNull();
        result!.Major.Should().Be(1);
        result.Minor.Should().Be(2);
        result.Build.Should().Be(0);
        result.Revision.Should().Be(0);
    }

    [Fact]
    public void ToFullVersion_ThreeComponentVersion_ReturnsFullVersion()
    {
        // Arrange
        var version = new Version(1, 2, 3);

        // Act
        var result = version.ToFullVersion();

        // Assert
        result.Should().NotBeNull();
        result!.Major.Should().Be(1);
        result.Minor.Should().Be(2);
        result.Build.Should().Be(3);
        result.Revision.Should().Be(0);
    }

    [Fact]
    public void ToFullVersion_VersionWithUnsetFields_ReplacesWithZero()
    {
        // Arrange
        var version = new Version(1, 2); // Build and Revision will be -1

        // Act
        var result = version.ToFullVersion();

        // Assert
        result.Should().NotBeNull();
        result!.Build.Should().Be(0, "unset build field should be replaced with 0");
        result.Revision.Should().Be(0, "unset revision field should be replaced with 0");
    }

    [Fact]
    public void ToFullVersion_EnsuresConsistentComparison()
    {
        // Arrange
        var version1 = new Version(1, 2);
        var version2 = new Version(1, 2, 0, 0);

        // Act
        var fullVersion1 = version1.ToFullVersion();
        var fullVersion2 = version2.ToFullVersion();

        // Assert
        fullVersion1.Should().NotBeNull();
        fullVersion2.Should().NotBeNull();
        fullVersion1!.CompareTo(fullVersion2).Should().Be(0, "versions should be equal after normalization");
    }
}
