namespace Plugin.BaseTypeExtensions;

/// <summary>
/// Provides extension methods for working with <see cref="IDictionary{TKey, TValue}"/> and related dictionary types.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    ///     Adds a key-value pair to the dictionary.
    /// </summary>
    /// <param name="output">The dictionary to add the key-value pair to. Cannot be null.</param>
    /// <param name="kvp">The key-value pair to add.</param>
    public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> output, KeyValuePair<TKey, TValue> kvp)
    {
        ArgumentNullException.ThrowIfNull(output);
        output.Add(kvp.Key, kvp.Value);
    }

    /// <summary>
    ///     Removes a key-value pair from the dictionary.
    /// </summary>
    /// <param name="output">The dictionary to remove the key-value pair from. Cannot be null.</param>
    /// <param name="kvp">The key-value pair to remove.</param>
    public static void Remove<TKey, TValue>(this IDictionary<TKey, TValue> output, KeyValuePair<TKey, TValue> kvp)
    {
        ArgumentNullException.ThrowIfNull(output);
        output.Remove(kvp.Key);
    }

    /// <summary>
    ///     Updates the value of a key-value pair in the dictionary.
    /// </summary>
    /// <param name="output">The dictionary to update the key-value pair in. Cannot be null.</param>
    /// <param name="kvp">The key-value pair to update.</param>
    public static void Update<TKey, TValue>(this IDictionary<TKey, TValue> output, KeyValuePair<TKey, TValue> kvp)
    {
        ArgumentNullException.ThrowIfNull(output);
        output[kvp.Key] = kvp.Value;
    }

    /// <summary>
    ///     Gets the values for the specified keys from the dictionary.
    /// </summary>
    public static IEnumerable<TValue> GetValuesForKeys<TValue, TKey>(this IDictionary<TKey, TValue> source, IEnumerable<TKey> keys)
    {
        return keys.Select(key => source[key]);
    }

    /// <summary>
    ///     Updates the output collection from the input collection with optional actions for adding, updating, and removing items.
    /// </summary>
    public static void UpdateFrom<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> output,
        IEnumerable<KeyValuePair<TKey, TValue>> input,
        Action<TKey, TValue>? addAction = null,
        Action<TKey, TValue>? updateAction = null,
        Func<TKey, bool>? removeAction = null
    ) where TKey : notnull
    {
        output.UpdateFrom(
            input: input,

            addAction: addAction,
            updateAction: updateAction,
            removeAction: removeAction,

            areRepresentingTheSameKey: (iK, oK) => iK.Equals(oK),
            areRepresentingTheSameValue: (iV, oV) => iV != null && iV.Equals(oV),

            fromKeyInputTypeToKeyOutputTypeConversion: k => k,
            fromValueInputTypeToValueOutputTypeConversion: v => v
        );
    }

    /// <summary>
    ///     Updates the output collection from the input collection with key conversion and optional actions for adding, updating, and removing items.
    /// </summary>
    public static void UpdateFrom<TKey, TValueInput, TValueOutput>(this IEnumerable<KeyValuePair<TKey, TValueOutput>> output,
        IEnumerable<KeyValuePair<TKey, TValueInput>> input,
        Func<TValueInput, TValueOutput, bool> areRepresentingTheSameValue,
        Func<TValueInput, TValueOutput> fromValueInputTypeToValueOutputTypeConversion,
        Action<TKey, TValueOutput>? addAction = null,
        Action<TKey, TValueOutput>? updateAction = null,
        Func<TKey, bool>? removeAction = null) where TKey : notnull
    {
        output.UpdateFrom(input, (iK, oK) => iK.Equals(oK), k => k, areRepresentingTheSameValue,
            fromValueInputTypeToValueOutputTypeConversion, addAction, updateAction, removeAction);
    }

    /// <summary>
    ///     Updates the output collection from the input collection with key and value conversion and optional actions for adding, updating, and removing items.
    /// </summary>
    public static void UpdateFrom<TKeyInput, TKeyOutput, TValue>(this IEnumerable<KeyValuePair<TKeyOutput, TValue>> output,
        IEnumerable<KeyValuePair<TKeyInput, TValue>> input,
        Func<TKeyInput, TKeyOutput, bool> areRepresentingTheSameKey,
        Func<TKeyInput, TKeyOutput> fromKeyInputTypeToKeyOutputTypeConversion,
        Action<TKeyOutput, TValue>? addAction = null,
        Action<TKeyOutput, TValue>? updateAction = null,
        Func<TKeyOutput, bool>? removeAction = null) where TKeyOutput : notnull
        where TKeyInput : notnull
    {
        output.UpdateFrom(input, areRepresentingTheSameKey, fromKeyInputTypeToKeyOutputTypeConversion, (iV, oV) => iV != null && iV.Equals(oV),
            v => v, addAction, updateAction, removeAction);
    }

    /// <summary>
    ///     Updates the output collection from the input collection with key and value comparison and optional actions for adding, updating, and removing items.
    /// </summary>
    public static void UpdateFrom<TKeyInput, TValueInput, TKeyOutput, TValueOutput>(this IEnumerable<KeyValuePair<TKeyOutput, TValueOutput>> output,
        IEnumerable<KeyValuePair<TKeyInput, TValueInput>> input,
        Func<TKeyInput, TKeyOutput> fromKeyInputTypeToKeyOutputTypeConversion,
        Func<TValueInput, TValueOutput> fromValueInputTypeToValueOutputTypeConversion,
        Action<TKeyOutput, TValueOutput>? addAction = null,
        Action<TKeyOutput, TValueOutput>? updateAction = null,
        Func<TKeyOutput, bool>? removeAction = null) where TValueInput : IEquatable<TValueOutput>
        where TKeyInput : IEquatable<TKeyOutput>
        where TKeyOutput : notnull
    {
        output.UpdateFrom(input, (iK, oK) => iK.Equals(oK), fromKeyInputTypeToKeyOutputTypeConversion, (iV, oV) => iV.Equals(oV),
            fromValueInputTypeToValueOutputTypeConversion, addAction, updateAction, removeAction);
    }

    /// <summary>
    ///     Updates the output collection from the input collection with key and value comparison and optional actions for adding, updating, and removing items.
    /// </summary>
    public static void UpdateFrom<TKeyInput, TValueInput, TKeyOutput, TValueOutput>(this IEnumerable<KeyValuePair<TKeyOutput, TValueOutput>> output,
        IEnumerable<KeyValuePair<TKeyInput, TValueInput>> input,
        Func<TKeyInput, TKeyOutput, bool> areRepresentingTheSameKey,
        Func<TKeyInput, TKeyOutput> fromKeyInputTypeToKeyOutputTypeConversion,
        Func<TValueInput, TValueOutput> fromValueInputTypeToValueOutputTypeConversion,
        Action<TKeyOutput, TValueOutput>? addAction = null,
        Action<TKeyOutput, TValueOutput>? updateAction = null,
        Func<TKeyOutput, bool>? removeAction = null) where TValueInput : IEquatable<TValueOutput>
        where TKeyOutput : notnull
        where TKeyInput : notnull
    {
        output.UpdateFrom(input, areRepresentingTheSameKey, fromKeyInputTypeToKeyOutputTypeConversion, (iV, oV) => iV.Equals(oV),
            fromValueInputTypeToValueOutputTypeConversion, addAction, updateAction, removeAction);
    }

    /// <summary>
    ///     Updates the output collection from the input collection with key and value comparison and optional actions for adding, updating, and removing items.
    /// </summary>
    public static void UpdateFrom<TKeyInput, TValueInput, TKeyOutput, TValueOutput>(this IEnumerable<KeyValuePair<TKeyOutput, TValueOutput>> output,
        IEnumerable<KeyValuePair<TKeyInput, TValueInput>> input,
        Func<TKeyInput, TKeyOutput> fromKeyInputTypeToKeyOutputTypeConversion,
        Func<TValueInput, TValueOutput, bool> areRepresentingTheSameValue,
        Func<TValueInput, TValueOutput> fromValueInputTypeToValueOutputTypeConversion,
        Action<TKeyOutput, TValueOutput>? addAction = null,
        Action<TKeyOutput, TValueOutput>? updateAction = null,
        Func<TKeyOutput, bool>? removeAction = null) where TKeyInput : IEquatable<TKeyOutput>
        where TKeyOutput : notnull
    {
        output.UpdateFrom(input, (iK, oK) => iK.Equals(oK), fromKeyInputTypeToKeyOutputTypeConversion, areRepresentingTheSameValue,
            fromValueInputTypeToValueOutputTypeConversion, addAction, updateAction, removeAction);
    }

    /// <summary>
    /// Updates the output collection from the input collection with custom key and value comparison, conversion, and optional actions for adding, updating, and removing items.
    /// </summary>
    /// <typeparam name="TKeyInput">The type of the input dictionary keys.</typeparam>
    /// <typeparam name="TValueInput">The type of the input dictionary values.</typeparam>
    /// <typeparam name="TKeyOutput">The type of the output dictionary keys.</typeparam>
    /// <typeparam name="TValueOutput">The type of the output dictionary values.</typeparam>
    /// <param name="output">The output collection to update.</param>
    /// <param name="input">The input collection to use for updates.</param>
    /// <param name="areRepresentingTheSameKey">Function to determine if two keys represent the same key.</param>
    /// <param name="fromKeyInputTypeToKeyOutputTypeConversion">Function to convert input keys to output keys.</param>
    /// <param name="areRepresentingTheSameValue">Function to determine if two values represent the same value.</param>
    /// <param name="fromValueInputTypeToValueOutputTypeConversion">Function to convert input values to output values.</param>
    /// <param name="addAction">Action to perform when adding an item.</param>
    /// <param name="updateAction">Action to perform when updating an item.</param>
    /// <param name="removeAction">Function to perform when removing an item.</param>
    public static void UpdateFrom<TKeyInput, TValueInput, TKeyOutput, TValueOutput>(
        this IEnumerable<KeyValuePair<TKeyOutput, TValueOutput>> output,
        IEnumerable<KeyValuePair<TKeyInput, TValueInput>> input,
        Func<TKeyInput, TKeyOutput, bool> areRepresentingTheSameKey,
        Func<TKeyInput, TKeyOutput> fromKeyInputTypeToKeyOutputTypeConversion,
        Func<TValueInput, TValueOutput, bool> areRepresentingTheSameValue,
        Func<TValueInput, TValueOutput> fromValueInputTypeToValueOutputTypeConversion,
        Action<TKeyOutput, TValueOutput>? addAction = null,
        Action<TKeyOutput, TValueOutput>? updateAction = null,
        Func<TKeyOutput, bool>? removeAction = null
    ) where TKeyInput : notnull
        where TKeyOutput : notnull
    {
        // Validate inputs
        if (output is IDictionary<TKeyOutput, TValueOutput> outputAsDictionary)
        {
            addAction ??= outputAsDictionary.Add;
            updateAction ??= (keyOutput, valueOutput) => outputAsDictionary[keyOutput] = valueOutput;
            removeAction ??= outputAsDictionary.Remove;
        }
        else
        {
            ArgumentNullException.ThrowIfNull(addAction);
            ArgumentNullException.ThrowIfNull(updateAction);
            ArgumentNullException.ThrowIfNull(removeAction);
        }

        ArgumentNullException.ThrowIfNull(areRepresentingTheSameKey);
        ArgumentNullException.ThrowIfNull(fromKeyInputTypeToKeyOutputTypeConversion);
        ArgumentNullException.ThrowIfNull(areRepresentingTheSameValue);
        ArgumentNullException.ThrowIfNull(fromValueInputTypeToValueOutputTypeConversion);

        // Prepare work collections

        var keyValuePairs = output as KeyValuePair<TKeyOutput, TValueOutput>[] ?? output.ToArray();
        var outputDictionary = new Dictionary<TKeyOutput, TValueOutput>(keyValuePairs.ToDictionary(pair => pair.Key, pair => pair.Value));
        var inputDictionary = new Dictionary<TKeyInput, TValueInput>(input.ToDictionary(pair => pair.Key, pair => pair.Value));

        var (toBeAdded, toBeUpdated, toBeRemoved) = CompareDictionaries(areRepresentingTheSameKey, areRepresentingTheSameValue, inputDictionary, outputDictionary);

        lock (keyValuePairs)
        {
            foreach (var itemToBeAdded in toBeAdded)
            {
                var outputKey = fromKeyInputTypeToKeyOutputTypeConversion.Invoke(itemToBeAdded.Key);
                var outputValue = fromValueInputTypeToValueOutputTypeConversion.Invoke(itemToBeAdded.Value);
                addAction(outputKey, outputValue);
            }

            foreach (var itemToBeRemoved in toBeRemoved)
            {
                removeAction(itemToBeRemoved);
            }

            foreach (var itemToBeUpdated in toBeUpdated)
            {
                var outputValue = fromValueInputTypeToValueOutputTypeConversion.Invoke(itemToBeUpdated.Value);
                updateAction(itemToBeUpdated.Key, outputValue);
            }
        }
    }

    /// <summary>
    ///     Compares two dictionaries and determines which items need to be added, updated, or removed.
    /// </summary>
    /// <typeparam name="TKeyInput">The type of the input dictionary keys.</typeparam>
    /// <typeparam name="TValueInput">The type of the input dictionary values.</typeparam>
    /// <typeparam name="TKeyOutput">The type of the output dictionary keys.</typeparam>
    /// <typeparam name="TValueOutput">The type of the output dictionary values.</typeparam>
    /// <param name="areRepresentingTheSameKey">Function to determine if two keys represent the same key.</param>
    /// <param name="areRepresentingTheSameValue">Function to determine if two values represent the same value.</param>
    /// <param name="inputDictionary">The input dictionary.</param>
    /// <param name="outputDictionary">The output dictionary.</param>
    /// <returns>A tuple containing dictionaries of items to be added, updated, and a list of keys to be removed.</returns>
    private static (Dictionary<TKeyInput, TValueInput> toBeAdded, Dictionary<TKeyOutput, TValueInput> toBeUpdated, List<TKeyOutput> toBeRemoved) CompareDictionaries<TKeyInput, TValueInput, TKeyOutput, TValueOutput>(
        Func<TKeyInput, TKeyOutput, bool> areRepresentingTheSameKey,
        Func<TValueInput, TValueOutput, bool> areRepresentingTheSameValue,
        Dictionary<TKeyInput, TValueInput> inputDictionary,
        Dictionary<TKeyOutput, TValueOutput> outputDictionary
    )
        where TKeyInput : notnull
        where TKeyOutput : notnull
    {
        var toBeAdded = new Dictionary<TKeyInput, TValueInput>();
        var toBeUpdated = new Dictionary<TKeyOutput, TValueInput>();
        var toBeRemoved = new List<TKeyOutput>(outputDictionary.Keys);
        var processedOutputKeys = new HashSet<TKeyOutput>();

        foreach (var inputKvp in inputDictionary)
        {
            var found = false;
            TKeyOutput? matchedKey = default;

            foreach (var outputKey in outputDictionary.Keys.Where(outputKey => areRepresentingTheSameKey(inputKvp.Key, outputKey)))
            {
                found = true;
                matchedKey = outputKey;
                processedOutputKeys.Add(outputKey);
                break;
            }

            if (!found)
            {
                // OutputDictionary does not contain matching Key, will be added
                toBeAdded.Add(inputKvp);
            }
            else if (matchedKey != null && !areRepresentingTheSameValue(inputKvp.Value, outputDictionary[matchedKey]))
            {
                // OutputDictionary contains matching Key but values are different, will be updated
                toBeUpdated.Add(matchedKey, inputKvp.Value);
            }
        }

        // Only keep the keys in toBeRemoved that weren't processed
        toBeRemoved.RemoveAll(key => processedOutputKeys.Contains(key));

        return (toBeAdded, toBeUpdated, toBeRemoved);
    }
}
