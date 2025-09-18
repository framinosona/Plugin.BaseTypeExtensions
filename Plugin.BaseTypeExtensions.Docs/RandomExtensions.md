# RandomExtensions

Provides enhanced random number generation and utilities.

## Overview

The `RandomExtensions` class offers advanced random number generation including ranges, collections, and specialized random operations.

## API Reference

<xref:Plugin.BaseTypeExtensions.RandomExtensions>

## Core Methods

### Range Generation

#### `NextDouble(Random random, double min, double max)`
Generates a random double within a specified range.

```csharp
var random = new Random();
var temperature = random.NextDouble(-10.0, 40.0); // Random temperature
var percentage = random.NextDouble(0.0, 1.0); // Random percentage
```

#### `NextFloat(Random random, float min, float max)`
Generates a random float within a specified range.

```csharp
var random = new Random();
var speed = random.NextFloat(0.0f, 100.0f); // Random speed
```

### Boolean and Selection

#### `NextBool(Random random, double probability = 0.5)`
Generates a random boolean with optional probability bias.

```csharp
var random = new Random();
var coinFlip = random.NextBool(); // 50/50 chance
var biased = random.NextBool(0.7); // 70% chance of true
```

#### `Choose<T>(Random random, params T[] items)`
Randomly selects an item from the provided options.

```csharp
var random = new Random();
var color = random.Choose("Red", "Green", "Blue", "Yellow");
var number = random.Choose(1, 5, 10, 25, 50);
```

### Collection Operations

#### `Shuffle<T>(Random random, IList<T> list)`
Randomly shuffles a list in-place using Fisher-Yates algorithm.

```csharp
var cards = new List<string> { "Ace", "King", "Queen", "Jack" };
var random = new Random();
random.Shuffle(cards); // Cards are now in random order
```

#### `Sample<T>(Random random, IEnumerable<T> source, int count)`
Randomly samples items from a collection without replacement.

```csharp
var numbers = Enumerable.Range(1, 100);
var random = new Random();
var sample = random.Sample(numbers, 5); // 5 random numbers from 1-100
```

## Advanced Operations

### String Generation

#### `NextString(Random random, int length, string chars = "...")`
Generates a random string of specified length.

```csharp
var random = new Random();
var password = random.NextString(12); // Random 12-character string
var code = random.NextString(6, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"); // Uppercase + digits
```

### Weighted Selection

#### `WeightedChoice<T>(Random random, Dictionary<T, double> weights)`
Selects an item based on weighted probabilities.

```csharp
var weights = new Dictionary<string, double>
{
    ["Common"] = 0.7,
    ["Rare"] = 0.25,
    ["Epic"] = 0.05
};

var random = new Random();
var rarity = random.WeightedChoice(weights);
```

## Practical Examples

### Game Development

```csharp
public class GameRandomizer
{
    private readonly Random _random = new();

    public Enemy SpawnRandomEnemy()
    {
        var enemyTypes = new[] { "Goblin", "Orc", "Troll", "Dragon" };
        var weights = new Dictionary<string, double>
        {
            ["Goblin"] = 0.5,
            ["Orc"] = 0.3,
            ["Troll"] = 0.15,
            ["Dragon"] = 0.05
        };

        var enemyType = _random.WeightedChoice(weights);
        var level = _random.Next(1, 10);
        var health = _random.NextDouble(50.0, 200.0);

        return new Enemy(enemyType, level, health);
    }

    public List<Item> GenerateRandomLoot(int itemCount)
    {
        var allItems = GetAllItems();
        return _random.Sample(allItems, itemCount).ToList();
    }
}
```

### Testing Data Generation

```csharp
public class TestDataGenerator
{
    private readonly Random _random = new();

    public User GenerateRandomUser()
    {
        var firstNames = new[] { "John", "Jane", "Mike", "Sarah", "David", "Lisa" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones" };

        return new User
        {
            FirstName = _random.Choose(firstNames),
            LastName = _random.Choose(lastNames),
            Age = _random.Next(18, 80),
            Email = GenerateRandomEmail(),
            IsActive = _random.NextBool(0.8) // 80% chance of being active
        };
    }

    private string GenerateRandomEmail()
    {
        var username = _random.NextString(8, "abcdefghijklmnopqrstuvwxyz");
        var domains = new[] { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" };
        var domain = _random.Choose(domains);
        return $"{username}@{domain}";
    }

    public List<User> GenerateUserList(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GenerateRandomUser())
            .ToList();
    }
}
```

## Performance Considerations

- Reuse Random instances instead of creating new ones frequently
- Fisher-Yates shuffle is O(n) and efficient for large collections
- Weighted selection is optimized for reasonable numbers of choices
- String generation creates minimal allocations

## Thread Safety

- Random class is not thread-safe
- Use ThreadLocal<Random> or Random.Shared (NET 6+) for concurrent scenarios
- Extension methods are thread-safe if Random instance is properly managed

## Best Practices

1. **Seed Management**: Use appropriate seeds for reproducible vs truly random behavior
2. **Instance Reuse**: Reuse Random instances for better performance
3. **Distribution**: Understand the distribution characteristics of different methods
4. **Testing**: Use fixed seeds for deterministic testing scenarios
