# EnumExtensions

Provides comprehensive enum manipulation and flag operations.

## Overview

The `EnumExtensions` class offers powerful enum operations including flag manipulation, description extraction, and enum introspection utilities.

## API Reference

<xref:Plugin.BaseTypeExtensions.EnumExtensions>

## Core Methods

### Flag Operations

#### `SetFlag<T>(T flags, T flag)` where T : struct, Enum
Sets a specific flag in a flags enum.

```csharp
[Flags]
public enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}

var permissions = FilePermissions.None;
permissions = permissions.SetFlag(FilePermissions.Read);
permissions = permissions.SetFlag(FilePermissions.Write);
// Result: FilePermissions.Read | FilePermissions.Write
```

#### `UnsetFlag<T>(T flags, T flag)` where T : struct, Enum
Removes a specific flag from a flags enum.

```csharp
var permissions = FilePermissions.Read | FilePermissions.Write | FilePermissions.Execute;
permissions = permissions.UnsetFlag(FilePermissions.Write);
// Result: FilePermissions.Read | FilePermissions.Execute
```

#### `TweakFlag<T>(T flags, T flag, bool setFlag)` where T : struct, Enum
Sets or unsets a flag based on a boolean condition.

```csharp
var permissions = FilePermissions.Read;
bool allowWrite = GetUserWritePermission();

permissions = permissions.TweakFlag(FilePermissions.Write, allowWrite);
// Sets Write flag if allowWrite is true, otherwise removes it
```

#### `HasFlag<T>(T flags, T flag)` where T : struct, Enum
Checks if a specific flag is set.

```csharp
var permissions = FilePermissions.Read | FilePermissions.Write;
bool canRead = permissions.HasFlag(FilePermissions.Read); // true
bool canExecute = permissions.HasFlag(FilePermissions.Execute); // false
```

### Enum Introspection

#### `GetDescription<T>(T enumValue)` where T : struct, Enum
Gets the description from DescriptionAttribute or returns the enum name.

```csharp
public enum Priority
{
    [Description("Low Priority")]
    Low,
    [Description("Normal Priority")]
    Normal,
    [Description("High Priority")]
    High
}

var description = Priority.High.GetDescription(); // "High Priority"
```

#### `AllAsArray<T>()` where T : struct, Enum
Gets all values of an enum as an array.

```csharp
var allPriorities = EnumExtensions.AllAsArray<Priority>();
// Result: [Priority.Low, Priority.Normal, Priority.High]

var allPermissions = EnumExtensions.AllAsArray<FilePermissions>();
// Result: [FilePermissions.None, FilePermissions.Read, FilePermissions.Write, FilePermissions.Execute]
```

## Advanced Operations

### Enum Validation

#### `IsValid<T>(T enumValue)` where T : struct, Enum
Checks if an enum value is defined.

```csharp
var validPriority = Priority.High;
var invalidPriority = (Priority)999;

bool isValid1 = validPriority.IsValid(); // true
bool isValid2 = invalidPriority.IsValid(); // false
```

### Conversion Operations

#### `ToInt<T>(T enumValue)` where T : struct, Enum
Converts enum value to its underlying integer value.

```csharp
var priority = Priority.High;
int priorityValue = priority.ToInt(); // Gets the underlying int value
```

#### `FromInt<T>(int value)` where T : struct, Enum
Converts integer value to enum.

```csharp
var priority = EnumExtensions.FromInt<Priority>(1); // Priority.Normal (if 1 is Normal's value)
```

## Practical Examples

### Permission Management System

```csharp
public class UserPermissionManager
{
    public FilePermissions CalculatePermissions(User user, FileInfo file)
    {
        var permissions = FilePermissions.None;

        // Set permissions based on user role
        permissions = permissions.TweakFlag(FilePermissions.Read, user.CanRead(file));
        permissions = permissions.TweakFlag(FilePermissions.Write, user.CanWrite(file));
        permissions = permissions.TweakFlag(FilePermissions.Execute, user.CanExecute(file));

        return permissions;
    }

    public string GetPermissionDescription(FilePermissions permissions)
    {
        var descriptions = new List<string>();

        foreach (var permission in EnumExtensions.AllAsArray<FilePermissions>())
        {
            if (permission != FilePermissions.None && permissions.HasFlag(permission))
            {
                descriptions.Add(permission.GetDescription());
            }
        }

        return string.Join(", ", descriptions);
    }
}
```

### State Machine Implementation

```csharp
public enum WorkflowState
{
    [Description("Draft")]
    Draft = 1,
    [Description("Under Review")]
    Review = 2,
    [Description("Approved")]
    Approved = 4,
    [Description("Published")]
    Published = 8,
    [Description("Archived")]
    Archived = 16
}

public class WorkflowManager
{
    public bool CanTransition(WorkflowState from, WorkflowState to)
    {
        // Define valid transitions
        var validTransitions = new Dictionary<WorkflowState, WorkflowState[]>
        {
            [WorkflowState.Draft] = new[] { WorkflowState.Review, WorkflowState.Archived },
            [WorkflowState.Review] = new[] { WorkflowState.Draft, WorkflowState.Approved, WorkflowState.Archived },
            [WorkflowState.Approved] = new[] { WorkflowState.Published, WorkflowState.Archived },
            [WorkflowState.Published] = new[] { WorkflowState.Archived }
        };

        return validTransitions.TryGetValue(from, out var allowedStates) &&
               allowedStates.Contains(to);
    }

    public string GetStateDescription(WorkflowState state)
    {
        return state.GetDescription();
    }

    public WorkflowState[] GetAllStates()
    {
        return EnumExtensions.AllAsArray<WorkflowState>();
    }
}
```

## Performance Considerations

- Flag operations use efficient bitwise operations
- Uses `MethodImpl(MethodImplOptions.AggressiveInlining)` for performance
- Enum validation uses cached reflection for better performance
- Description lookup is optimized with attribute caching

## Thread Safety

- All extension methods are thread-safe
- Enum values are immutable
- Safe for concurrent usage across multiple threads

## Best Practices

1. **Use Flags Attribute**: Apply `[Flags]` attribute to enums intended for bitwise operations
2. **Power of 2 Values**: Use powers of 2 for flag enum values (1, 2, 4, 8, etc.)
3. **Validation**: Always validate enum values from external sources
4. **Descriptions**: Use `DescriptionAttribute` for user-friendly enum descriptions
5. **Performance**: Cache enum arrays when used frequently in loops
