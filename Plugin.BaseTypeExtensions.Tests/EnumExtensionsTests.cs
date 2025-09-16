using System.ComponentModel;
using Plugin.BaseTypeExtensions;

public class EnumExtensionsTests
{
    [Flags]
    private enum ByteFlag : byte
    {
        None = 0,
        First = 1,
        Second = 2,
    }

    [Flags]
    private enum ShortFlag : short
    {
        None = 0,
        First = 1,
        Second = 2,
    }

    [Flags]
    private enum IntFlag
    {
        None = 0,
        First = 1,
        Second = 2,
    }

    [Flags]
    private enum LongFlag : long
    {
        None = 0,
        First = 1,
        Second = 2,
    }

    private enum TestEnum
    {
        [Description("First Value")]
        First,
        Second
    }

    [Fact]
    public void TweakFlag_SupportsAllUnderlyingTypes()
    {
        ByteFlag.None.SetFlag(ByteFlag.First).Should().Be(ByteFlag.First);
        ShortFlag.First.UnsetFlag(ShortFlag.First).Should().Be(ShortFlag.None);
        IntFlag.None.SetFlag(IntFlag.Second).Should().Be(IntFlag.Second);
        LongFlag.None.SetFlag(LongFlag.First).Should().Be(LongFlag.First);
    }

    [Fact]
    public void AllAsArray_ReturnsDefinedValues()
    {
        EnumExtensions.AllAsArray<TestEnum>().Should().Equal(TestEnum.First, TestEnum.Second);
    }

    [Fact]
    public void GetDescription_ReturnsDescriptionOrName()
    {
        TestEnum.First.GetDescription().Should().Be("First Value");
        TestEnum.Second.GetDescription().Should().Be("Second");
    }

    [Fact]
    public void Count_ValidatesEnumType()
    {
        typeof(TestEnum).Count().Should().Be(2);
        Action action = () => typeof(string).Count();
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AllAsFlag_CombinesFlags()
    {
        EnumExtensions.AllAsFlag<IntFlag>().Should().Be(IntFlag.First | IntFlag.Second);
    }
}
