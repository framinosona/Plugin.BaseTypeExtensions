# 🔢 NumericExtensions

Provides powerful numeric operations for all `INumber<T>` types in .NET.

## Overview

The `NumericExtensions` class provides type-safe numeric operations using modern .NET generic constraints (`INumber<T>`, `IFloatingPoint<T>`) for compile-time safety and performance.

## Core Operations

### Range Operations

#### `Clamp<T>(T value, T min, T max)` where T : INumber<T>, IComparable<T>
Clamps value within specified range.

```csharp
int clamped = 150.Clamp(0, 100); // 100
double clamped = (-5.0).Clamp(0.0, 10.0); // 0.0
float clamped = 75.5f.Clamp(0f, 50f); // 50f
```

### Percentage Calculations

#### `PercentageToValue<T>(double percentage, T min, T max)`
Converts percentage (0.0-1.0) to value within range.

```csharp
// 50% between 0 and 100 = 50
int value = 0.5.PercentageToValue(0, 100); // 50

// 25% between 10.0 and 20.0 = 12.5
double value = 0.25.PercentageToValue(10.0, 20.0); // 12.5
```

#### `ValueToPercentage<T>(T value, T min, T max)`
Converts value to percentage within range.

```csharp
// 75 in range 0-100 = 0.75 (75%)
double percent = 75.ValueToPercentage(0, 100); // 0.75

// 15.0 in range 10.0-20.0 = 0.5 (50%)
double percent = 15.0.ValueToPercentage(10.0, 20.0); // 0.5
```

## Angle Conversions

### Degree/Radian Conversion

#### `DegreeToRadian<T>(T degrees)` where T : IFloatingPoint<T>
Converts degrees to radians.

```csharp
double radians = 180.0.DegreeToRadian(); // π (3.14159...)
float radians = 90f.DegreeToRadian(); // π/2
```

#### `RadianToDegree<T>(T radians)` where T : IFloatingPoint<T>
Converts radians to degrees.

```csharp
double degrees = Math.PI.RadianToDegree(); // 180.0
float degrees = (Math.PI / 2).RadianToDegree(); // 90.0
```

## Type Conversion

### Byte Conversion

#### `ToByte<T>(T value)` where T : INumber<T>
Safely converts numeric value to byte with clamping.

```csharp
byte result = 300.ToByte(); // 255 (clamped to byte.MaxValue)
byte result = (-10).ToByte(); // 0 (clamped to byte.MinValue)
byte result = 100.ToByte(); // 100
```

## Advanced Examples

### Working with Different Numeric Types

```csharp
// Works with any INumber<T> type
public T ProcessValue<T>(T input, T min, T max) where T : INumber<T>, IComparable<T>
{
    return input.Clamp(min, max);
}

// Usage with different types
int intResult = ProcessValue(150, 0, 100); // 100
double doubleResult = ProcessValue(1.5, 0.0, 1.0); // 1.0
float floatResult = ProcessValue(75.5f, 0f, 50f); // 50f
```

### Progress Calculations

```csharp
// Calculate download progress
public double CalculateProgress(long downloaded, long total)
{
    return downloaded.ValueToPercentage(0L, total);
}

// Map progress to visual range
public int ProgressToPixels(double progress, int maxWidth)
{
    return progress.PercentageToValue(0, maxWidth);
}
```

### Angular Operations

```csharp
// Convert compass bearing to radians
double bearing = 45.0; // Northeast
double radians = bearing.DegreeToRadian();

// Calculate rotation
double currentAngle = 30.0;
double targetAngle = 90.0;
double rotationRadians = (targetAngle - currentAngle).DegreeToRadian();
```

### Color Value Mapping

```csharp
// Map value to color intensity
public byte ValueToColorIntensity(double value, double min, double max)
{
    double percentage = value.ValueToPercentage(min, max);
    return percentage.PercentageToValue(0, 255).ToByte();
}

// Usage
byte red = ValueToColorIntensity(temperature, 0.0, 100.0);
```

## Performance Characteristics

### Aggressive Inlining
All methods use `MethodImpl(MethodImplOptions.AggressiveInlining)` for optimal performance in hot paths.

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static T Clamp<T>(this T value, T min, T max)
    where T : INumber<T>, IComparable<T>
```

### Generic Constraints
Utilizes modern .NET generic constraints for compile-time safety:

- `INumber<T>` - Basic numeric operations
- `IFloatingPoint<T>` - Floating-point specific operations
- `IComparable<T>` - Comparison operations

### Zero Allocations
All operations are allocation-free value operations with no boxing.

## Type Safety

### Compile-Time Guarantees

```csharp
// ✅ Valid - int implements INumber<int>
int result = 50.Clamp(0, 100);

// ✅ Valid - double implements IFloatingPoint<double>
double radians = 180.0.DegreeToRadian();

// ❌ Compile Error - string doesn't implement INumber<string>
// string result = "hello".Clamp("a", "z");
```

### Mixed Type Operations

```csharp
// All parameters must be same type
int result = 50.Clamp(0, 100); // ✅ All int
double result = 50.0.Clamp(0.0, 100.0); // ✅ All double

// Compile-time error for mixed types
// int result = 50.Clamp(0.0, 100); // ❌ Mixed int/double
```

## Integration with NumericRangeTools

Works seamlessly with `NumericRangeTools` for range generation:

```csharp
// Generate range and clamp values
var range = NumericRangeTools.GetRange(0.0, 10.0, 0.5);
var clampedValues = inputValues.Select(v => v.Clamp(0.0, 10.0));

// Convert range to percentages
var percentages = range.Select(v => v.ValueToPercentage(0.0, 10.0));
```

## Common Patterns

### Configuration Value Clamping

```csharp
public class Settings
{
    private int _volume = 50;

    public int Volume
    {
        get => _volume;
        set => _volume = value.Clamp(0, 100);
    }
}
```

### Animation Interpolation

```csharp
public Vector3 Lerp(Vector3 start, Vector3 end, double t)
{
    t = t.Clamp(0.0, 1.0); // Ensure valid range
    return new Vector3(
        t.PercentageToValue(start.X, end.X),
        t.PercentageToValue(start.Y, end.Y),
        t.PercentageToValue(start.Z, end.Z)
    );
}
```

### Data Normalization

```csharp
public IEnumerable<double> NormalizeData(IEnumerable<double> values)
{
    var min = values.Min();
    var max = values.Max();
    return values.Select(v => v.ValueToPercentage(min, max));
}
```
