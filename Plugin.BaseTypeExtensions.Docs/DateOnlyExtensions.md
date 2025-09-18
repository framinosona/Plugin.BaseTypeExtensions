# DateOnlyExtensions

Provides utility methods for DateOnly type operations and calculations.

## Overview

The `DateOnlyExtensions` class offers utilities for DateOnly operations, including percentage calculations and range operations for modern date handling in .NET.

## API Reference

<xref:Plugin.BaseTypeExtensions.DateOnlyExtensions>

## Core Methods

### Percentage Operations

#### `PercentageToDateOnly(double percentage, DateOnly start, DateOnly end)`
Converts a percentage (0.0-1.0) to a DateOnly within a specified range.

```csharp
var start = new DateOnly(2024, 1, 1);
var end = new DateOnly(2024, 12, 31);
var midYear = 0.5.PercentageToDateOnly(start, end); // Around July 1, 2024
```

#### `DateOnlyToPercentage(DateOnly value, DateOnly start, DateOnly end)`
Converts a DateOnly to a percentage within a specified range.

```csharp
var start = new DateOnly(2024, 1, 1);
var end = new DateOnly(2024, 12, 31);
var july1 = new DateOnly(2024, 7, 1);
var percentage = july1.DateOnlyToPercentage(start, end); // ~0.5
```

## Practical Examples

### Date Range Calculations

```csharp
public class DateRangeCalculator
{
    public DateOnly CalculateQuarterEnd(DateOnly yearStart, int quarter)
    {
        var yearEnd = new DateOnly(yearStart.Year, 12, 31);
        var quarterPercentage = quarter * 0.25;
        return quarterPercentage.PercentageToDateOnly(yearStart, yearEnd);
    }

    public double GetYearProgress(DateOnly current, DateOnly yearStart)
    {
        var yearEnd = new DateOnly(yearStart.Year, 12, 31);
        return current.DateOnlyToPercentage(yearStart, yearEnd);
    }
}
```

## Performance Considerations

- Efficient DateOnly calculations without time component overhead
- Optimized for date-only operations
- Thread-safe operations

## Best Practices

1. **Use DateOnly for Date-Only Operations**: Prefer DateOnly over DateTime when time is not relevant
2. **Validate Ranges**: Ensure start date is before end date
3. **Year Boundaries**: Be mindful of year boundaries in calculations
4. **Leap Years**: Consider leap year effects in long-range calculations
