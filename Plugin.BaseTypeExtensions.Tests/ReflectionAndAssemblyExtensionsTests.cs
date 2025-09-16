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

    [Fact]
    public void MoveManifestResourceToCache_WritesFileToCache()
    {
        var originalCacheDirectory = AssemblyExtensions.CacheDirectory;
        var tempCacheDirectory = CreateTempDirectory();
        try
        {
            AssemblyExtensions.CacheDirectory = tempCacheDirectory;
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;

            var result = assembly.MoveManifestResourceToCache(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                filename: "cached_resource.txt");

            result.DirectoryName.Should().Be(tempCacheDirectory);
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            AssemblyExtensions.CacheDirectory = originalCacheDirectory;
            Directory.Delete(tempCacheDirectory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToCacheAsync_WritesFileToCache()
    {
        var originalCacheDirectory = AssemblyExtensions.CacheDirectory;
        var tempCacheDirectory = CreateTempDirectory();
        try
        {
            AssemblyExtensions.CacheDirectory = tempCacheDirectory;
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;

            var result = await assembly.MoveManifestResourceToCacheAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.SecondResource.txt",
                filename: "cached_async_resource.txt");

            result.DirectoryName.Should().Be(tempCacheDirectory);
            File.ReadAllText(result.FullName).Should().Contain("second resource content");
        }
        finally
        {
            AssemblyExtensions.CacheDirectory = originalCacheDirectory;
            Directory.Delete(tempCacheDirectory, recursive: true);
        }
    }

    [Fact]
    public void MoveManifestResourceToCache_GeneratesRandomFilename()
    {
        var originalCacheDirectory = AssemblyExtensions.CacheDirectory;
        var tempCacheDirectory = CreateTempDirectory();
        try
        {
            AssemblyExtensions.CacheDirectory = tempCacheDirectory;
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;

            var result = assembly.MoveManifestResourceToCache(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt");

            result.DirectoryName.Should().Be(tempCacheDirectory);
            result.Name.Should().NotBeNullOrEmpty();
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            AssemblyExtensions.CacheDirectory = originalCacheDirectory;
            Directory.Delete(tempCacheDirectory, recursive: true);
        }
    }

    #region Error Condition Tests

    [Fact]
    public void MoveManifestResourceToDirectory_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Action action = () => assembly.MoveManifestResourceToDirectory("resource", "/tmp");
        action.Should().Throw<ArgumentNullException>().WithParameterName("assembly");
    }

    [Fact]
    public void MoveManifestResourceToDirectory_ThrowsOnNullResourceName()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Action action = () => assembly.MoveManifestResourceToDirectory(null!, "/tmp");
        action.Should().Throw<ArgumentNullException>().WithParameterName("manifestResourceName");
    }

    [Fact]
    public void MoveManifestResourceToDirectory_ThrowsOnNullTargetDirectory()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Action action = () => assembly.MoveManifestResourceToDirectory("resource", null!);
        action.Should().Throw<ArgumentNullException>().WithParameterName("targetDirectory");
    }

    [Fact]
    public void MoveManifestResourceToDirectory_ThrowsOnEmptyResourceName()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Action action = () => assembly.MoveManifestResourceToDirectory("", "/tmp");
        action.Should().Throw<ArgumentException>().WithParameterName("manifestResourceName");
    }

    [Fact]
    public void MoveManifestResourceToDirectory_ThrowsOnWhitespaceResourceName()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Action action = () => assembly.MoveManifestResourceToDirectory("   ", "/tmp");
        action.Should().Throw<ArgumentException>().WithParameterName("manifestResourceName");
    }

    [Fact]
    public void MoveManifestResourceToDirectory_ThrowsOnNonExistentResource()
    {
        var directory = CreateTempDirectory();
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            Action action = () => assembly.MoveManifestResourceToDirectory("NonExistent.Resource", directory);
            action.Should().Throw<FileNotFoundException>().WithMessage("*Couldn't find resource named*");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void MoveManifestResourceToDirectory_CreatesDirectoryIfNotExists()
    {
        var parentDirectory = CreateTempDirectory();
        var targetDirectory = Path.Combine(parentDirectory, "subdir", "nested");
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = assembly.MoveManifestResourceToDirectory(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                targetDirectory,
                filename: "resource.txt");

            Directory.Exists(targetDirectory).Should().BeTrue();
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(parentDirectory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Func<Task> action = () => assembly.MoveManifestResourceToDirectoryAsync("resource", "/tmp");
        await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("assembly");
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_ThrowsOnNullResourceName()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Func<Task> action = () => assembly.MoveManifestResourceToDirectoryAsync(null!, "/tmp");
        await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("manifestResourceName");
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_ThrowsOnNullTargetDirectory()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Func<Task> action = () => assembly.MoveManifestResourceToDirectoryAsync("resource", null!);
        await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("targetDirectory");
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_ThrowsOnEmptyResourceName()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Func<Task> action = () => assembly.MoveManifestResourceToDirectoryAsync("", "/tmp");
        await action.Should().ThrowAsync<ArgumentException>().WithParameterName("manifestResourceName");
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_SkipReturnsExistingFile()
    {
        var directory = CreateTempDirectory();
        try
        {
            var targetFile = Path.Combine(directory, "async_resource.txt");
            File.WriteAllText(targetFile, "existing async");

            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = await assembly.MoveManifestResourceToDirectoryAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory,
                filename: "async_resource.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Skip);

            result.FullName.Should().Be(targetFile);
            File.ReadAllText(result.FullName).Should().Be("existing async");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_RenameCreatesUniqueFile()
    {
        var directory = CreateTempDirectory();
        try
        {
            var targetFile = Path.Combine(directory, "async_resource.txt");
            File.WriteAllText(targetFile, "existing async");

            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = await assembly.MoveManifestResourceToDirectoryAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory,
                filename: "async_resource.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Rename);

            result.Name.Should().Be("async_resource_1.txt");
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_FailThrows()
    {
        var directory = CreateTempDirectory();
        try
        {
            var targetFile = Path.Combine(directory, "async_resource.txt");
            File.WriteAllText(targetFile, "existing async");

            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            Func<Task> action = () => assembly.MoveManifestResourceToDirectoryAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory,
                filename: "async_resource.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Fail);

            await action.Should().ThrowAsync<IOException>();
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_OverwriteReplacesFile()
    {
        var directory = CreateTempDirectory();
        try
        {
            var targetFile = Path.Combine(directory, "async_resource.txt");
            File.WriteAllText(targetFile, "existing async");

            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = await assembly.MoveManifestResourceToDirectoryAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory,
                filename: "async_resource.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Overwrite);

            result.FullName.Should().Be(targetFile);
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void MoveManifestResourceToDirectory_OverwriteReplacesFile()
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
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Overwrite);

            result.FullName.Should().Be(targetFile);
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void MoveManifestResourceToDirectory_GeneratesRandomFilename()
    {
        var directory = CreateTempDirectory();
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = assembly.MoveManifestResourceToDirectory(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory);

            result.Name.Should().NotBeNullOrEmpty();
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void MoveManifestResourceToCache_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Action action = () => assembly.MoveManifestResourceToCache("resource");
        action.Should().Throw<ArgumentNullException>().WithParameterName("assembly");
    }

    [Fact]
    public async Task MoveManifestResourceToCacheAsync_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Func<Task> action = () => assembly.MoveManifestResourceToCacheAsync("resource");
        await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("assembly");
    }

    [Fact]
    public async Task MoveManifestResourceToCacheAsync_HonorsCancellation()
    {
        var originalCacheDirectory = AssemblyExtensions.CacheDirectory;
        var tempCacheDirectory = CreateTempDirectory();
        try
        {
            AssemblyExtensions.CacheDirectory = tempCacheDirectory;
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Func<Task> action = () => assembly.MoveManifestResourceToCacheAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.SecondResource.txt",
                cancellationToken: cts.Token);

            await action.Should().ThrowAsync<OperationCanceledException>();
        }
        finally
        {
            AssemblyExtensions.CacheDirectory = originalCacheDirectory;
            Directory.Delete(tempCacheDirectory, recursive: true);
        }
    }

    [Fact]
    public void GetAssembly_ThrowsOnNullInstance()
    {
        object instance = null!;
        Action action = () => instance.GetAssembly();
        action.Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void GetTypesWithAttribute_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Action action = () => assembly.GetTypesWithAttribute<SampleAttribute>();
        action.Should().Throw<ArgumentNullException>().WithParameterName("assembly");
    }

    [Fact]
    public async Task GetTypesWithAttributeAsync_ThrowsOnNullAssembly()
    {
        Assembly assembly = null!;
        Func<Task> action = () => assembly.GetTypesWithAttributeAsync<SampleAttribute>();
        await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("assembly");
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
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        Action action = () => assembly.GetResourceStream(null!);
        action.Should().Throw<ArgumentNullException>().WithParameterName("manifestResourceName");
    }

    [Fact]
    public void CacheDirectory_Property_CanBeAccessedAndModified()
    {
        var originalValue = AssemblyExtensions.CacheDirectory;
        try
        {
            // Test getter
            var currentValue = AssemblyExtensions.CacheDirectory;
            currentValue.Should().NotBeNull();

            // Test setter
            var testPath = "/tmp/test";
            AssemblyExtensions.CacheDirectory = testPath;
            AssemblyExtensions.CacheDirectory.Should().Be(testPath);
        }
        finally
        {
            AssemblyExtensions.CacheDirectory = originalValue;
        }
    }

    [Fact]
    public void GetTypesWithAttribute_ReturnsEmptyWhenNoMatches()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        var result = assembly.GetTypesWithAttribute<ObsoleteAttribute>();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTypesWithAttributeAsync_ReturnsEmptyWhenNoMatches()
    {
        var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
        var result = await assembly.GetTypesWithAttributeAsync<ObsoleteAttribute>();
        result.Should().BeEmpty();
    }

    [Fact]
    public void MoveManifestResourceToCache_WithAllFileAlreadyExistsBehaviors()
    {
        var originalCacheDirectory = AssemblyExtensions.CacheDirectory;
        var tempCacheDirectory = CreateTempDirectory();
        try
        {
            AssemblyExtensions.CacheDirectory = tempCacheDirectory;
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;

            // Test Skip behavior
            var existingFile = Path.Combine(tempCacheDirectory, "test_skip.txt");
            File.WriteAllText(existingFile, "existing content");

            var result = assembly.MoveManifestResourceToCache(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                filename: "test_skip.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Skip);

            File.ReadAllText(result.FullName).Should().Be("existing content");

            // Test Rename behavior
            var result2 = assembly.MoveManifestResourceToCache(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                filename: "test_skip.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Rename);

            result2.Name.Should().Be("test_skip_1.txt");

            // Test Overwrite behavior
            var result3 = assembly.MoveManifestResourceToCache(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                filename: "test_skip.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Overwrite);

            File.ReadAllText(result3.FullName).Should().Contain("first resource content");
        }
        finally
        {
            AssemblyExtensions.CacheDirectory = originalCacheDirectory;
            Directory.Delete(tempCacheDirectory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToCacheAsync_WithAllFileAlreadyExistsBehaviors()
    {
        var originalCacheDirectory = AssemblyExtensions.CacheDirectory;
        var tempCacheDirectory = CreateTempDirectory();
        try
        {
            AssemblyExtensions.CacheDirectory = tempCacheDirectory;
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;

            // Test Skip behavior
            var existingFile = Path.Combine(tempCacheDirectory, "test_async_skip.txt");
            File.WriteAllText(existingFile, "existing async content");

            var result = await assembly.MoveManifestResourceToCacheAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                filename: "test_async_skip.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Skip);

            File.ReadAllText(result.FullName).Should().Be("existing async content");

            // Test Rename behavior
            var result2 = await assembly.MoveManifestResourceToCacheAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                filename: "test_async_skip.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Rename);

            result2.Name.Should().Be("test_async_skip_1.txt");

            // Test Overwrite behavior
            var result3 = await assembly.MoveManifestResourceToCacheAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                filename: "test_async_skip.txt",
                fileAlreadyExistsBehavior: AssemblyExtensions.FileAlreadyExistsBehavior.Overwrite);

            File.ReadAllText(result3.FullName).Should().Contain("first resource content");
        }
        finally
        {
            AssemblyExtensions.CacheDirectory = originalCacheDirectory;
            Directory.Delete(tempCacheDirectory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_ThrowsOnNonExistentResource()
    {
        var directory = CreateTempDirectory();
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            Func<Task> action = () => assembly.MoveManifestResourceToDirectoryAsync("NonExistent.Resource", directory);
            await action.Should().ThrowAsync<FileNotFoundException>().WithMessage("*Couldn't find resource named*");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_CreatesDirectoryIfNotExists()
    {
        var parentDirectory = CreateTempDirectory();
        var targetDirectory = Path.Combine(parentDirectory, "async_subdir", "nested");
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = await assembly.MoveManifestResourceToDirectoryAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                targetDirectory,
                filename: "async_resource.txt");

            Directory.Exists(targetDirectory).Should().BeTrue();
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(parentDirectory, recursive: true);
        }
    }

    [Fact]
    public async Task MoveManifestResourceToDirectoryAsync_GeneratesRandomFilename()
    {
        var directory = CreateTempDirectory();
        try
        {
            var assembly = typeof(ReflectionAndAssemblyExtensionsTests).Assembly;
            var result = await assembly.MoveManifestResourceToDirectoryAsync(
                "Plugin.BaseTypeExtensions.Tests.Resources.FirstResource.txt",
                directory);

            result.Name.Should().NotBeNullOrEmpty();
            File.ReadAllText(result.FullName).Should().Contain("first resource content");
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    #endregion

    private static string CreateTempDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }
}
