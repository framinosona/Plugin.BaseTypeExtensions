using Plugin.BaseTypeExtensions;

public class NumericRangeToolsTests
{
    [Fact]
    public void GetRange_GenericHandlesZeroStep()
    {
        NumericRangeTools.GetRange(0, 10, 0).Should().BeEmpty();
    }

    [Fact]
    public void GetRange_GenericProducesSequence()
    {
        NumericRangeTools.GetRange(1, 3, 1).Should().Equal(1, 2, 3);
    }

    [Fact]
    public void GetRange_DoubleProducesSequence()
    {
        NumericRangeTools.GetRange(0.0, 0.6, 0.3).Should().Equal(0.0, 0.3);
    }

    [Fact]
    public void GetRange_FloatProducesSequence()
    {
        NumericRangeTools.GetRange(0f, 0.6f, 0.3f).Should().Equal(0f, 0.3f);
    }

    [Fact]
    public void GetRange_DateTimeProducesSequence()
    {
        var start = new DateTime(2024, 1, 1);
        var step = TimeSpan.FromDays(1);
        NumericRangeTools.GetRange(start, start.AddDays(2), step).Should().Equal(start, start.AddDays(1));
    }
}
