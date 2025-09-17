# Plugin.BaseTypeExtensions - AI C### Build configuration**: Sophisticated MSBuild setup with `Directory.Build.props` and `Directory.Build.targets`
  - **Three-tier MSBuild pattern**: `Directory.Packages.props` (central packages), `Directory.Build.props` (pre-project), `Directory.Build.targets` (post-project)
  - **CI/CD auto-detection**: Automatically switches to Release configuration in CI environments
  - **Strict quality gates**: All .NET analyzers enabled, warnings treated as errorsding Agent Instructions

## Project Overview
This is a .NET 9 library providing comprehensive extension methods for base types. The architecture follows a modular extension pattern with each type having dedicated extension classes (e.g., `StringExtensions`, `NumericExtensions`) plus utility classes ending in `Tools` (e.g., `ComparableTools`, `NumericRangeTools`).

## Key Architectural Patterns

### Extension Method Structure
- **One class per base type**: `StringExtensions.cs`, `NumericExtensions.cs`, etc.
- **Static partial classes**: All extension classes are `static partial` allowing future expansion
- **Generic constraints**: Heavy use of modern .NET generic constraints like `INumber<T>`, `IComparable<T>`
- **Performance optimization**: Liberal use of `[MethodImpl(MethodImplOptions.AggressiveInlining)]`

### Code Conventions
- **Null safety**: Always handle null inputs gracefully, return empty or null as appropriate
- **Documentation**: Every public method has XML documentation with `<summary>`, parameters, returns, and exceptions
- **Naming**: Use descriptive method names like `NullIfDud()`, `RemoveSpecialCharacters()`
- **Type safety**: Prefer compile-time safety with generic constraints over runtime checks
- **Code quality**: Microsoft.CodeAnalysis.NetAnalyzers enabled with `AnalysisMode=AllEnabledByDefault`
- **Zero tolerance**: `TreatWarningsAsErrors=true` - all code must be spotless with no warnings

### Test Patterns
- **File naming**: `[ClassUnderTest]Tests.cs` (e.g., `StringExtensionsTests.cs`)
- **Test framework**: xUnit with FluentAssertions v7.2.0 (licensing compliance)
- **Global usings**: `Xunit` and `FluentAssertions` are globally imported
- **Assertion style**: Prefer `.Should().Be()` over `Assert.Equal()`
- **Coverage requirement**: 95% minimum code coverage enforced in CI
- **Test structure**: Mirror main project 1:1 - each source file has exactly one test file

## Build System

### Project Structure
- **Solution**: Uses modern `.slnx` format instead of traditional `.sln`
- **Central Package Management**: All package versions centralized in `Directory.Packages.props`
- **Build configuration**: Sophisticated MSBuild setup with `Directory.Build.props` and `Directory.Build.targets`
- **Multi-targeting**: Currently .NET 9 only, but extensible architecture

### Development Workflow
```bash
# Build the solution
dotnet build Plugin.BaseTypeExtensions.slnx

# Run tests with coverage
dotnet test Plugin.BaseTypeExtensions.Tests/ --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "StringExtensionsTests"

# Package for NuGet (only in CI)
dotnet build -p:GeneratePackageOnBuild=true
```

### CI/CD Pipeline
- **Versioning**: Automatic semantic versioning based on git tags (`v1.0.*` pattern)
- **Artifacts**: NuGet packages, test results, and documentation
- **Deployment**: Auto-publish to NuGet.org and GitHub Pages documentation
- **Quality gates**: 95% test coverage required for merge

## Implementation Guidelines

### Code Quality Standards
- **All code must compile without warnings** - `TreatWarningsAsErrors=true` is enforced
- **Full static analysis**: `Microsoft.CodeAnalysis.NetAnalyzers` with `AnalysisMode=AllEnabledByDefault`
- **Modern .NET features**: Use latest C# language features, nullable reference types enabled
- **Performance-first**: Use `[MethodImpl(MethodImplOptions.AggressiveInlining)]` for hot paths
- **Documentation**: All public APIs require complete XML documentation with examples

### Adding New Extensions
1. Create new file: `[TypeName]Extensions.cs` in main project
2. Use pattern: `public static partial class [TypeName]Extensions`
3. Add corresponding test file: `[TypeName]ExtensionsTests.cs`
4. Include XML documentation with examples
5. Add performance-critical methods with `[MethodImpl(MethodImplOptions.AggressiveInlining)]`

### Generic Type Constraints
- Use `INumber<T>` for numeric operations (see `NumericExtensions.cs`)
- Use `IComparable<T>` for comparison operations (see `ComparableExtensions.cs`)
- Constrain to `struct, Enum` for enum operations (see `EnumExtensions.cs`)

### Documentation Generation
- DocFX automatically generates API documentation from XML comments
- Deployed to GitHub Pages on main branch commits
- Configuration in `Plugin.BaseTypeExtensions.Docs/docfx.json`

## Key Files Reference
- **Build logic**: `Directory.Build.props`, `Directory.Build.targets`
- **Dependencies**: `Directory.Packages.props`
- **CI pipeline**: `.github/workflows/ci.yml`
- **Core examples**: `StringExtensions.cs`, `NumericExtensions.cs`
- **Test patterns**: `StringExtensionsTests.cs`

## Performance Considerations
- All extension methods are designed for high performance
- Use `Unsafe.SizeOf<T>()` for efficient enum operations (see `EnumExtensions.TweakFlag`)
- Aggressive inlining for simple operations
- Minimal allocations in hot paths
