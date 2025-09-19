# StringExtensions

Provides comprehensive string manipulation, validation, transformation, and utility methods.

## Overview

The `StringExtensions` class offers 40+ extension methods for string operations including validation, case conversion, encoding, manipulation, analysis, and generation. All methods are designed to be null-safe and performance-optimized.

## API Reference

<xref:Plugin.BaseTypeExtensions.StringExtensions>

## Core Methods

### Basic Utilities

#### `NullIfEmptyOrWhiteSpace(string? input)`
Returns null if the string is null, empty, or contains only whitespace.

```csharp
string? result = "   ".NullIfEmptyOrWhiteSpace(); // null
string? result = "hello".NullIfEmptyOrWhiteSpace(); // "hello"
```

#### `RemoveSpecialCharacters(string? input)`
Removes special characters, keeping only letters, digits, whitespace, and hyphens.

```csharp
string clean = "he!!o-w@rld".RemoveSpecialCharacters(); // "heo-wrld"
```

## String Validation

### Email and URL Validation

#### `IsValidEmail(string? input)`
Validates email address format using `MailAddress` parsing.

```csharp
bool valid = "test@example.com".IsValidEmail(); // true
bool invalid = "invalid-email".IsValidEmail(); // false
```

#### `IsValidUrl(string? input)`
Validates URL format using `Uri.IsWellFormedUriString`.

```csharp
bool valid = "https://example.com".IsValidUrl(); // true
bool invalid = "not-a-url".IsValidUrl(); // false
```

### Numeric Validation

#### `IsNumeric(string? input)`
Determines if string represents a numeric value using `double.TryParse`.

```csharp
bool isNum = "123.45".IsNumeric(); // true
bool isNum = "1e10".IsNumeric(); // true (scientific notation)
bool isNum = "abc123".IsNumeric(); // false
```

## String Transformations

### Case Conversion

#### `ToTitleCase(string? input)`
Converts string to Title Case.

```csharp
string title = "hello world".ToTitleCase(); // "Hello World"
```

#### `ToCamelCase(string? input)`
Converts string to camelCase.

```csharp
string camel = "hello world".ToCamelCase(); // "helloWorld"
string camel = "hello_world-test".ToCamelCase(); // "helloWorldTest"
```

#### `ToPascalCase(string? input)`
Converts string to PascalCase.

```csharp
string pascal = "hello world".ToPascalCase(); // "HelloWorld"
```

#### `ToKebabCase(string? input)`
Converts string to kebab-case.

```csharp
string kebab = "Hello World".ToKebabCase(); // "hello-world"
```

#### `ToSnakeCase(string? input)`
Converts string to snake_case.

```csharp
string snake = "Hello World".ToSnakeCase(); // "hello_world"
```

## String Manipulation

### Truncation and Padding

#### `Truncate(string? input, int maxLength, string suffix = "...")`
Truncates string to specified length with optional suffix.

```csharp
string short = "Very long text".Truncate(8); // "Very ..."
string custom = "Long text".Truncate(5, "!"); // "Long!"
```

#### `EllipsisIfTooLong(string? input, int maxLength)`
Shorthand for truncation with ellipsis.

```csharp
string result = "Too long".EllipsisIfTooLong(5); // "To..."
```

## Encoding & Safety

### HTML Operations

#### `EscapeHtml(string? input)` / `UnescapeHtml(string? input)`
HTML encoding and decoding.

```csharp
string escaped = "<script>".EscapeHtml(); // "&lt;script&gt;"
string unescaped = "&lt;test&gt;".UnescapeHtml(); // "<test>"
```

### Base64 Operations

#### `ToBase64(string? input, Encoding? encoding = null)`
Encodes string to Base64.

```csharp
string encoded = "Hello World".ToBase64(); // "SGVsbG8gV29ybGQ="
```

#### `FromBase64(string? input, Encoding? encoding = null)`
Decodes Base64 string.

```csharp
string decoded = "SGVsbG8gV29ybGQ=".FromBase64(); // "Hello World"
```

## Performance Considerations

- Most methods use `MethodImpl(MethodImplOptions.AggressiveInlining)` for performance
- Uses `StringBuilder` for string building operations
- Optimized null checking and early returns
- Minimal memory allocations in hot paths

## Thread Safety

- All extension methods are thread-safe (stateless)
- No shared state between method calls
- Safe for concurrent usage across multiple threads

## Best Practices

1. **Choose Appropriate Methods**: Use `NullIfEmptyOrWhiteSpace()` for clean null handling
2. **Performance**: Chain operations efficiently with early returns
3. **Validation**: Use validation methods for user input processing
4. **Encoding**: Always specify encoding for Base64 operations when needed
5. **Case Conversion**: Use appropriate case for your specific use case
