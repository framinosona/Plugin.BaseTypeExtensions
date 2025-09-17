using System.Reflection;

namespace Plugin.BaseTypeExtensions.Tests;

public class ReflectionExtensionsTests
{
    [AttributeUsage(AttributeTargets.Class)]
    private sealed class SampleAttribute : Attribute
    {
        public SampleAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class NotUsedAttribute : Attribute
    {
    }

    [Sample("match")]
    private sealed class DecoratedType
    {
    }

    private sealed class PlainType
    {
    }

    [Fact]
    public void GetAssembly_ReturnsOwningAssembly()
    {
        var instance = new DecoratedType();
        instance.GetAssembly().Should().BeSameAs(typeof(DecoratedType).Assembly);
    }

    [Fact]
    public void GetAssembly_ThrowsOnNullInstance()
    {
        object instance = null!;
        Action action = () => instance.GetAssembly();
        action.Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void GetTypesWithAttribute_FindsDecoratedTypes()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;
        var result = assembly.GetTypesWithAttribute<SampleAttribute>();

        result.Should().ContainSingle().Which.Type.Should().Be(typeof(DecoratedType));
    }

    [Fact]
    public void GetTypesWithAttribute_ReturnsEmptyWhenNoMatches()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;
        var result = assembly.GetTypesWithAttribute<NotUsedAttribute>();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetTypesWithAttribute_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Action action = () => assembly.GetTypesWithAttribute<SampleAttribute>();
        action.Should().Throw<ArgumentNullException>().WithParameterName("assembly");
    }

    [Fact]
    public async Task GetTypesWithAttributeAsync_FindsDecoratedTypes()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;
        var result = await assembly.GetTypesWithAttributeAsync<SampleAttribute>();

        result.Should().ContainSingle().Which.Type.Should().Be(typeof(DecoratedType));
    }

    [Fact]
    public async Task GetTypesWithAttributeAsync_ReturnsEmptyWhenNoMatches()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;
        var result = await assembly.GetTypesWithAttributeAsync<NotUsedAttribute>();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTypesWithAttributeAsync_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Func<Task> action = () => assembly.GetTypesWithAttributeAsync<SampleAttribute>();
        await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("assembly");
    }

    [Fact]
    public void GetResourceStream_ReturnsResourceStream()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;
        using var stream = assembly.GetResourceStream("Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt");

        using var reader = new StreamReader(stream);
        reader.ReadToEnd().Should().Contain("first resource content");
    }

    [Fact]
    public void GetResourceStream_ThrowsForMissingResource()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;
        Action action = () => assembly.GetResourceStream("missing_resource");
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void GetResourceStream_ThrowsForMultipleMatches()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;
        var name = "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt|Plugin.BaseTypeExtensions.Tests.Resources.SecondResource.txt";
        Action action = () => assembly.GetResourceStream(name);
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void GetResourceStream_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Action action = () => assembly.GetResourceStream("resource");
        action.Should().Throw<ArgumentNullException>().WithParameterName("assembly");
    }

    [Fact]
    public void GetResourceStream_ThrowsOnNullResourceName()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;
        Action action = () => assembly.GetResourceStream(null!);
        action.Should().Throw<ArgumentNullException>().WithParameterName("manifestResourceName");
    }
}
