# NumericExtensions

Provides powerful numeric operations for all `INumber<T>` types in .NET.

## Overview

The `NumericExtensions` class provides type-safe numeric operations using modern .NET generic constraints (`INumber<T>`, `IFloatingPoint<T>`) for compile-time safety and performance.

## API Reference

<xref:Plugin.BaseTypeExtensions.NumericExtensions>

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

## Performance Characteristics

### Aggressive Inlining
All methods use `MethodImpl(MethodImplOptions.AggressiveInlining)` for optimal performance in hot paths.

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

## Best Practices

1. **Use Type Constraints**: Leverage compile-time safety with generic constraints
2. **Range Validation**: Always validate min/max ranges before calculations
3. **Precision Awareness**: Be mindful of floating-point precision in calculations
4. **Performance**: Utilize aggressive inlining for hot paths
5. **Null Safety**: All operations are null-safe by design
