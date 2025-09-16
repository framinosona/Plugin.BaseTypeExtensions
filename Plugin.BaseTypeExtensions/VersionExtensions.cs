namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="Version"/> manipulation.
/// </summary>
public static class VersionExtensions
{
    /// <summary>
    /// Returns a <see cref="Version"/> with all fields set (Major, Minor, Build, Revision), replacing unset fields (-1) with 0.
    /// </summary>
    /// <param name="version">The input version.</param>
    /// <returns>A full <see cref="Version"/> with unset fields replaced by 0, or null if <paramref name="version"/> is null.</returns>
    public static Version? ToFullVersion(this Version? version) //00
    {
        if (version == null)
        {
            return null;
        }

        if (version is { Major: >= 0, Minor: >= 0, Build: >= 0, Revision: >= 0 })
        {
            return version; //already a full version
        }

        return new Version(
                           major: version.Major,
                           minor: Math.Max(version.Minor, 0),
                           build: Math.Max(version.Build, 0), //       this is the whole point   if any of these fields is unset it will
                           revision: Math.Max(version.Revision, 0) //  be reported as -1 but we want to ensure that they get turned to 0 instead
                          );

        //00  we want to ensure that new Version(1, 2) will be converted to Version(1, 2, 0, 0) instead of Version(1, 2, -1, -1)
        //    which what the first constructor does internally leading to bizarre results when comparing versions such as for
        //    example   Version(1, 2, 0, 0) > Version(1, 2)  which is nuts and leads to insane bugs when evaluating fw versions
    }
}
