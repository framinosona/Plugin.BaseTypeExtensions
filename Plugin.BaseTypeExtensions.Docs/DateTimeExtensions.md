# DateTimeExtensions

Provides comprehensive DateTime manipulation and formatting utilities.

## Overview

The `DateTimeExtensions` class offers utilities for DateTime operations, including percentage calculations, range operations, and enhanced formatting capabilities.

## API Reference

<xref:Plugin.BaseTypeExtensions.DateTimeExtensions>

## Core Methods

### Percentage Operations

#### `PercentageToDateTime(double percentage, DateTime start, DateTime end)`
Converts a percentage (0.0-1.0) to a DateTime within a specified range.

```csharp
var start = new DateTime(2024, 1, 1);
var end = new DateTime(2024, 12, 31);
var midYear = 0.5.PercentageToDateTime(start, end); // Around July 1, 2024
```

#### `DateTimeToPercentage(DateTime value, DateTime start, DateTime end)`
Converts a DateTime to a percentage within a specified range.

```csharp
var start = new DateTime(2024, 1, 1);
var end = new DateTime(2024, 12, 31);
var july1 = new DateTime(2024, 7, 1);
var percentage = july1.DateTimeToPercentage(start, end); // ~0.5
```

## Practical Examples

### Progress Tracking

```csharp
public class ProjectTracker
{
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;

    public ProjectTracker(DateTime startDate, DateTime endDate)
    {
        _startDate = startDate;
        _endDate = endDate;
    }

    public double GetProgress()
    {
        return DateTime.Now.DateTimeToPercentage(_startDate, _endDate);
    }

    public DateTime GetProjectedEndDate(double completionPercentage)
    {
        return completionPercentage.PercentageToDateTime(_startDate, _endDate);
    }
}
```

### Timeline Operations

```csharp
public class TimelineCalculator
{
    public DateTime CalculateMilestone(DateTime projectStart, DateTime projectEnd, double milestonePercentage)
    {
        return milestonePercentage.PercentageToDateTime(projectStart, projectEnd);
    }

    public double CalculateProgress(DateTime current, DateTime start, DateTime end)
    {
        return current.DateTimeToPercentage(start, end);
    }
}
```

## Performance Considerations

- All operations use efficient DateTime calculations
- No string parsing or formatting overhead
- Optimized for high-frequency calculations
- Thread-safe operations

## Thread Safety

- All extension methods are thread-safe
- DateTime is an immutable value type
- Safe for concurrent usage

## Best Practices

1. **Validate Ranges**: Ensure start date is before end date
2. **Handle Edge Cases**: Check for equal start and end dates
3. **Precision Awareness**: Be mindful of DateTime precision in calculations
4. **Time Zone Considerations**: Use UTC for consistent calculations across time zones
5. **Performance**: Use these methods for timeline and progress calculations
