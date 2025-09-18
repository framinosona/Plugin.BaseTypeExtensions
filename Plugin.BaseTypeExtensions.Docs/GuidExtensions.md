# GuidExtensions

Provides GUID manipulation and validation utilities.

## Overview

The `GuidExtensions` class offers utilities for GUID operations including validation, formatting, and sequential GUID generation.

## API Reference

<xref:Plugin.BaseTypeExtensions.GuidExtensions>

## Core Methods

### Validation Operations

#### `IsEmpty(Guid guid)`
Checks if a GUID is empty (all zeros).

```csharp
var emptyGuid = Guid.Empty;
var isEmptyGuid = emptyGuid.IsEmpty(); // true

var newGuid = Guid.NewGuid();
var isNewEmpty = newGuid.IsEmpty(); // false
```

#### `IsValid(string guidString)`
Validates if a string represents a valid GUID.

```csharp
bool valid1 = "123e4567-e89b-12d3-a456-426614174000".IsValidGuid(); // true
bool valid2 = "invalid-guid-string".IsValidGuid(); // false
```

### Sequential GUID Operations

#### `GetPlusOne(Guid guid)`
Generates the next sequential GUID (useful for database optimization).

```csharp
var baseGuid = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");
var nextGuid = baseGuid.GetPlusOne();
// Results in the next sequential GUID
```

## Practical Examples

### Database Entity Management

```csharp
public class EntityIdGenerator
{
    private Guid _lastGeneratedId = Guid.Empty;

    public Guid GenerateSequentialId()
    {
        if (_lastGeneratedId.IsEmpty())
        {
            _lastGeneratedId = Guid.NewGuid();
        }
        else
        {
            _lastGeneratedId = _lastGeneratedId.GetPlusOne();
        }

        return _lastGeneratedId;
    }
}
```

### Input Validation

```csharp
public class GuidValidator
{
    public ValidationResult ValidateGuidInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new ValidationResult("GUID cannot be empty");
        }

        if (!input.IsValidGuid())
        {
            return new ValidationResult("Invalid GUID format");
        }

        var guid = Guid.Parse(input);
        if (guid.IsEmpty())
        {
            return new ValidationResult("GUID cannot be empty");
        }

        return ValidationResult.Success;
    }
}
```

## Best Practices

1. **Empty Validation**: Always check for empty GUIDs in business logic
2. **String Validation**: Validate GUID strings before parsing
3. **Sequential GUIDs**: Use for database performance optimization
4. **Immutability**: Remember that GUIDs are immutable value types
