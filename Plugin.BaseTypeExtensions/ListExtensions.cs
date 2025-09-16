namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for <see cref="IList{T}"/> to support advanced update and pairing operations.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    ///     Updates the output list from the input list with item comparison and type conversion.
    /// </summary>
    public static void UpdateFrom<TInput, TOutput>(this IList<TOutput> output, IList<TInput> input, Func<TInput, TOutput, bool> areRepresentingTheSameItem, Func<TInput, TOutput> fromInputTypeToOutputTypeConversion)
    {
        ArgumentNullException.ThrowIfNull(fromInputTypeToOutputTypeConversion);

        lock (output)
        {
            output.UpdateFrom(input, areRepresentingTheSameItem, i => output.Add(fromInputTypeToOutputTypeConversion.Invoke(i)), o => output.Remove(o));
        }
    }

    /// <summary>
    ///     Updates the output list from the input list with item comparison and type conversion.
    /// </summary>
    public static void UpdateFrom<TInput, TOutput>(this IList<TOutput> output, IList<TInput> input, Func<TInput, TOutput> fromInputTypeToOutputTypeConversion) where TOutput : IEquatable<TInput>
    {
        lock (output)
        {
            output.UpdateFrom(input, (i, o) => i != null && i.Equals(o), i => output.Add(fromInputTypeToOutputTypeConversion.Invoke(i)), o => output.Remove(o));
        }
    }

    /// <summary>
    ///     Updates the output list from the input list with item comparison.
    /// </summary>
    public static void UpdateFrom<T>(this IList<T> output, IList<T> input, Func<T, T, bool> areRepresentingTheSameItem)
    {
        output.UpdateFrom(input, areRepresentingTheSameItem, i => i);
    }

    /// <summary>
    ///     Updates the output list from the input list with item comparison.
    /// </summary>
    public static void UpdateFrom<T>(this IList<T> output, IList<T> input)
    {
        output.UpdateFrom(input, (equatable1, equatable2) => equatable1 != null && equatable1.Equals(equatable2), i => i);
    }

    /// <summary>
    ///     Returns pairs of consecutive elements from the list.
    /// </summary>
    /// <remarks>Returns an empty sequence if the input has fewer than two elements.</remarks>
    public static IEnumerable<ValueTuple<T, T>> Pairs<T>(this IList<T> input)
    {
        ArgumentNullException.ThrowIfNull(input);
        var count = input.Count;
        if (count < 2)
        {
            return [];
        }

        return input.Take(count - 1).Select((o1, i) => (o1, input.ElementAt(i + 1)));
    }
}
