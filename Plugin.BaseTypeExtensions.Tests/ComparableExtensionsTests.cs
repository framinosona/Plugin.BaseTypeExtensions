namespace Plugin.BaseTypeExtensions.Tests;

public class ComparableExtensionsTests
{
    [Fact]
    public void IsBetween_UsesInclusiveOrExclusiveChecks()
    {
        5.IsBetween(1, 10).Should().BeTrue();
        5.IsBetween(1, 5, inclusive: false).Should().BeFalse();
    }

    [Fact]
    public void ComparisonHelpers_WorkAsExpected()
    {
        5.IsGreaterThan(3).Should().BeTrue();
        5.IsGreaterThanOrEqual(5).Should().BeTrue();
        3.IsLessThan(5).Should().BeTrue();
        3.IsLessThanOrEqual(3).Should().BeTrue();
    }

    [Fact]
    public void ComparableTools_MinMax_UseExtensions()
    {
        ComparableTools.Min(1, 2).Should().Be(1);
        ComparableTools.Max(1, 2).Should().Be(2);
    }
}
