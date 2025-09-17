using System.Text;

namespace Plugin.BaseTypeExtensions.Tests;

public class StreamExtensionsTests
{
    [Fact]
    public void ToByteArray_ConvertsStreamToByteArray()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var result = stream.ToByteArray();

        // Assert
        result.Should().Equal(data);
    }

    [Fact]
    public async Task ToByteArrayAsync_ConvertsStreamToByteArray()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var result = await stream.ToByteArrayAsync();

        // Assert
        result.Should().Equal(data);
    }

    [Fact]
    public void ToStringContent_ConvertsStreamToString()
    {
        // Arrange
        var text = "Hello, World!";
        var data = Encoding.UTF8.GetBytes(text);
        using var stream = new MemoryStream(data);

        // Act
        var result = stream.ToStringContent();

        // Assert
        result.Should().Be(text);
    }

    [Fact]
    public async Task ToStringContentAsync_ConvertsStreamToString()
    {
        // Arrange
        var text = "Hello, World!";
        var data = Encoding.UTF8.GetBytes(text);
        using var stream = new MemoryStream(data);

        // Act
        var result = await stream.ToStringContentAsync();

        // Assert
        result.Should().Be(text);
    }

    [Fact]
    public void ToMemoryStream_ConvertsToMemoryStream()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        using var result = stream.ToMemoryStream();

        // Assert
        result.Should().BeOfType<MemoryStream>();
        result.ToArray().Should().Equal(data);
    }

    [Fact]
    public void Reset_ResetsStreamPosition()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        stream.Position = 3;

        // Act
        stream.Reset();

        // Assert
        stream.Position.Should().Be(0);
    }

    [Fact]
    public void CanReset_ReturnsTrueForSeekableStream()
    {
        // Arrange
        using var stream = new MemoryStream();

        // Act
        var result = stream.CanReset();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetLength_ReturnsStreamLength()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var result = stream.GetLength();

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void CopyToWithProgress_CopiesStreamWithProgress()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();
        var progressReports = new List<long>();
        var progress = new Progress<long>(value => progressReports.Add(value));

        // Act
        source.CopyToWithProgress(destination, progress);

        // Assert
        destination.ToArray().Should().Equal(data);
        progressReports.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CopyToWithProgressAsync_CopiesStreamWithProgress()
    {
        // Arrange
        var data = new byte[8192]; // Larger data to ensure progress reporting
        for (var i = 0; i < data.Length; i++)
        {
            data[i] = (byte)(i % 256);
        }
        using var source = new MemoryStream(data);
        using var destination = new MemoryStream();
        var progressReports = new List<long>();
        var progress = new Progress<long>(value => progressReports.Add(value));

        // Act
        await source.CopyToWithProgressAsync(destination, progress);

        // Assert
        destination.ToArray().Should().Equal(data);
        progressReports.Should().Contain(value => value >= 0); // At least one progress report
    }

    [Fact]
    public void SaveToFile_CreatesFileWithStreamContent()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            stream.SaveToFile(tempFile);

            // Assert
            var fileContent = File.ReadAllBytes(tempFile);
            fileContent.Should().Equal(data);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public async Task SaveToFileAsync_CreatesFileWithStreamContent()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            await stream.SaveToFileAsync(tempFile);

            // Assert
            var fileContent = await File.ReadAllBytesAsync(tempFile);
            fileContent.Should().Equal(data);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
