# DictionaryExtensions

Provides enhanced dictionary operations and safety utilities.

## Overview

The `DictionaryExtensions` class offers advanced dictionary operations including safe access, bulk operations, and dictionary comparison utilities.

## API Reference

<xref:Plugin.BaseTypeExtensions.DictionaryExtensions>

## Core Methods

### Safe Access Operations

#### `GetValueOrDefault<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)`
Safely gets value from dictionary or returns default if key doesn't exist.

```csharp
var settings = new Dictionary<string, string>
{
    ["theme"] = "dark",
    ["language"] = "en"
};

var theme = settings.GetValueOrDefault("theme", "light"); // "dark"
var timeout = settings.GetValueOrDefault("timeout", "30"); // "30" (default)
```

#### `TryAdd<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)`
Adds key-value pair only if key doesn't already exist.

```csharp
var config = new Dictionary<string, object>();
config.TryAdd("setting1", "value1"); // true - added
config.TryAdd("setting1", "value2"); // false - not added (key exists)
```

### Bulk Operations

#### `AddRange<TKey, TValue>(Dictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)`
Adds multiple key-value pairs to the dictionary.

```csharp
var mainDict = new Dictionary<string, int> { ["a"] = 1 };
var additionalItems = new[]
{
    new KeyValuePair<string, int>("b", 2),
    new KeyValuePair<string, int>("c", 3)
};

mainDict.AddRange(additionalItems);
// Result: {"a": 1, "b": 2, "c": 3}
```

#### `RemoveRange<TKey, TValue>(Dictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)`
Removes multiple keys from the dictionary.

```csharp
var dict = new Dictionary<string, int>
{
    ["a"] = 1, ["b"] = 2, ["c"] = 3, ["d"] = 4
};

dict.RemoveRange(new[] { "b", "d" });
// Result: {"a": 1, "c": 3}
```

### Dictionary Comparison

#### `CompareDictionaries<TKey, TValue>(Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> target)`
Compares two dictionaries and returns differences.

```csharp
var oldConfig = new Dictionary<string, string>
{
    ["setting1"] = "value1",
    ["setting2"] = "value2"
};

var newConfig = new Dictionary<string, string>
{
    ["setting1"] = "updated_value1",
    ["setting3"] = "value3"
};

var comparison = oldConfig.CompareDictionaries(newConfig);
// Returns: Added: ["setting3"], Removed: ["setting2"], Modified: ["setting1"]
```

## Advanced Operations

### Conditional Updates

#### `UpdateIf<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue newValue, Func<TValue, bool> condition)`
Updates dictionary value only if condition is met.

```csharp
var counters = new Dictionary<string, int> { ["clicks"] = 5 };

// Only update if current value is less than 10
counters.UpdateIf("clicks", 7, currentValue => currentValue < 10); // Updated
counters.UpdateIf("clicks", 15, currentValue => currentValue < 10); // Not updated
```

### Transformation Operations

#### `MapValues<TKey, TValue, TNewValue>(Dictionary<TKey, TValue> dictionary, Func<TValue, TNewValue> mapper)`
Transforms all values in the dictionary using the provided mapper function.

```csharp
var numbers = new Dictionary<string, int>
{
    ["a"] = 1, ["b"] = 2, ["c"] = 3
};

var doubled = numbers.MapValues(x => x * 2);
// Result: {"a": 2, "b": 4, "c": 6}
```

## Practical Examples

### Configuration Management

```csharp
public class ConfigurationManager
{
    private readonly Dictionary<string, object> _settings = new();

    public void LoadDefaults()
    {
        var defaults = new Dictionary<string, object>
        {
            ["timeout"] = 30,
            ["theme"] = "light",
            ["language"] = "en"
        };

        _settings.AddRange(defaults.Where(kvp => !_settings.ContainsKey(kvp.Key)));
    }

    public T GetSetting<T>(string key, T defaultValue = default)
    {
        return (T)_settings.GetValueOrDefault(key, defaultValue);
    }

    public void UpdateSettings(Dictionary<string, object> newSettings)
    {
        var changes = _settings.CompareDictionaries(newSettings);
        LogConfigurationChanges(changes);

        _settings.AddRange(newSettings);
    }
}
```

### Cache Management

```csharp
public class CacheManager<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, (TValue Value, DateTime Expiry)> _cache = new();

    public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, TimeSpan expiry)
    {
        if (_cache.TryGetValue(key, out var cached) && cached.Expiry > DateTime.UtcNow)
        {
            return cached.Value;
        }

        var value = valueFactory();
        _cache[key] = (value, DateTime.UtcNow.Add(expiry));
        return value;
    }

    public void CleanupExpired()
    {
        var expiredKeys = _cache
            .Where(kvp => kvp.Value.Expiry <= DateTime.UtcNow)
            .Select(kvp => kvp.Key)
            .ToList();

        _cache.RemoveRange(expiredKeys);
    }
}
```

## Performance Considerations

- Bulk operations are more efficient than individual operations
- Dictionary lookups are O(1) on average
- Comparison operations enumerate both dictionaries once
- Memory-efficient operations that minimize allocations

## Thread Safety

- Extension methods are thread-safe but Dictionary<TKey, TValue> itself is not
- Use ConcurrentDictionary<TKey, TValue> for thread-safe scenarios
- Consider locking when modifying dictionaries from multiple threads

## Best Practices

1. **Use Safe Access**: Prefer `GetValueOrDefault()` over direct indexing
2. **Bulk Operations**: Use bulk operations for multiple additions/removals
3. **Key Validation**: Ensure keys are not null (for reference types)
4. **Memory Management**: Remove unused entries to prevent memory leaks
5. **Thread Safety**: Use appropriate synchronization for concurrent access
