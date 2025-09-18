# Plugin.BaseTypeExtensions

Welcome to **Plugin.BaseTypeExtensions*### 🚀 Advanced Utilities
- **EnumExtensions** - Enum manipulation and flag operations | <xref:Plugin.BaseTypeExtensions.EnumExtensions>
- **ComparableExtensions** - Generic comparison utilities | <xref:Plugin.BaseTypeExtensions.ComparableExtensions>
- **TaskCompletionSourceExtensions** - Enhanced TaskCompletionSource operations | <xref:Plugin.BaseTypeExtensions.TaskCompletionSourceExtensions>a comprehensive .NET 9 library that provides powerful extension methods for base types, designed to enhance your development experience with clean, intuitive, and performance-optimized code.

## 🚀 Overview

This library extends .NET's fundamental types with a rich collection of utility methods, following modern .NET patterns and leveraging advanced generic constraints like `INumber<T>` and `IComparable<T>` for type safety and performance.

## 📋 Complete Feature Documentation

### 🔤 String & Text Processing
- **<xref:Plugin.BaseTypeExtensions.StringExtensions>** - Comprehensive string manipulation, validation, transformation, and utility methods (40+ methods) | <xref:Plugin.BaseTypeExtensions.StringExtensions>
  - Validation (email, URL, numeric)
  - Case conversion (camelCase, PascalCase, kebab-case, snake_case)
  - Manipulation (truncation, padding, whitespace handling)
  - Encoding (Base64, HTML encoding/decoding)
  - Analysis (word count, character analysis, substring extraction)

### 🔢 Numeric & Mathematical Operations
- **<xref:Plugin.BaseTypeExtensions.NumericExtensions>** - Type-safe numeric operations for all `INumber<T>` types | <xref:Plugin.BaseTypeExtensions.NumericExtensions>
  - Range operations (clamp, percentage calculations)
  - Angle conversions (degree/radian)
  - Type conversions with safety
  - Generic constraints for compile-time safety

### 📋 Collection & LINQ Operations
- **<xref:Plugin.BaseTypeExtensions.EnumerableExtensions>** - Advanced LINQ-style operations and utilities | <xref:Plugin.BaseTypeExtensions.EnumerableExtensions>
  - Batch processing and chunking
  - Conditional operations (WhereIf, SelectIf)
  - Advanced LINQ operations (DistinctBy, MaxBy, MinBy)
  - Safety extensions (EmptyIfNull, IsNullOrEmpty)

### ⚡ Async & Threading
- **<xref:Plugin.BaseTypeExtensions.TaskExtensions>** - Advanced async operations and timeout management | <xref:Plugin.BaseTypeExtensions.TaskExtensions>
  - Timeout operations with cancellation support
  - Configurable timeout management
  - Robust error handling patterns
- **<xref:Plugin.BaseTypeExtensions.CancellationTokenExtensions>** - Advanced timeout and async operations for CancellationToken | <xref:Plugin.BaseTypeExtensions.CancellationTokenExtensions>
  - Timeout creation and management
  - Task conversion and coordination
  - Combined cancellation patterns

### ⏰ Time & Date Operations
- **<xref:Plugin.BaseTypeExtensions.TimeSpanExtensions>** - Comprehensive TimeSpan operations and formatting | <xref:Plugin.BaseTypeExtensions.TimeSpanExtensions>
  - Human-readable formatting (ToReadableString, ToHumanReadable)
  - Arithmetic operations (multiply, divide, percentage)
  - Rounding operations (RoundToNearest, RoundUp, RoundDown)
  - Conversion and precision methods

### 📅 Date & Time Extensions
- **DateTimeExtensions** - DateTime manipulation and formatting utilities | <xref:Plugin.BaseTypeExtensions.DateTimeExtensions>
- **DateOnlyExtensions** - DateOnly type extensions for modern date handling | <xref:Plugin.BaseTypeExtensions.DateOnlyExtensions>
- **TimeOnlyExtensions** - TimeOnly type extensions for time-specific operations | <xref:Plugin.BaseTypeExtensions.TimeOnlyExtensions>

### 🗂️ Collection Specific Extensions
- **ListExtensions** - List-specific operations and utilities | <xref:Plugin.BaseTypeExtensions.ListExtensions>
- **DictionaryExtensions** - Dictionary manipulation and safety extensions | <xref:Plugin.BaseTypeExtensions.DictionaryExtensions>

### 🔍 Reflection & Metadata
- **AssemblyExtensions** - Assembly information and metadata extraction | <xref:Plugin.BaseTypeExtensions.AssemblyExtensions>
- **ReflectionExtensions** - Advanced reflection utilities and type operations | <xref:Plugin.BaseTypeExtensions.ReflectionExtensions>

### 🎲 Utility Extensions
- **RandomExtensions** - Enhanced random number generation and utilities | <xref:Plugin.BaseTypeExtensions.RandomExtensions>
- **GuidExtensions** - GUID manipulation and validation | <xref:Plugin.BaseTypeExtensions.GuidExtensions>
- **VersionExtensions** - Version comparison and formatting | <xref:Plugin.BaseTypeExtensions.VersionExtensions>
- **ByteExtensions** - Byte array operations and conversions | <xref:Plugin.BaseTypeExtensions.ByteExtensions>
- **UriExtensions** - URI manipulation and validation utilities | <xref:Plugin.BaseTypeExtensions.UriExtensions>
- **StreamExtensions** - Stream operations and utilities | <xref:Plugin.BaseTypeExtensions.StreamExtensions>
- **ExceptionExtensions** - Exception handling and manipulation utilities | <xref:Plugin.BaseTypeExtensions.ExceptionExtensions>

### � Advanced Utilities
- **EnumExtensions** - Enum manipulation and flag operations
- **ComparableExtensions** - Generic comparison utilities
- **TaskCompletionSourceExtensions** - Enhanced TaskCompletionSource operations

### 🛠️ Utility Classes
- **ComparableTools** - Utility methods for comparable operations | <xref:Plugin.BaseTypeExtensions.ComparableTools>
- **NumericRangeTools** - Numeric range generation and manipulation | <xref:Plugin.BaseTypeExtensions.NumericRangeTools>

## 🏁 Quick Start

### Installation

```bash
dotnet add package Plugin.BaseTypeExtensions
```

### Basic Usage

```csharp
using Plugin.BaseTypeExtensions;

// String operations
string email = "  user@EXAMPLE.com  ";
string clean = email.NullIfDud()?.Trim().ToLowerInvariant();
bool isValid = clean.IsValidEmail(); // true

// Numeric operations
int value = 150;
int clamped = value.Clamp(0, 100); // 100
double percentage = value.ValueToPercentage(0, 200); // 0.75

// Collection operations
var numbers = Enumerable.Range(1, 100);
var chunks = numbers.ChunkBy(10); // 10 chunks of 10 items each

// Async operations with timeout
var result = await SomeLongRunningTask()
    .WithTimeoutInMs(5000); // 5 second timeout

// TimeSpan formatting
var duration = TimeSpan.FromHours(2.5);
string readable = duration.ToReadableString(); // "2h 30m"
string human = duration.ToHumanReadable(); // "2 hours, 30 minutes"

// CancellationToken with timeout
using var cts = new CancellationTokenSource();
var timeoutToken = cts.Token.WithTimeout(TimeSpan.FromMinutes(5));
await ProcessDataAsync(timeoutToken);
```

## 🎯 Design Principles

### Performance First
- **Aggressive Inlining**: Critical methods use `MethodImpl(MethodImplOptions.AggressiveInlining)`
- **Zero Allocations**: Minimal memory allocation in hot paths
- **Generic Constraints**: Compile-time type safety with `INumber<T>`, `IComparable<T>`

### Type Safety
- **Modern .NET**: Leverages .NET 9 features and nullable reference types
- **Generic Constraints**: Prevents runtime type errors
- **Null Safety**: Comprehensive null handling throughout

### Clean API Design
- **Intuitive Names**: Methods named for clarity and discoverability
- **Consistent Patterns**: Uniform naming and parameter conventions
- **Fluent Interface**: Chain operations naturally

### Comprehensive Testing
- **95% Coverage**: Minimum test coverage requirement (386+ tests)
- **Edge Cases**: Extensive edge case and error condition testing
- **Thread Safety**: Deterministic testing for async operations
- **Performance Tests**: Benchmarks for critical paths

## 💡 Advanced Examples

### Data Processing Pipeline
```csharp
var results = await data
    .EmptyIfNull()
    .WhereIf(includeInactive, item => item.IsActive)
    .DistinctBy(item => item.Id)
    .ProcessInBatchesAsync(100, async batch =>
    {
        return await ProcessBatchAsync(batch)
            .WithTimeoutInMs(30000);
    });
```

### Configuration Validation
```csharp
public bool ValidateConfig(Dictionary<string, string> config)
{
    return config
        .EmptyIfNull()
        .Where(kvp => kvp.Key.IsValidEmail() || kvp.Key.IsValidUrl())
        .All(kvp => kvp.Value.NullIfDud() != null);
}
```

### Time-Based Operations
```csharp
public async Task<T> ExecuteWithProgressAsync<T>(
    Func<CancellationToken, Task<T>> operation,
    TimeSpan estimatedDuration,
    IProgress<string> progress)
{
    var timeoutToken = CancellationToken.None.WithTimeout(estimatedDuration.Multiply(1.5));
    var stopwatch = Stopwatch.StartNew();

    var progressTask = Task.Run(async () =>
    {
        while (!timeoutToken.IsCancellationRequested)
        {
            var elapsed = stopwatch.Elapsed;
            var remaining = estimatedDuration - elapsed;
            progress?.Report($"Elapsed: {elapsed.ToReadableString()}, Est. Remaining: {remaining.ToReadableString()}");

            await Task.Delay(TimeSpan.FromSeconds(1), timeoutToken);
        }
    });

    try
    {
        return await operation(timeoutToken);
    }
    finally
    {
        stopwatch.Stop();
    }
}
```

### Async Coordination with Multiple Timeouts
```csharp
public async Task<ProcessingResult> CoordinatedProcessingAsync(
    IEnumerable<ProcessingTask> tasks,
    ProcessingOptions options)
{
    using var masterCts = new CancellationTokenSource();
    var overallTimeout = masterCts.Token.WithTimeout(options.OverallTimeout);

    var results = new ConcurrentBag<TaskResult>();

    await tasks
        .EmptyIfNull()
        .ProcessInBatchesAsync(options.MaxConcurrency, async batch =>
        {
            var batchTasks = batch.Select(async task =>
            {
                var taskTimeout = overallTimeout.WithTimeout(options.TaskTimeout);

                try
                {
                    var result = await task.ExecuteAsync(taskTimeout);
                    results.Add(new TaskResult { Success = true, Result = result });
                }
                catch (OperationCanceledException) when (overallTimeout.IsCancellationRequested)
                {
                    results.Add(new TaskResult { Success = false, Error = "Overall timeout" });
                }
                catch (Exception ex)
                {
                    results.Add(new TaskResult { Success = false, Error = ex.Message });
                }
            });

            await Task.WhenAll(batchTasks);
        });

    return new ProcessingResult
    {
        Results = results.ToList(),
        Duration = masterCts.Token.AsTask().IsCompleted ? options.OverallTimeout : TimeSpan.Zero
    };
}
```

## 🏗️ Architecture

The library follows a modular extension pattern:

- **Extension Classes**: One class per base type (`StringExtensions`, `NumericExtensions`, etc.)
- **Utility Classes**: Helper classes for complex operations (`ComparableTools`, `NumericRangeTools`)
- **Performance Optimization**: Strategic use of aggressive inlining and generic constraints
- **Comprehensive Testing**: 386+ tests covering all functionality with edge cases
- **Thread-Safe Patterns**: Robust async and threading support

## ⚡️ Performance

All extension methods are optimized for performance:

- **Minimal Allocations**: String operations reuse buffers where possible
- **Efficient Algorithms**: Optimized for common use cases
- **Inlined Operations**: Hot paths use aggressive inlining
- **Culture-Independent**: Reliable behavior across different system cultures
- **Benchmarked**: Regular performance testing ensures optimization

## 🤝 Contributing

We welcome contributions! The codebase follows strict quality standards:

- **Zero Warnings**: `TreatWarningsAsErrors=true` enforced
- **95% Test Coverage**: Comprehensive test coverage required
- **Modern .NET**: Full use of .NET 9 features and patterns
- **Performance Focus**: All additions must maintain performance standards

## 📄 License

This project is licensed under the MIT License.
