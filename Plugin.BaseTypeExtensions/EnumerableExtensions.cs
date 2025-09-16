using System.Collections;
using System.Collections.Concurrent;

namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for working with <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>Coalesces the input to null if it is empty.</summary>
    /// <param name="input">The input sequence to coalesce. Can be null.</param>
    /// <returns>The input sequence that was passed in if it is not empty; otherwise, null.</returns>
    public static IEnumerable? NullIfEmpty(this IEnumerable? input)
    {
        return input == null || !input.Cast<object?>().Any()
            ? null
            : input;
    }

    /// <summary>Coalesces the input to null if it is empty.</summary>
    /// <param name="input">The input sequence to coalesce. Can be null.</param>
    /// <returns>The input sequence that was passed in if it is not empty; otherwise, null.</returns>
    public static IEnumerable<T?>? NullIfEmpty<T>(this IEnumerable<T?>? input)
    {
        return input == null || !input.Any()
            ? null
            : input;
    }

    /// <summary>
    ///     Enqueues an item to the queue and ensures the queue does not exceed the specified maximum size.
    /// </summary>
    /// <param name="queue">The queue to enqueue the item to. Cannot be null.</param>
    /// <param name="obj">The item to enqueue. Can be null.</param>
    /// <param name="max">The maximum size of the queue.</param>
    public static void Enqueue<T>(this ConcurrentQueue<T> queue, T obj, int max)
    {
        ArgumentNullException.ThrowIfNull(queue);

        queue.Enqueue(obj);
        while (queue.Count > max)
        {
            queue.TryDequeue(out var _);
        }
    }

    /// <summary>
    ///     Gets the element at the specified index or returns the default value if the index is out of range.
    /// </summary>
    /// <param name="enumerable">The enumerable to get the element from. Can be null.</param>
    /// <param name="index">The index of the element to get. If the index is out of range or if the enumerable is null, the default value is returned.</param>
    /// <param name="defaultValue">The default value to return if the index is out of range. Can be null.</param>
    public static T? GetOrDefault<T>(this IEnumerable<T?>? enumerable, int index, T? defaultValue = default)
    {
        if (enumerable is null || index < 0)
        {
            return defaultValue;
        }

        try
        {
            var enumeratedList = enumerable as IList<T?> ?? enumerable.ToList();
            if (index >= enumeratedList.Count)
            {
                return defaultValue;
            }

            return enumeratedList[index];
        }
        catch (ArgumentOutOfRangeException)
        {
            return defaultValue;
        }
        catch (InvalidCastException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    ///     Updates the output collection from the input collection with item comparison and actions for adding and removing items.
    /// </summary>
    public static void UpdateFrom<TInput, TOutput>(
        this IEnumerable<TOutput> output,
        IEnumerable<TInput> input,
        Func<TInput, TOutput, bool> areRepresentingTheSameItem,
        Action<TInput> addAction,
        Action<TOutput> removeAction
    )
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(areRepresentingTheSameItem);
        ArgumentNullException.ThrowIfNull(addAction);
        ArgumentNullException.ThrowIfNull(removeAction);

        var inputList = input as TInput[] ?? input.ToArray();

        lock (output)
        {
            // Take a fresh snapshot of the output inside the lock
            var outputList = new List<TOutput>(output);

            var toBeAdded = new List<TInput>();
            var toBeRemoved = new List<TOutput>();

            // Find items to add (present in input but not in output)
            foreach (var inputItem in inputList)
            {
                var found = false;
                foreach (var outputItem in outputList)
                {
                    if (areRepresentingTheSameItem(inputItem, outputItem))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    toBeAdded.Add(inputItem);
                }
            }

            // Find items to remove (present in output but not in input)
            foreach (var outputItem in outputList)
            {
                var found = false;
                foreach (var inputItem in inputList)
                {
                    if (areRepresentingTheSameItem(inputItem, outputItem))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    toBeRemoved.Add(outputItem);
                }
            }

            // Apply changes while still holding the lock
            foreach (var itemToBeAdded in toBeAdded)
            {
                addAction(itemToBeAdded);
            }

            foreach (var itemToBeRemoved in toBeRemoved)
            {
                removeAction(itemToBeRemoved);
            }
        }
    }

    /// <summary>
    ///     Picks a random element from the collection.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the source collection is empty.</exception>
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        var sourceArray = source as T[] ?? source.ToArray();
        if (sourceArray.Length == 0)
        {
            throw new InvalidOperationException("Cannot pick a random element from an empty collection");
        }

        return sourceArray.PickRandom(1).Single();
    }

    /// <summary>
    ///     Picks a specified number of random elements from the collection.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if requested count exceeds the source collection count.</exception>
    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    /// <summary>
    ///     Shuffles the elements of the collection using Fisher-Yates algorithm.
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var sourceArray = source.ToArray();

        // Fisher-Yates shuffle algorithm
        for (var i = sourceArray.Length - 1; i > 0; i--)
        {
#pragma warning disable CA5394 // Do not use insecure randomness
            var j = Random.Shared.Next(i + 1);
#pragma warning restore CA5394 // Do not use insecure randomness
            (sourceArray[j], sourceArray[i]) = (sourceArray[i], sourceArray[j]);
        }

        return sourceArray;
    }
}
