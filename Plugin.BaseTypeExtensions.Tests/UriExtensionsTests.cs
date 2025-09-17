namespace Plugin.BaseTypeExtensions.Tests;

public class UriExtensionsTests
{
    [Fact]
    public void AppendPath_AppendsPathCorrectly()
    {
        // Arrange
        var uri = new Uri("https://example.com/api");
        var path = "users/123";

        // Act
        var result = uri.AppendPath(path);

        // Assert
        result.ToString().Should().Be("https://example.com/api/users/123");
    }

    [Fact]
    public void AppendQuery_SingleParameter_AppendsCorrectly()
    {
        // Arrange
        var uri = new Uri("https://example.com");
        var key = "name";
        var value = "test";

        // Act
        var result = uri.AppendQuery(key, value);

        // Assert
        result.ToString().Should().Be("https://example.com/?name=test");
    }

    [Fact]
    public void AppendQuery_MultipleParameters_AppendsCorrectly()
    {
        // Arrange
        var uri = new Uri("https://example.com");
        var parameters = new Dictionary<string, string>
        {
            { "name", "test" },
            { "id", "123" }
        };

        // Act
        var result = uri.AppendQuery(parameters);

        // Assert
        result.Query.Should().Contain("name=test");
        result.Query.Should().Contain("id=123");
    }

    [Fact]
    public void RemoveQuery_RemovesAllQueryParameters()
    {
        // Arrange
        var uri = new Uri("https://example.com?name=test&id=123");

        // Act
        var result = uri.RemoveQuery();

        // Assert
        result.ToString().Should().Be("https://example.com/");
    }

    [Fact]
    public void RemoveQuery_SpecificKey_RemovesOnlyThatParameter()
    {
        // Arrange
        var uri = new Uri("https://example.com?name=test&id=123");

        // Act
        var result = uri.RemoveQuery("name");

        // Assert
        result.Query.Should().Contain("id=123");
        result.Query.Should().NotContain("name=test");
    }

    [Fact]
    public void GetQueryParameters_ReturnsAllParameters()
    {
        // Arrange
        var uri = new Uri("https://example.com?name=test&id=123");

        // Act
        var result = uri.GetQueryParameters();

        // Assert
        result.Should().ContainKey("name").WhoseValue.Should().Be("test");
        result.Should().ContainKey("id").WhoseValue.Should().Be("123");
    }

    [Fact]
    public void GetQueryParameter_ReturnsSpecificParameter()
    {
        // Arrange
        var uri = new Uri("https://example.com?name=test&id=123");

        // Act
        var result = uri.GetQueryParameter("name");

        // Assert
        result.Should().Be("test");
    }

    [Fact]
    public void GetQueryParameter_NonExistentKey_ReturnsDefault()
    {
        // Arrange
        var uri = new Uri("https://example.com?name=test");

        // Act
        var result = uri.GetQueryParameter("nonexistent", "default");

        // Assert
        result.Should().Be("default");
    }

    [Fact]
    public void HasQueryParameter_ExistingKey_ReturnsTrue()
    {
        // Arrange
        var uri = new Uri("https://example.com?name=test");

        // Act
        var result = uri.HasQueryParameter("name");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasQueryParameter_NonExistentKey_ReturnsFalse()
    {
        // Arrange
        var uri = new Uri("https://example.com?name=test");

        // Act
        var result = uri.HasQueryParameter("nonexistent");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SetScheme_ChangesScheme()
    {
        // Arrange
        var uri = new Uri("http://example.com");

        // Act
        var result = uri.SetScheme("https");

        // Assert
        result.Scheme.Should().Be("https");
        result.Host.Should().Be("example.com");
    }

    [Fact]
    public void SetPort_ChangesPort()
    {
        // Arrange
        var uri = new Uri("https://example.com:80");

        // Act
        var result = uri.SetPort(443);

        // Assert
        result.Port.Should().Be(443);
        result.Host.Should().Be("example.com");
    }

    [Fact]
    public void IsLocalhost_LocalhostUri_ReturnsTrue()
    {
        // Arrange
        var uri = new Uri("https://localhost:5000");

        // Act
        var result = uri.IsLocalhost();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLocalhost_RemoteUri_ReturnsFalse()
    {
        // Arrange
        var uri = new Uri("https://example.com");

        // Act
        var result = uri.IsLocalhost();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsSecure_HttpsUri_ReturnsTrue()
    {
        // Arrange
        var uri = new Uri("https://example.com");

        // Act
        var result = uri.IsSecure();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsSecure_HttpUri_ReturnsFalse()
    {
        // Arrange
        var uri = new Uri("http://example.com");

        // Act
        var result = uri.IsSecure();

        // Assert
        result.Should().BeFalse();
    }
}
