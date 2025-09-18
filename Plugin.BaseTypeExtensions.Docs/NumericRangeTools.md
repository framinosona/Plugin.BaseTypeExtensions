# NumericRangeTools

Provides numeric range generation and manipulation utilities.

## Overview

The `NumericRangeTools` class offers powerful utilities for generating numeric ranges, creating sequences, and performing range-based calculations using modern .NET generic constraints.

## API Reference

<xref:Plugin.BaseTypeExtensions.NumericRangeTools>

## Core Methods

### Range Generation

#### `GetRange<T>(T start, T end, T step)` where T : INumber<T>
Generates a sequence of numbers from start to end with specified step size.

```csharp
// Integer ranges
var integers = NumericRangeTools.GetRange(0, 10, 2);
// Result: [0, 2, 4, 6, 8, 10]

// Floating-point ranges
var decimals = NumericRangeTools.GetRange(0.0, 1.0, 0.1);
// Result: [0.0, 0.1, 0.2, 0.3, ..., 1.0]

// Custom step sizes
var fibonacci = NumericRangeTools.GetRange(1, 100, 3);
// Result: [1, 4, 7, 10, ..., 97, 100]
```

#### `GetRange<T>(T start, T end, int numberOfSteps)` where T : INumber<T>
Generates a sequence with a specific number of evenly distributed steps.

```csharp
// 5 evenly distributed points between 0 and 100
var points = NumericRangeTools.GetRange(0.0, 100.0, 5);
// Result: [0.0, 25.0, 50.0, 75.0, 100.0]

// 10 points between 1 and 2
var precision = NumericRangeTools.GetRange(1.0, 2.0, 10);
// Result: [1.0, 1.111..., 1.222..., ..., 2.0]
```

### Linear Interpolation

#### `Lerp<T>(T start, T end, double t)` where T : INumber<T>
Performs linear interpolation between two values.

```csharp
// 50% between 0 and 100
double mid = NumericRangeTools.Lerp(0.0, 100.0, 0.5); // 50.0

// 25% between 10 and 20
float quarter = NumericRangeTools.Lerp(10.0f, 20.0f, 0.25); // 12.5f

// Beyond range (extrapolation)
double beyond = NumericRangeTools.Lerp(0.0, 10.0, 1.5); // 15.0
```

#### `InverseLerp<T>(T start, T end, T value)` where T : INumber<T>
Finds the interpolation factor for a value within a range.

```csharp
// What percentage is 50 between 0 and 100?
double t = NumericRangeTools.InverseLerp(0.0, 100.0, 50.0); // 0.5

// What percentage is 75 between 50 and 100?
double t2 = NumericRangeTools.InverseLerp(50.0, 100.0, 75.0); // 0.5
```

## Advanced Operations

### Range Mapping

#### `MapRange<T>(T value, T fromMin, T fromMax, T toMin, T toMax)` where T : INumber<T>
Maps a value from one range to another range.

```csharp
// Map temperature from Celsius (0-100) to Fahrenheit (32-212)
double celsius = 37.0; // Body temperature
double fahrenheit = NumericRangeTools.MapRange(celsius, 0.0, 100.0, 32.0, 212.0);
// Result: 98.6°F

// Map percentage (0-100) to RGB value (0-255)
int rgbValue = NumericRangeTools.MapRange(80, 0, 100, 0, 255);
// Result: 204
```

### Range Analysis

#### `IsInRange<T>(T value, T min, T max, bool inclusive = true)` where T : INumber<T>
Checks if a value falls within a specified numeric range.

```csharp
bool inRange = NumericRangeTools.IsInRange(50, 0, 100); // true
bool outOfRange = NumericRangeTools.IsInRange(150, 0, 100); // false
bool onBoundary = NumericRangeTools.IsInRange(100, 0, 100, false); // false (exclusive)
```

#### `ClampToRange<T>(T value, T min, T max)` where T : INumber<T>
Clamps a value to stay within specified bounds.

```csharp
int clamped = NumericRangeTools.ClampToRange(150, 0, 100); // 100
double clampedDouble = NumericRangeTools.ClampToRange(-5.0, 0.0, 10.0); // 0.0
```

## Practical Examples

### Animation and Graphics

```csharp
public class AnimationHelper
{
    public float[] GenerateEaseInOutCurve(int keyframes)
    {
        var linear = NumericRangeTools.GetRange(0.0f, 1.0f, keyframes);
        return linear.Select(t => EaseInOut(t)).ToArray();
    }

    private float EaseInOut(float t)
    {
        return t < 0.5f
            ? 2 * t * t
            : 1 - (float)Math.Pow(-2 * t + 2, 2) / 2;
    }

    public Color InterpolateColor(Color start, Color end, double t)
    {
        var r = (byte)NumericRangeTools.Lerp(start.R, end.R, t);
        var g = (byte)NumericRangeTools.Lerp(start.G, end.G, t);
        var b = (byte)NumericRangeTools.Lerp(start.B, end.B, t);
        return Color.FromArgb(r, g, b);
    }
}
```

### Scientific Calculations

```csharp
public class ScientificCalculator
{
    public double[] GenerateSineWave(double frequency, double duration, int sampleRate)
    {
        var timePoints = NumericRangeTools.GetRange(0.0, duration, 1.0 / sampleRate);
        return timePoints.Select(t => Math.Sin(2 * Math.PI * frequency * t)).ToArray();
    }

    public (double[] X, double[] Y) GenerateFunction(Func<double, double> func, double xMin, double xMax, int points)
    {
        var xValues = NumericRangeTools.GetRange(xMin, xMax, points).ToArray();
        var yValues = xValues.Select(func).ToArray();
        return (xValues, yValues);
    }

    public double IntegrateTrapezoidal(Func<double, double> func, double a, double b, int intervals)
    {
        var points = NumericRangeTools.GetRange(a, b, intervals + 1).ToArray();
        var h = (b - a) / intervals;

        var sum = func(a) + func(b);
        for (int i = 1; i < intervals; i++)
        {
            sum += 2 * func(points[i]);
        }

        return (h / 2) * sum;
    }
}
```

### Data Visualization

```csharp
public class ChartGenerator
{
    public DataPoint[] GenerateAxisPoints(double min, double max, int tickCount)
    {
        var values = NumericRangeTools.GetRange(min, max, tickCount);
        return values.Select((v, i) => new DataPoint { Value = v, Index = i }).ToArray();
    }

    public double[] NormalizeData(double[] data, double targetMin = 0.0, double targetMax = 1.0)
    {
        var (dataMin, dataMax) = (data.Min(), data.Max());
        return data.Select(value =>
            NumericRangeTools.MapRange(value, dataMin, dataMax, targetMin, targetMax)
        ).ToArray();
    }

    public Color[] GenerateHeatmapColors(double[] values)
    {
        var (min, max) = (values.Min(), values.Max());
        return values.Select(value =>
        {
            var normalized = NumericRangeTools.InverseLerp(min, max, value);
            return GetHeatmapColor(normalized);
        }).ToArray();
    }

    private Color GetHeatmapColor(double intensity)
    {
        // Blue (cold) to Red (hot) gradient
        if (intensity < 0.5)
        {
            var blue = (byte)NumericRangeTools.MapRange(intensity, 0.0, 0.5, 255.0, 0.0);
            var green = (byte)NumericRangeTools.MapRange(intensity, 0.0, 0.5, 0.0, 255.0);
            return Color.FromArgb(0, green, blue);
        }
        else
        {
            var red = (byte)NumericRangeTools.MapRange(intensity, 0.5, 1.0, 0.0, 255.0);
            var green = (byte)NumericRangeTools.MapRange(intensity, 0.5, 1.0, 255.0, 0.0);
            return Color.FromArgb(red, green, 0);
        }
    }
}
```

### Statistical Analysis

```csharp
public class StatisticsHelper
{
    public double[] GenerateNormalDistribution(double mean, double stdDev, int samples)
    {
        var random = new Random();
        var results = new double[samples];

        for (int i = 0; i < samples; i++)
        {
            // Box-Muller transformation
            var u1 = random.NextDouble();
            var u2 = random.NextDouble();
            var z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
            results[i] = mean + stdDev * z;
        }

        return results;
    }

    public int[] CreateHistogramBins(double[] data, int binCount)
    {
        var (min, max) = (data.Min(), data.Max());
        var binEdges = NumericRangeTools.GetRange(min, max, binCount + 1).ToArray();
        var bins = new int[binCount];

        foreach (var value in data)
        {
            for (int i = 0; i < binCount; i++)
            {
                if (value >= binEdges[i] && value < binEdges[i + 1])
                {
                    bins[i]++;
                    break;
                }
            }
        }

        return bins;
    }
}
```

## Performance Considerations

- Uses modern .NET generic constraints for compile-time safety
- Efficient range generation with minimal allocations
- Optimized interpolation algorithms
- Supports all numeric types implementing `INumber<T>`

## Thread Safety

- All static methods are thread-safe
- No shared state between operations
- Safe for concurrent usage across multiple threads

## Best Practices

1. **Type Safety**: Use appropriate numeric types for your precision requirements
2. **Range Validation**: Validate input ranges before generating sequences
3. **Memory Management**: Consider memory usage for large range generations
4. **Precision**: Be aware of floating-point precision limitations
5. **Performance**: Use appropriate step sizes and point counts for your use case
