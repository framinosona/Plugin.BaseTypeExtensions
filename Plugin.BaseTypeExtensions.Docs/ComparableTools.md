# ComparableTools

Provides utility methods for comparable operations and advanced comparison scenarios.

## Overview

The `ComparableTools` class offers static utility methods for working with comparable types, including multi-value comparisons, range operations, and comparison utilities.

## API Reference

<xref:Plugin.BaseTypeExtensions.ComparableTools>

## Core Methods

### Multi-Value Comparisons

#### `Min<T>(params T[] values)` where T : IComparable<T>
Finds the minimum value among multiple values.

```csharp
int min = ComparableTools.Min(5, 2, 8, 1, 9); // 1
double minDouble = ComparableTools.Min(3.14, 2.71, 1.41); // 1.41
DateTime earliest = ComparableTools.Min(DateTime.Today, DateTime.Now, DateTime.Today.AddDays(-1));
```

#### `Max<T>(params T[] values)` where T : IComparable<T>
Finds the maximum value among multiple values.

```csharp
int max = ComparableTools.Max(5, 2, 8, 1, 9); // 9
string latest = ComparableTools.Max("apple", "zebra", "banana"); // "zebra" (lexicographic)
```

### Range Operations

#### `ClampToRange<T>(T value, T min, T max)` where T : IComparable<T>
Clamps a value to stay within specified bounds.

```csharp
int clamped = ComparableTools.ClampToRange(150, 0, 100); // 100
double clampedDouble = ComparableTools.ClampToRange(-5.0, 0.0, 10.0); // 0.0
DateTime clampedDate = ComparableTools.ClampToRange(DateTime.Now.AddYears(5), DateTime.Today, DateTime.Today.AddYears(1));
```

#### `CreateRange<T>(T start, T end, int steps)` where T : IComparable<T>
Creates a range of values between start and end with specified number of steps.

```csharp
// Note: This method may require additional implementation for different types
var range = ComparableTools.CreateRange(0, 100, 5);
// Creates values distributed between 0 and 100 in 5 steps
```

### Comparison Utilities

#### `AreEqual<T>(T value1, T value2)` where T : IComparable<T>
Checks if two comparable values are equal.

```csharp
bool equal = ComparableTools.AreEqual(5, 5); // true
bool notEqual = ComparableTools.AreEqual("hello", "world"); // false
```

#### `Compare<T>(T value1, T value2)` where T : IComparable<T>
Compares two values and returns the comparison result.

```csharp
int result = ComparableTools.Compare(5, 3); // 1 (first is greater)
int result2 = ComparableTools.Compare(3, 5); // -1 (first is less)
int result3 = ComparableTools.Compare(5, 5); // 0 (equal)
```

## Advanced Operations

### Boundary Analysis

#### `FindBounds<T>(IEnumerable<T> values)` where T : IComparable<T>
Finds the minimum and maximum values in a collection.

```csharp
var numbers = new[] { 5, 2, 8, 1, 9, 3 };
var (min, max) = ComparableTools.FindBounds(numbers);
// min = 1, max = 9

var dates = new[] { DateTime.Today, DateTime.Now, DateTime.Today.AddDays(-1) };
var (earliest, latest) = ComparableTools.FindBounds(dates);
```

### Sorting Utilities

#### `Sort<T>(IEnumerable<T> values, bool ascending = true)` where T : IComparable<T>
Sorts values in ascending or descending order.

```csharp
var numbers = new[] { 5, 2, 8, 1, 9 };
var ascending = ComparableTools.Sort(numbers); // [1, 2, 5, 8, 9]
var descending = ComparableTools.Sort(numbers, false); // [9, 8, 5, 2, 1]
```

## Practical Examples

### Statistical Analysis

```csharp
public class StatisticsCalculator
{
    public (T Min, T Max, T Median) CalculateStats<T>(IEnumerable<T> values)
        where T : IComparable<T>
    {
        var (min, max) = ComparableTools.FindBounds(values);
        var sorted = ComparableTools.Sort(values).ToArray();
        var median = sorted[sorted.Length / 2];

        return (min, max, median);
    }

    public T[] RemoveOutliers<T>(IEnumerable<T> values, T lowerBound, T upperBound)
        where T : IComparable<T>
    {
        return values
            .Where(v => ComparableTools.Compare(v, lowerBound) >= 0 &&
                       ComparableTools.Compare(v, upperBound) <= 0)
            .ToArray();
    }
}
```

### Configuration Validation

```csharp
public class ConfigurationValidator
{
    public ValidationResult ValidateNumericRange<T>(T value, T min, T max, string parameterName)
        where T : IComparable<T>
    {
        var clamped = ComparableTools.ClampToRange(value, min, max);

        if (!ComparableTools.AreEqual(value, clamped))
        {
            return new ValidationResult(
                $"{parameterName} must be between {min} and {max}. " +
                $"Provided: {value}, Clamped to: {clamped}");
        }

        return ValidationResult.Success;
    }

    public T[] ValidateAndClampArray<T>(T[] values, T min, T max)
        where T : IComparable<T>
    {
        return values
            .Select(v => ComparableTools.ClampToRange(v, min, max))
            .ToArray();
    }
}
```

### Data Processing Pipeline

```csharp
public class DataProcessor
{
    public ProcessingResult<T> ProcessData<T>(IEnumerable<T> data, T threshold)
        where T : IComparable<T>
    {
        var dataArray = data.ToArray();
        var (min, max) = ComparableTools.FindBounds(dataArray);

        var belowThreshold = dataArray
            .Where(v => ComparableTools.Compare(v, threshold) < 0)
            .ToArray();

        var aboveThreshold = dataArray
            .Where(v => ComparableTools.Compare(v, threshold) >= 0)
            .ToArray();

        return new ProcessingResult<T>
        {
            Min = min,
            Max = max,
            Threshold = threshold,
            BelowThreshold = belowThreshold,
            AboveThreshold = aboveThreshold
        };
    }
}

public class ProcessingResult<T> where T : IComparable<T>
{
    public T Min { get; set; }
    public T Max { get; set; }
    public T Threshold { get; set; }
    public T[] BelowThreshold { get; set; }
    public T[] AboveThreshold { get; set; }
}
```

### Range Generation

```csharp
public class RangeGenerator
{
    public T[] GenerateSteps<T>(T start, T end, int numberOfSteps)
        where T : IComparable<T>, INumber<T>
    {
        if (numberOfSteps <= 1)
            return new[] { start };

        var steps = new T[numberOfSteps];
        var range = end - start;
        var stepSize = range / T.CreateChecked(numberOfSteps - 1);

        for (int i = 0; i < numberOfSteps; i++)
        {
            steps[i] = start + (stepSize * T.CreateChecked(i));
        }

        // Ensure last step is exactly the end value
        steps[numberOfSteps - 1] = end;

        return steps;
    }
}
```

## Performance Considerations

- All operations use efficient comparison algorithms
- Generic constraints ensure compile-time type safety
- Minimal boxing for value types
- Optimized for both single operations and bulk processing

## Thread Safety

- All static methods are thread-safe
- No shared state between operations
- Safe for concurrent usage across multiple threads

## Best Practices

1. **Type Constraints**: Ensure types implement `IComparable<T>` for all operations
2. **Null Handling**: Check for null values before calling comparison methods
3. **Performance**: Use appropriate methods based on data size and frequency
4. **Range Validation**: Always validate ranges before processing
5. **Error Handling**: Handle edge cases like empty collections or invalid ranges
