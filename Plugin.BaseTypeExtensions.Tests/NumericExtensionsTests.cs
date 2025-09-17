namespace Plugin.BaseTypeExtensions.Tests;

public class NumericExtensionsTests
{
    [Fact]
    public void DegreeToRadian_ConvertsCorrectly()
    {
        180.0.DegreeToRadian().Should().BeApproximately(Math.PI, 1e-10);
    }

    [Fact]
    public void RadianToDegree_ConvertsCorrectly()
    {
        Math.PI.RadianToDegree().Should().BeApproximately(180.0, 1e-10);
    }

    [Fact]
    public void PercentageToValue_RespectsBounds()
    {
        0.5.PercentageToValue(0.0, 10.0).Should().Be(5.0);
        1.5.PercentageToValue(0.0, 10.0).Should().Be(10.0);
        (-0.5).PercentageToValue(0.0, 10.0).Should().Be(0.0);
    }

    [Fact]
    public void PercentageToValue_ThrowsForInvalidBounds()
    {
        Action action = () => 0.5.PercentageToValue(10.0, 5.0);
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ValueToPercentage_ComputesExpectedRatio()
    {
        7.0.ValueToPercentage(2.0, 10.0).Should().Be(0.625);
    }

    [Fact]
    public void ValueToPercentage_UsesClampsAndValidations()
    {
        1.0.ValueToPercentage(2.0, 10.0).Should().Be(0);
        11.0.ValueToPercentage(2.0, 10.0).Should().Be(1);

        Action invalidRange = () => 1.0.ValueToPercentage(10.0, 2.0);
        invalidRange.Should().Throw<ArgumentOutOfRangeException>();

        Action equalBounds = () => 5.0.ValueToPercentage(5.0, 5.0);
        equalBounds.Should().Throw<DivideByZeroException>();
    }

    [Fact]
    public void ToByte_ClampsAndRounds()
    {
        (-5).ToByte().Should().Be((byte)0);
        300.ToByte().Should().Be((byte)255);
        12.6.ToByte().Should().Be((byte)13);
    }

    [Fact]
    public void PercentageToByte_UsesNumericConversions()
    {
        0.5.PercentageToByte().Should().Be((byte)128);
    }

    [Fact]
    public void Clamp_ValidatesBounds()
    {
        5.Clamp(0, 10).Should().Be(5);

        Action invalid = () => 5.Clamp(10, 0);
        invalid.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetInvertedSafe_ReturnsExpectedValues()
    {
        4.GetInvertedSafe(isInverted: true, maxValue: 10).Should().Be(4);
        4.GetInvertedSafe(isInverted: false, maxValue: 10).Should().Be(6);
    }
}
