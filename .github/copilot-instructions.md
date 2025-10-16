# Plugin.BaseTypeExtensions - AI Coding Agent Instructions

## Project Overview
This is a .NET 9 library providing comprehensive extension methods for base types, designed to enhance developer productivity with clean, intuitive, and performance-optimized code. The library offers multiple specialized components:
- **StringExtensions**: Comprehensive string manipulation, validation, transformation, and utility methods
- **NumericExtensions**: Type-safe numeric operations for all `INumber<T>` types using modern .NET constraints
- **EnumerableExtensions**: Advanced LINQ-style operations and collection utilities
- **TaskExtensions**: Advanced async operations and timeout management
- **CancellationTokenExtensions**: Sophisticated cancellation and timeout coordination
- **TimeSpanExtensions**: Comprehensive TimeSpan operations and formatting
- **CollectionExtensions**: List, Dictionary, and collection-specific operations
- **ReflectionExtensions**: Advanced reflection utilities and assembly operations
- **UtilityExtensions**: Random, GUID, Version, Byte, URI, Stream, and Exception extensions
- **UtilityClasses**: ComparableTools and NumericRangeTools for complex operations

## Architecture Patterns

### Extension Method Structure
- **One class per base type**: `StringExtensions.cs`, `NumericExtensions.cs`, etc.
- **Static partial classes**: All extension classes are `static partial` allowing future expansion
- **Generic constraints**: Heavy use of modern .NET generic constraints like `INumber<T>`, `IComparable<T>`, `IFloatingPoint<T>`
- **Performance optimization**: Liberal use of `[MethodImpl(MethodImplOptions.AggressiveInlining)]`
- **Thread-safe patterns**: All extension methods are stateless and thread-safe by design

### Method Design Patterns
Every extension method follows consistent patterns:
```csharp
// Basic extension pattern
public static TResult MethodName<T>(this T input, ...parameters)
    where T : IConstraint<T>

// Null-safe variants
public static T? NullIfCondition(this T? input)
public static T OrDefault(this T? input, T defaultValue)

// Performance-critical methods
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static T FastOperation<T>(this T input) where T : INumber<T>
```

### Code Quality Infrastructure
- **Null safety**: Always handle null inputs gracefully, return empty or null as appropriate
- **Documentation**: Every public method has XML documentation with `<summary>`, parameters, returns, and exceptions
- **Naming**: Use descriptive method names like `NullIfEmptyOrWhiteSpace()`, `RemoveSpecialCharacters()`, `WithTimeoutInMs()`
- **Type safety**: Prefer compile-time safety with generic constraints over runtime checks
- **Error handling**: Meaningful exceptions with context and helpful error messages

## Testing Conventions

### Test Structure
- Tests use **xUnit** with **FluentAssertions** (v7.2.0 for licensing compliance)
- Test classes mirror main project structure with 1:1 mapping: `{ClassName}Tests` (e.g., `StringExtensionsTests`)
- Test methods use descriptive names: `Method_Scenario_ExpectedResult`
- **95% minimum code coverage** enforced in CI/CD pipeline
- **386+ comprehensive tests** covering all functionality with extensive edge cases

### Test Patterns
```csharp
// Standard extension method test
[Fact]
public void Clamp_ValueWithinRange_ReturnsOriginalValue()
{
    // Arrange
    int value = 50;

    // Act
    int result = value.Clamp(0, 100);

    // Assert
    result.Should().Be(50);
}

// Null handling test
[Fact]
public void NullIfEmptyOrWhiteSpace_WithNullInput_ReturnsNull()
{
    // Arrange
    string? input = null;

    // Act
    string? result = input.NullIfEmptyOrWhiteSpace();

    // Assert
    result.Should().BeNull();
}

// Thread-safe async test pattern
[Fact]
public async Task WithTimeoutInMs_WithValidTimeout_CompletesSuccessfully()
{
    // Arrange
    var task = Task.FromResult("result");

    // Act
    var result = await task.WithTimeoutInMs(1000);

    // Assert
    result.Should().Be("result");
}

// Error condition testing
[Fact]
public void ToBase64_WithInvalidInput_ThrowsArgumentException()
{
    // Arrange
    string? input = null;

    // Act
    Action act = () => input.ToBase64();

    // Assert
    act.Should().Throw<ArgumentNullException>();
}
```

### Thread-Safe Testing Patterns
- **Deterministic patterns**: Use state-based validation instead of timing-dependent assertions
- **Cancellation testing**: Proper async test patterns for cancellation token operations
- **Timeout testing**: Use short, reliable timeouts for test scenarios
- **Parallel execution**: All tests must be safe for parallel execution

## Build System

### Central Package Management
Uses `Directory.Packages.props` with `ManagePackageVersionsCentrally=true`. All package versions defined centrally.

### MSBuild Configuration
- **Directory.Build.props** - Read BEFORE project file (OS detection, CI/CD, excluded folders)
- **Directory.Build.targets** - Read AFTER project file (packaging, versioning, analyzers)
- **Strict analyzer settings**: `TreatWarningsAsErrors=true`, `AnalysisMode=AllEnabledByDefault`
- **Zero tolerance**: All code must compile without warnings

### Project Structure Variables
Projects use these variables in `.csproj`:
- `$(Project_Name)` - Used for assembly name, package ID, root namespace
- `$(Project_Description)` - Package/assembly description
- `$(Project_Copyright)` - Author/owner information
- `$(Project_Tags)` - NuGet package tags
- `$(Project_Url)` - Repository URL

## Development Workflows

### Build Commands
```bash
# Restore and build
dotnet restore Plugin.BaseTypeExtensions.slnx
dotnet build Plugin.BaseTypeExtensions.slnx -c Release

# Run tests with coverage (95% minimum)
dotnet test Plugin.BaseTypeExtensions.Tests/Plugin.BaseTypeExtensions.Tests.csproj \
  --logger "trx" \
  -p:CollectCoverage=true \
  -p:CoverletOutputFormat=cobertura

# Run specific test class
dotnet test --filter "StringExtensionsTests"

# Generate packages
dotnet build -p:GeneratePackageOnBuild=true -p:Version_Full=1.0.0
```

### Documentation Generation
Uses **DocFX** for comprehensive documentation:
```bash
dotnet docfx metadata Plugin.BaseTypeExtensions.Docs/docfx.json
dotnet docfx build Plugin.BaseTypeExtensions.Docs/docfx.json --output Plugin.BaseTypeExtensions.Docs/site
```

## Documentation Standards

### Documentation Architecture Principles

#### 1:1 Source-to-Documentation Mapping
- **Every extension class must have a corresponding documentation file** in the `Plugin.BaseTypeExtensions.Docs/` directory
- **Naming convention**: `{ClassName}.md` (e.g., `StringExtensions.md`, `TaskExtensions.md`)
- **Organizational structure**: Mirror the source code structure in documentation
- **No orphaned documentation**: Remove docs when source files are deleted

#### Three-Tier Documentation Architecture
1. **index.md** - Main documentation hub with links to articles in format: `[ClassName](ClassName.md)`
2. **Article files** (e.g., `StringExtensions.md`) - Top-level documentation with usage examples, pointing to API docs using: `<xref:Plugin.BaseTypeExtensions.ClassName>`
3. **api/ folder** - Auto-generated API documentation (do not modify directly)

### Universal Documentation Content Standards

#### Required Sections (All Documentation Files)
1. **Class/Module Overview** - Purpose, capabilities, and architectural role
2. **Core Methods** - Primary functionality with complete examples
3. **Advanced Features** - Complex scenarios and integration patterns
4. **Practical Examples** - Real-world usage patterns and common implementations
5. **Performance Considerations** - Optimization notes and characteristics
6. **Thread Safety** - Concurrency behavior and safety guarantees
7. **Best Practices** - Usage guidelines and common pitfalls
8. **Cross-References** - Links to related components using `<xref:>` syntax

#### Documentation Writing Standards

##### Code Examples Requirements
```csharp
// Always provide complete, executable examples
// Include proper error handling demonstrations
// Show both basic and advanced usage patterns
// Use realistic data and scenarios

// Example: String validation pipeline
string ProcessInput(string? input)
{
    return input
        .NullIfEmptyOrWhiteSpace() ?? "default"
        .RemoveSpecialCharacters()
        .CollapseWhitespace()
        .Truncate(50);
}
```

##### Performance Documentation Template
```markdown
### Performance Characteristics
- **Method Complexity**: O(n) with specific behavior notes
- **Memory Usage**: Allocation patterns and optimization notes
- **Aggressive Inlining**: Hot paths use `MethodImpl(MethodImplOptions.AggressiveInlining)`
- **Thread Safety**: All methods are stateless and thread-safe
```

##### Cross-Reference Standards
- Use `<xref:Plugin.BaseTypeExtensions.ClassName>` for automatic API documentation links
- Reference related components within the same module
- Link to complementary modules and utilities
- Include bidirectional references where logical

### Documentation Maintenance Protocols

#### Adding New Extension Classes
1. **Create corresponding documentation file** using `{ClassName}.md` naming convention
2. **Follow content standards** including all required sections with comprehensive examples
3. **Update index.md** to include new component with `[ClassName](ClassName.md)` link
4. **Establish cross-references** to related existing components using `<xref:>` syntax
5. **Validate completeness** against component's public API surface

#### Modifying Existing Extension Classes
1. **Update documentation synchronously** with code changes
2. **Refresh examples** to reflect new functionality or changed behavior
3. **Update performance notes** if implementation changes affect characteristics
4. **Maintain cross-reference accuracy** and add new relationships
5. **Preserve backward compatibility notes** for breaking changes

## Key Implementation Details

### Performance Optimizations
- Uses `[MethodImpl(MethodImplOptions.AggressiveInlining)]` for hot paths
- Generic constraints with `INumber<T>`, `IComparable<T>`, `IFloatingPoint<T>` for compile-time safety
- Zero-allocation patterns where possible
- Culture-independent operations for reliable behavior

### Safety Features
- Extensive null checking with descriptive error messages
- Thread-safe patterns for async operations (TaskExtensions, CancellationTokenExtensions)
- Argument validation using `ArgumentNullException.ThrowIfNull()`
- Generic constraint validation for type safety

### String Extension Specifics
- Comprehensive validation (email, URL, numeric)
- Case conversion (camelCase, PascalCase, kebab-case, snake_case)
- Encoding operations (Base64, HTML encoding/decoding)
- Cultural-neutral operations for consistent behavior

### Async Operation Patterns
- Timeout management with cancellation token support
- Deterministic timeout behavior without race conditions
- Proper resource disposal patterns
- Exception handling that distinguishes timeout vs cancellation

## Error Handling Philosophy
- Throw meaningful exceptions with context (method name, parameter values, expected behavior)
- Provide `OrDefault` variants for non-throwing scenarios where appropriate
- Use specific exception types (`ArgumentNullException`, `TimeoutException`, `OperationCanceledException`)
- Include helpful error messages that aid debugging

## CI/CD Integration
- GitHub Actions with automatic versioning based on git tags
- Deploys to NuGet.org and GitHub Pages documentation
- 95% code coverage requirement enforced
- Multi-targeting support for future .NET versions
- Quality gates prevent merging of failing builds

## Generic Constraint Guidelines
- Use `INumber<T>` for numeric operations (see `NumericExtensions.cs`)
- Use `IComparable<T>` for comparison operations (see `ComparableExtensions.cs`)
- Use `IFloatingPoint<T>` for floating-point specific operations
- Constrain to `struct, Enum` for enum operations (see `EnumExtensions.cs`)
- Prefer compile-time constraints over runtime type checking

When extending this library, maintain the established patterns for consistency, follow the safety-first approach with comprehensive error handling, and ensure all new functionality includes corresponding tests and documentation following the three-tier architecture.
