# ComparableExtensions

Provides enhanced comparison operations for IComparable<T> types.

## Overview

The `ComparableExtensions` class offers fluent comparison operations and range checking utilities for any type that implements `IComparable<T>`.

## API Reference

<xref:Plugin.BaseTypeExtensions.ComparableExtensions>

## Core Methods

### Range Operations

#### `IsBetween<T>(T value, T min, T max, bool inclusive = true)` where T : IComparable<T>
Checks if a value is within a specified range.

```csharp
int value = 50;
bool inRange = value.IsBetween(0, 100); // true (inclusive by default)
bool inRangeExclusive = value.IsBetween(0, 100, false); // true

DateTime date = DateTime.Now;
bool isToday = date.IsBetween(DateTime.Today, DateTime.Today.AddDays(1), false);
```

#### `IsOutsideRange<T>(T value, T min, T max, bool inclusive = true)` where T : IComparable<T>
Checks if a value is outside a specified range.

```csharp
int value = 150;
bool outside = value.IsOutsideRange(0, 100); // true

double temperature = -10.5;
bool belowFreezing = temperature.IsOutsideRange(0.0, 100.0); // true
```

### Fluent Comparisons

#### `IsGreaterThan<T>(T value, T other)` where T : IComparable<T>
Checks if value is greater than another value.

```csharp
int score = 85;
bool passedTest = score.IsGreaterThan(70); // true

DateTime deadline = DateTime.Now.AddDays(5);
bool hasTime = deadline.IsGreaterThan(DateTime.Now); // true
```

#### `IsLessThan<T>(T value, T other)` where T : IComparable<T>
Checks if value is less than another value.

```csharp
double price = 19.99;
bool affordable = price.IsLessThan(25.00); // true
```

#### `IsGreaterThanOrEqual<T>(T value, T other)` where T : IComparable<T>
Checks if value is greater than or equal to another value.

```csharp
int age = 18;
bool canVote = age.IsGreaterThanOrEqual(18); // true
```

#### `IsLessThanOrEqual<T>(T value, T other)` where T : IComparable<T>
Checks if value is less than or equal to another value.

```csharp
int inventory = 5;
bool lowStock = inventory.IsLessThanOrEqual(10); // true
```

### Boundary Operations

#### `Min<T>(T value, T other)` where T : IComparable<T>
Returns the smaller of two values.

```csharp
int result = 10.Min(5); // 5
DateTime earlier = DateTime.Now.Min(DateTime.Today); // DateTime.Today
```

#### `Max<T>(T value, T other)` where T : IComparable<T>
Returns the larger of two values.

```csharp
int result = 10.Max(5); // 10
double higher = 3.14.Max(2.71); // 3.14
```

## Advanced Operations

### Conditional Comparisons

#### `CompareWith<T>(T value, T other)` where T : IComparable<T>
Returns the comparison result as an integer.

```csharp
int comparison = 5.CompareWith(3); // 1 (greater)
int comparison2 = 3.CompareWith(5); // -1 (less)
int comparison3 = 5.CompareWith(5); // 0 (equal)
```

### Range Validation

#### `EnsureInRange<T>(T value, T min, T max, string parameterName = null)` where T : IComparable<T>
Throws exception if value is outside the specified range.

```csharp
public void SetVolume(int volume)
{
    volume.EnsureInRange(0, 100, nameof(volume));
    _volume = volume;
}

public void ScheduleMeeting(DateTime meetingTime)
{
    meetingTime.EnsureInRange(DateTime.Today, DateTime.Today.AddYears(1), nameof(meetingTime));
    _meetings.Add(meetingTime);
}
```

## Practical Examples

### Validation Framework

```csharp
public class ValidationHelper
{
    public static ValidationResult ValidateAge(int age)
    {
        if (age.IsBetween(0, 150))
            return ValidationResult.Success;

        return new ValidationResult("Age must be between 0 and 150");
    }

    public static ValidationResult ValidateScore(double score)
    {
        if (score.IsBetween(0.0, 100.0))
            return ValidationResult.Success;

        return new ValidationResult("Score must be between 0 and 100");
    }

    public static ValidationResult ValidateDate(DateTime date, DateTime minDate, DateTime maxDate)
    {
        if (date.IsBetween(minDate, maxDate))
            return ValidationResult.Success;

        return new ValidationResult($"Date must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}");
    }
}
```

### Range-Based Logic

```csharp
public class PricingEngine
{
    public decimal CalculateDiscount(decimal orderAmount)
    {
        return orderAmount switch
        {
            var amount when amount.IsLessThan(50m) => 0m,
            var amount when amount.IsBetween(50m, 100m) => 0.05m,
            var amount when amount.IsBetween(100m, 500m) => 0.10m,
            var amount when amount.IsGreaterThan(500m) => 0.15m,
            _ => 0m
        };
    }

    public string GetCustomerTier(int totalOrders)
    {
        return totalOrders switch
        {
            var orders when orders.IsLessThan(5) => "Bronze",
            var orders when orders.IsBetween(5, 25) => "Silver",
            var orders when orders.IsBetween(26, 100) => "Gold",
            var orders when orders.IsGreaterThan(100) => "Platinum",
            _ => "Unknown"
        };
    }
}
```

### Configuration Management

```csharp
public class ConfigurationSettings
{
    private int _timeout = 30;
    private double _retryMultiplier = 1.5;

    public int Timeout
    {
        get => _timeout;
        set
        {
            value.EnsureInRange(1, 300, nameof(Timeout));
            _timeout = value;
        }
    }

    public double RetryMultiplier
    {
        get => _retryMultiplier;
        set
        {
            value.EnsureInRange(1.0, 5.0, nameof(RetryMultiplier));
            _retryMultiplier = value;
        }
    }

    public bool IsValidConfiguration()
    {
        return _timeout.IsBetween(1, 300) &&
               _retryMultiplier.IsBetween(1.0, 5.0);
    }
}
```

## Performance Considerations

- All operations use the type's built-in `CompareTo` method
- No boxing for value types when using generic constraints
- Efficient inline operations for hot paths
- Minimal overhead compared to direct comparison operations

## Thread Safety

- All extension methods are thread-safe
- Comparison operations are stateless
- Safe for concurrent usage across multiple threads

## Best Practices

1. **Type Constraints**: Ensure types implement `IComparable<T>` for compile-time safety
2. **Range Validation**: Use `EnsureInRange` for parameter validation
3. **Fluent API**: Chain comparison operations for readable code
4. **Performance**: Use appropriate comparison methods based on frequency
5. **Null Safety**: Be aware that nullable types require null checks before comparison
