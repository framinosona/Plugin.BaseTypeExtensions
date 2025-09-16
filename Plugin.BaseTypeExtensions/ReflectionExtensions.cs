using System.Collections.Concurrent;
using System.Reflection;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for advanced reflection operations, including event field manipulation,
/// hierarchy traversal, and assembly operations.
/// </summary>
public static class ReflectionExtensions
{
    #region Assembly Operations

    /// <summary>
    /// Gets the assembly of the specified object instance.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="instance">The object instance.</param>
    /// <returns>The assembly containing the object's type.</returns>
    public static Assembly GetAssembly<T>(this T instance) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);
        return instance.GetType().Assembly;
    }

    /// <summary>
    /// Gets all types in the specified assembly that have the specified attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type.</typeparam>
    /// <param name="assembly">The assembly to search.</param>
    /// <returns>An enumerable of tuples containing the attribute and the type.</returns>
    public static IEnumerable<(TAttribute Attribute, Type Type)> GetTypesWithAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var results = new ConcurrentBag<(TAttribute, Type)>();

        Parallel.ForEach(assembly.GetTypes(), type =>
        {
            var attribute = type.GetCustomAttribute<TAttribute>(inherit: true);
            if (attribute != null)
            {
                results.Add((attribute, type));
            }
        });

        return results.ToList();
    }

    /// <summary>
    /// Asynchronously gets all types in the specified assembly that have the specified attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type.</typeparam>
    /// <param name="assembly">The assembly to search.</param>
    /// <returns>A task representing the asynchronous operation, with a list of tuples containing the attribute and the type.</returns>
    public static async Task<List<(TAttribute Attribute, Type Type)>> GetTypesWithAttributeAsync<TAttribute>(this Assembly assembly) where TAttribute : Attribute
    {
        ArgumentNullException.ThrowIfNull(assembly);

        return await Task.Run(() =>
        {
            var results = new ConcurrentBag<(TAttribute, Type)>();

            Parallel.ForEach(assembly.GetTypes(), type =>
            {
                var attribute = type.GetCustomAttribute<TAttribute>(inherit: true);
                if (attribute != null)
                {
                    results.Add((attribute, type));
                }
            });

            return results.ToList();
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a resource stream from the specified assembly by resource name.
    /// </summary>
    /// <param name="assembly">The assembly to search.</param>
    /// <param name="manifestResourceName">The name of the resource to find.</param>
    /// <returns>A stream for the resource.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="assembly"/> or <paramref name="manifestResourceName"/> is null.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the resource is not found or multiple matches are found.</exception>
    public static Stream GetResourceStream(this Assembly assembly, string manifestResourceName)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(manifestResourceName);

        var manifestResourceNameUpperInvariant = manifestResourceName.ToUpperInvariant();

        var matchingNames = assembly
                           .GetManifestResourceNames()
                           .Where(name => manifestResourceNameUpperInvariant.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                           .ToList();

        if (matchingNames.Count == 0)
        {
            throw new FileNotFoundException($"Couldn't find resource named '{manifestResourceName}' in assembly '{assembly.GetName().Name}'.");
        }

        if (matchingNames.Skip(1).Any())
        {
            throw new FileNotFoundException($"Found multiple resources named '{manifestResourceName}' in assembly '{assembly.GetName().Name}' : {string.Join(", ", matchingNames)}");
        }

        var matchingName = matchingNames.Single();

        var output = assembly.GetManifestResourceStream(matchingName);
        return output ?? throw new FileNotFoundException($"Couldn't find resource named '{manifestResourceName}' in assembly '{assembly.GetName().Name}'.");
    }

    #endregion
}
