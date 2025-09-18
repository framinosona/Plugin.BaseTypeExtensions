# VersionExtensions

Provides version comparison and formatting utilities.

## Overview

The `VersionExtensions` class offers utilities for Version objects including enhanced comparison, formatting, and version analysis operations.

## API Reference

<xref:Plugin.BaseTypeExtensions.VersionExtensions>

## Core Methods

### Comparison Operations

#### `IsNewerThan(Version version, Version other)`
Checks if a version is newer than another version.

```csharp
var current = new Version(2, 1, 0);
var old = new Version(2, 0, 5);
bool isNewer = current.IsNewerThan(old); // true
```

#### `IsOlderThan(Version version, Version other)`
Checks if a version is older than another version.

```csharp
var legacy = new Version(1, 5, 0);
var current = new Version(2, 0, 0);
bool isOlder = legacy.IsOlderThan(current); // true
```

### Formatting Operations

#### `ToShortString(Version version)`
Formats version without trailing zeros.

```csharp
var version = new Version(2, 1, 0, 0);
var short = version.ToShortString(); // "2.1" (removes .0.0)
```

## Practical Examples

### Software Update Management

```csharp
public class UpdateManager
{
    public bool IsUpdateAvailable(Version currentVersion, Version latestVersion)
    {
        return latestVersion.IsNewerThan(currentVersion);
    }

    public string GetVersionDisplay(Version version)
    {
        return version.ToShortString();
    }
}
```

## Best Practices

1. **Semantic Versioning**: Follow semantic versioning principles
2. **Null Checks**: Always validate Version objects before comparison
3. **Display Formatting**: Use appropriate formatting for user interfaces
