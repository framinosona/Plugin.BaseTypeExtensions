# TimeOnlyExtensions

Provides utility methods for TimeOnly type operations and calculations.

## Overview

The `TimeOnlyExtensions` class offers utilities for TimeOnly operations, including percentage calculations and time-specific operations for modern time handling in .NET.

## API Reference

<xref:Plugin.BaseTypeExtensions.TimeOnlyExtensions>

## Core Methods

### Percentage Operations

#### `PercentageToTimeOnly(double percentage, TimeOnly start, TimeOnly end)`
Converts a percentage (0.0-1.0) to a TimeOnly within a specified range.

```csharp
var start = new TimeOnly(9, 0); // 9:00 AM
var end = new TimeOnly(17, 0);   // 5:00 PM
var lunch = 0.5.PercentageToTimeOnly(start, end); // 1:00 PM
```

#### `TimeOnlyToPercentage(TimeOnly value, TimeOnly start, TimeOnly end)`
Converts a TimeOnly to a percentage within a specified range.

```csharp
var start = new TimeOnly(9, 0);  // 9:00 AM
var end = new TimeOnly(17, 0);   // 5:00 PM
var current = new TimeOnly(13, 0); // 1:00 PM
var percentage = current.TimeOnlyToPercentage(start, end); // 0.5
```

## Practical Examples

### Work Schedule Calculations

```csharp
public class WorkScheduleCalculator
{
    private readonly TimeOnly _workStart = new(9, 0);
    private readonly TimeOnly _workEnd = new(17, 0);

    public TimeOnly CalculateBreakTime(double progressPercentage)
    {
        return progressPercentage.PercentageToTimeOnly(_workStart, _workEnd);
    }

    public double GetWorkDayProgress(TimeOnly currentTime)
    {
        return currentTime.TimeOnlyToPercentage(_workStart, _workEnd);
    }
}
```

### Time Range Operations

```csharp
public class TimeSlotManager
{
    public TimeOnly[] GenerateTimeSlots(TimeOnly start, TimeOnly end, int numberOfSlots)
    {
        var slots = new TimeOnly[numberOfSlots];
        for (int i = 0; i < numberOfSlots; i++)
        {
            var percentage = (double)i / (numberOfSlots - 1);
            slots[i] = percentage.PercentageToTimeOnly(start, end);
        }
        return slots;
    }
}
```

## Performance Considerations

- Efficient TimeOnly calculations without date component overhead
- Optimized for time-only operations
- Thread-safe operations

## Best Practices

1. **Use TimeOnly for Time-Only Operations**: Prefer TimeOnly over DateTime when date is not relevant
2. **Handle Day Boundaries**: Be careful with times that cross midnight
3. **Validate Ranges**: Ensure start time is before end time (within same day)
4. **Precision**: Consider the precision requirements for your time calculations
