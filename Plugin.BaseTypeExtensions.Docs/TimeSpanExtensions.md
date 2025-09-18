# TimeSpanExtensions

Provides comprehensive operations and formatting for TimeSpan objects.

## Overview

The `TimeSpanExtensions` class offers utilities for TimeSpan manipulation, formatting, arithmetic operations, and human-readable representations.

## API Reference

<xref:Plugin.BaseTypeExtensions.TimeSpanExtensions>

## Formatting Methods

### Human-Readable Formatting

#### `ToReadableString(TimeSpan timeSpan)`
Converts TimeSpan to a concise, readable string format.

```csharp
TimeSpan.FromDays(1).ToReadableString();         // "1d"
TimeSpan.FromHours(2).ToReadableString();        // "2h"
TimeSpan.FromMinutes(30).ToReadableString();     // "30m"
TimeSpan.FromSeconds(45).ToReadableString();     // "45s"
TimeSpan.FromMilliseconds(500).ToReadableString(); // "500ms"

// Complex durations
TimeSpan.FromHours(25).ToReadableString();       // "1d 1h"
new TimeSpan(1, 2, 30, 45).ToReadableString();   // "1d 2h 30m 45s"
```

#### `ToHumanReadable(TimeSpan timeSpan)`
Converts TimeSpan to a detailed, human-friendly string.

```csharp
TimeSpan.FromDays(1).ToHumanReadable();          // "1 day"
TimeSpan.FromDays(3).ToHumanReadable();          // "3 days"
TimeSpan.FromHours(1).ToHumanReadable();         // "1 hour"
TimeSpan.FromMinutes(30).ToHumanReadable();      // "30 minutes"

// Complex durations with proper pluralization
new TimeSpan(2, 3, 30, 0).ToHumanReadable();     // "2 days, 3 hours, 30 minutes"
new TimeSpan(0, 1, 1, 1).ToHumanReadable();      // "1 hour, 1 minute, 1 second"
```

## Arithmetic Operations

### Multiplication and Division

#### `Multiply(TimeSpan timeSpan, double multiplier)`
Multiplies TimeSpan by a factor.

```csharp
var duration = TimeSpan.FromHours(2);
var doubled = duration.Multiply(2.0);            // 4 hours
var halved = duration.Multiply(0.5);             // 1 hour
var extended = duration.Multiply(1.5);           // 3 hours
```

#### `Divide(TimeSpan timeSpan, double divisor)`
Divides TimeSpan by a factor.

```csharp
var duration = TimeSpan.FromHours(6);
var half = duration.Divide(2.0);                 // 3 hours
var third = duration.Divide(3.0);                // 2 hours
```

### Percentage Operations

#### `Percentage(TimeSpan timeSpan, double percentage)`
Calculates percentage of TimeSpan.

```csharp
var fullDuration = TimeSpan.FromHours(8);
var quarter = fullDuration.Percentage(25.0);     // 2 hours (25% of 8 hours)
var half = fullDuration.Percentage(50.0);        // 4 hours
var oneAndHalf = fullDuration.Percentage(150.0); // 12 hours
```

## Rounding Operations

### Time Rounding

#### `RoundToNearest(TimeSpan timeSpan, TimeSpan interval)`
Rounds TimeSpan to nearest interval.

```csharp
var time = TimeSpan.FromMinutes(37);
var rounded = time.RoundToNearest(TimeSpan.FromMinutes(15)); // 30 minutes

var precise = TimeSpan.FromSeconds(127);
var roundedMin = precise.RoundToNearest(TimeSpan.FromMinutes(1)); // 2 minutes
```

#### `RoundUp(TimeSpan timeSpan, TimeSpan interval)`
Rounds TimeSpan up to next interval.

```csharp
var time = TimeSpan.FromMinutes(31);
var roundedUp = time.RoundUp(TimeSpan.FromMinutes(15)); // 45 minutes

var billing = TimeSpan.FromMinutes(1);
var billableHour = billing.RoundUp(TimeSpan.FromHours(1)); // 1 hour
```

#### `RoundDown(TimeSpan timeSpan, TimeSpan interval)`
Rounds TimeSpan down to previous interval.

```csharp
var time = TimeSpan.FromMinutes(44);
var roundedDown = time.RoundDown(TimeSpan.FromMinutes(15)); // 30 minutes
```

## Performance Considerations

- All methods use `MethodImpl(MethodImplOptions.AggressiveInlining)` for performance
- String formatting methods create minimal allocations
- Arithmetic operations preserve precision where possible
- Rounding operations use efficient tick-based calculations

## Thread Safety

- All extension methods are thread-safe (no shared state)
- TimeSpan is an immutable value type
- Safe for concurrent usage across multiple threads

## Best Practices

1. **Choose Appropriate Formatting**: Use `ToReadableString()` for logs, `ToHumanReadable()` for UI
2. **Round Appropriately**: Consider business rules when rounding time values
3. **Handle Edge Cases**: Account for negative TimeSpan values
4. **Use Meaningful Intervals**: Choose rounding intervals that make business sense
5. **Performance Monitoring**: Use TimeSpan extensions for consistent time formatting in metrics
