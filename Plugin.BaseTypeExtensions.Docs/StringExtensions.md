# 🔤 StringExtensions

Provides comprehensive string manipulation, validation, transformation, and utility methods.

## Overview

The `StringExtensions` class offers 40+ extension methods for string operations including validation, case conversion, encoding, manipulation, analysis, and generation.

## Core Methods

### Basic Utilities

#### `NullIfDud(string? input)`
Returns null if the string is null, empty, or contains only whitespace.

```csharp
string? result = "   ".NullIfDud(); // null
string? result = "hello".NullIfDud(); // "hello"
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

#### `IsInteger(string? input)`
Determines if string represents an integer value.

```csharp
bool isInt = "123".IsInteger(); // true
bool isInt = "123.45".IsInteger(); // false
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

#### `PadBoth(string? input, int totalWidth, char paddingChar = ' ')`
Centers string within specified width.

```csharp
string centered = "hi".PadBoth(6); // "  hi  "
string custom = "hi".PadBoth(6, '*'); // "**hi**"
```

### Content Manipulation

#### `RemoveWhitespace(string? input)`
Removes all whitespace characters.

```csharp
string clean = "hello world".RemoveWhitespace(); // "helloworld"
```

#### `CollapseWhitespace(string? input)`
Collapses multiple whitespace into single spaces.

```csharp
string clean = "hello    world".CollapseWhitespace(); // "hello world"
```

#### `Reverse(string? input)`
Reverses the string.

```csharp
string reversed = "hello".Reverse(); // "olleh"
```

#### Character Filtering

```csharp
// Remove digits
string noDigits = "abc123def".RemoveDigits(); // "abcdef"

// Keep only digits
string onlyDigits = "abc123def".OnlyDigits(); // "123"

// Keep only letters
string onlyLetters = "abc123def".OnlyLetters(); // "abcdef"
```

## String Analysis

### Counting Operations

#### `CountOccurrences(string? input, char/string target)`
Counts occurrences of character or substring.

```csharp
int count = "hello world".CountOccurrences('l'); // 3
int count = "test test".CountOccurrences("test"); // 2
```

#### `WordCount(string? input)`
Counts words separated by whitespace.

```csharp
int words = "hello world test".WordCount(); // 3
```

#### `LineCount(string? input)`
Counts lines (newline characters + 1).

```csharp
int lines = "line1\nline2\nline3".LineCount(); // 3
```

## String Extraction

### Substring Operations

#### `Between(string? input, string start, string end)`
Extracts substring between two markers.

```csharp
string content = "hello[world]test".Between("[", "]"); // "world"
```

#### `After(string? input, string marker)`
Extracts substring after specified marker.

```csharp
string after = "hello:world".After(":"); // "world"
```

#### `Before(string? input, string marker)`
Extracts substring before specified marker.

```csharp
string before = "hello:world".Before(":"); // "hello"
```

#### `ExtractNumbers(string? input)`
Extracts all numeric characters.

```csharp
string numbers = "abc123def456".ExtractNumbers(); // "123456"
```

## String Splitting & Joining

### Enhanced Split Operations

#### `SplitAndTrim(string? input, params char[] separators)`
Splits string and trims each result.

```csharp
string[] parts = "hello, world , test".SplitAndTrim(',');
// ["hello", "world", "test"]
```

#### `SplitAndRemoveEmpty(string? input, params char[] separators)`
Splits string and removes empty entries.

```csharp
string[] parts = "hello,,world".SplitAndRemoveEmpty(',');
// ["hello", "world"]
```

#### `JoinWith(IEnumerable<string> values, string separator)`
Joins string collection with separator.

```csharp
string joined = new[] {"a", "b", "c"}.JoinWith(", "); // "a, b, c"
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

## String Comparison

### Case-Insensitive Operations

```csharp
// Case-insensitive comparisons
bool contains = "Hello World".ContainsIgnoreCase("WORLD"); // true
bool starts = "Hello".StartsWithIgnoreCase("HEL"); // true
bool ends = "World".EndsWithIgnoreCase("RLD"); // true
bool equals = "Test".EqualsIgnoreCase("TEST"); // true
```

## String Generation

### Repeat Operations

#### `Repeat(string? input, int count)`
Repeats string specified number of times.

```csharp
string repeated = "abc".Repeat(3); // "abcabcabc"
```

#### `RandomString(int length, string allowedChars = "...")`
Generates random string of specified length.

```csharp
string random = StringExtensions.RandomString(10); // Random 10-char string
string custom = StringExtensions.RandomString(5, "abc"); // Random from "abc"
```

## Performance Notes

- Most methods use `MethodImpl(MethodImplOptions.AggressiveInlining)` for performance
- Uses `StringBuilder` for string building operations
- Optimized null checking and early returns
- Minimal memory allocations in hot paths

## Thread Safety

- All extension methods are thread-safe (stateless)
- `RandomString` uses `new Random()` per call (not thread-safe across calls)
- No shared state between method calls

## Examples

### Input Validation Pipeline

```csharp
string ProcessInput(string? input)
{
    return input
        .NullIfDud() ?? "default"
        .RemoveSpecialCharacters()
        .CollapseWhitespace()
        .Truncate(50);
}
```

### Data Transformation

```csharp
// Convert user input to system format
string systemName = userInput
    .RemoveSpecialCharacters()
    .ToSnakeCase()
    .ToLowerInvariant();

// Generate display name
string displayName = systemName
    .Replace("_", " ")
    .ToTitleCase();
```

### Content Analysis

```csharp
// Analyze text content
var stats = new {
    Words = content.WordCount(),
    Lines = content.LineCount(),
    Characters = content?.Length ?? 0,
    HasEmail = content.IsValidEmail(),
    HasUrl = content.IsValidUrl()
};
```
