using Plugin.BaseTypeExtensions;

public class DictionaryExtensionsTests
{
    [Fact]
    public void Add_Remove_Update_ExtendDictionary()
    {
        var dictionary = new Dictionary<string, int>();
        dictionary.Add(new KeyValuePair<string, int>("a", 1));
        dictionary.Update(new KeyValuePair<string, int>("a", 2));
        dictionary.Remove(new KeyValuePair<string, int>("a", 0));

        dictionary.Should().BeEmpty();
    }

    [Fact]
    public void GetValuesForKeys_ReturnsProjectedValues()
    {
        var dictionary = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        dictionary.GetValuesForKeys(new[] { "a", "b" }).Should().Equal(1, 2);
    }

    [Fact]
    public void UpdateFrom_WithDictionary_DefaultsToDictionaryActions()
    {
        var output = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        var input = new Dictionary<string, int> { ["a"] = 1, ["c"] = 3 };

        output.UpdateFrom(input);

        output.Should().ContainKeys("a", "c");
        output.Should().NotContainKey("b");
        output["c"].Should().Be(3);
    }

    [Fact]
    public void UpdateFrom_WithDictionary_UpdatesExistingValues()
    {
        var output = new Dictionary<string, int> { ["a"] = 1 };
        var input = new Dictionary<string, int> { ["a"] = 2 };

        output.UpdateFrom(input);

        output["a"].Should().Be(2);
    }

    [Fact]
    public void UpdateFrom_WithValueConversion_UpdatesDifferentTypes()
    {
        var output = new Dictionary<string, string> { ["a"] = "1" };
        var input = new Dictionary<string, int> { ["a"] = 2, ["b"] = 3 };

        output.UpdateFrom(
            input,
            areRepresentingTheSameValue: (src, dest) => src.ToString() == dest,
            fromValueInputTypeToValueOutputTypeConversion: value => value.ToString());

        output.Should().Contain(new KeyValuePair<string, string>("a", "2"));
        output.Should().Contain(new KeyValuePair<string, string>("b", "3"));
    }

    [Fact]
    public void UpdateFrom_WithKeyConversion_UpdatesDifferentKeys()
    {
        var output = new Dictionary<int, string> { [1] = "one" };
        var input = new Dictionary<string, string> { ["1"] = "one", ["2"] = "two" };

        output.UpdateFrom(
            input,
            areRepresentingTheSameKey: (src, dest) => src == dest.ToString(),
            fromKeyInputTypeToKeyOutputTypeConversion: key => int.Parse(key));

        output.Should().Contain(new KeyValuePair<int, string>(1, "one"));
        output.Should().Contain(new KeyValuePair<int, string>(2, "two"));
    }

    [Fact]
    public void UpdateFrom_WithFullCustomization_HandlesAddUpdateRemove()
    {
        var output = new List<KeyValuePair<string, string>>
        {
            new("1", "1"),
            new("b", "2"),
        };
        var input = new List<KeyValuePair<int, int>>
        {
            new(1, 2),
            new(3, 4),
        };

        var added = new List<(string Key, string Value)>();
        var updated = new List<(string Key, string Value)>();
        var removed = new List<string>();

        output.UpdateFrom(
            input,
            areRepresentingTheSameKey: (src, dest) => src.ToString() == dest,
            fromKeyInputTypeToKeyOutputTypeConversion: key => key.ToString(),
            areRepresentingTheSameValue: (src, dest) => src.ToString() == dest,
            fromValueInputTypeToValueOutputTypeConversion: value => value.ToString(),
            addAction: (k, v) => added.Add((k, v)),
            updateAction: (k, v) => updated.Add((k, v)),
            removeAction: key =>
            {
                removed.Add(key);
                return true;
            });

        added.Should().ContainSingle().Which.Should().Be(("3", "4"));
        updated.Should().ContainSingle().Which.Should().Be(("1", "2"));
        removed.Should().ContainSingle().Which.Should().Be("b");
    }
}
