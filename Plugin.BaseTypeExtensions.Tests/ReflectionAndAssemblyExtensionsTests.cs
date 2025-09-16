using System.Reflection;
using Plugin.BaseTypeExtensions;

using AssemblyExtensions = Plugin.BaseTypeExtensions.AssemblyExtensions;

public class ReflectionAndAssemblyExtensionsTests
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
    public void GetTypesWithAttribute_FindsDecoratedTypes()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        var result = assembly.GetTypesWithAttribute<SampleAttribute>();

        result.Should().ContainSingle().Which.Type.Should().Be(typeof(DecoratedType));
    }

    [Fact]
    public async Task GetTypesWithAttributeAsync_FindsDecoratedTypes()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        var result = await assembly.GetTypesWithAttributeAsync<SampleAttribute>();

        result.Should().ContainSingle().Which.Type.Should().Be(typeof(DecoratedType));
    }

    [Fact]
    public void GetResourceStream_ReturnsResourceStream()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        using var stream = assembly.GetResourceStream("Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt");

        using var reader = new StreamReader(stream);
        reader.ReadToEnd().Should().Contain("first resource content");
    }

    [Fact]
    public void GetResourceStream_ThrowsForMissingResource()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Action action = () => assembly.GetResourceStream("missing_resource");
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void GetResourceStream_ThrowsForMultipleMatches()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        var name = "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt|Plugin.BaseTypeExtensions.Tests.Resources.SecondResource.txt";
        Action action = () => assembly.GetResourceStream(name);
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void MoveManifestResourceToDirectory_WritesFile()
    {
        var directory = CreateTempDirectory();
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = assembly.MoveManifestResourceToDirectory(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory,
                filename: "resource.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Overwrite);

            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void MoveManifestResourceToDirectory_SkipReturnsExistingFile()
    {
        var directory = CreateTempDirectory();
        try
        {
            var targetFile = Path.Combine(directory, "resource.txt");
            File.WriteAllText(targetFile, "existing");

            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = assembly.MoveManifestResourceToDirectory(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory,
                filename: "resource.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Skip);

            result.FullName.Should().Be(targetFile);
            File.ReadAllText(result.FullName).Should().Be("existing");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void MoveManifestResourceToDirectory_RenameCreatesUniqueFile()
    {
        var directory = CreateTempDirectory();
        try
        {
            var targetFile = Path.Combine(directory, "resource.txt");
            File.WriteAllText(targetFile, "existing");

            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = assembly.MoveManifestResourceToDirectory(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory,
                filename: "resource.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Rename);

            result.Name.Should().Be("resource_1.txt");
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void MoveManifestResourceToDirectory_FailThrows()
    {
        var directory = CreateTempDirectory();
        try
        {
            var targetFile = Path.Combine(directory, "resource.txt");
            File.WriteAllText(targetFile, "existing");

            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            Action action = () => assembly.MoveManifestResourceToDirectory(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory,
                filename: "resource.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Fail);

            action.Should().Throw<IOException>();
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_WritesFile()
    {
        var directory = CreateTempDirectory();
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = await assembly.MoveManifestResourceToDirectoryAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.SecondResource.txt",
                directory,
                filename: "resource.txt");

            File.ReadAllText(result.FullName).Should().Contain("second resource content");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_HonorsCancellation()
    {
        var directory = CreateTempDirectory();
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Func<Task> action = () => assembly.MoveManifestResourceToDirectoryAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.SecondResource.txt",
                directory,
                filename: "resource.txt",
                cancellationToken: cts.Token);

            await action.Should().ThrowAsync<OperationCanceledException>();
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static string CreateTempDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }
}
