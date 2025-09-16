using Plugin.BaseTypeExtensions;

public class RandomExtensionsTests
{
    private sealed class FixedRandom : Random
    {
        private readonly double _nextDouble;
        private readonly int _nextInt;

        public FixedRandom(double nextDouble, int nextInt)
        {
            _nextDouble = nextDouble;
            _nextInt = nextInt;
        }

        public override double NextDouble()
        {
            return _nextDouble;
        }

        public override int Next(int minValue, int maxValue)
        {
            return _nextInt;
        }
    }

    [Fact]
    public void NextBool_HandlesEdgeCases()
    {
        var random = new FixedRandom(0.4, 0);

        random.NextBool(0, 10).Should().BeFalse();
        random.NextBool(5, 0).Should().BeFalse();
        random.NextBool(5, 3).Should().BeTrue();
        random.NextBool(1, 2).Should().BeTrue();
    }

    [Fact]
    public void NextDouble_ReturnsValueInRange()
    {
        var random = new FixedRandom(0.25, 0);

        random.NextDouble(10, 20).Should().Be(12.5);
    }

    [Fact]
    public void NextByte_UsesNumericConversion()
    {
        var random = new FixedRandom(0, 100);

        random.NextByte().Should().Be(100);
    }
}
