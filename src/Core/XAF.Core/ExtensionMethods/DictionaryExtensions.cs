using System.Runtime.InteropServices;

namespace XAF.Core.ExtensionMethods;

/// <summary>
/// Several extensions for the <see cref="Dictionary{TKey, TValue}"/>
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Get the value of the Dictionary. 
    /// If no value exists for key add a default value.
    /// </summary>
    /// <typeparam name="TKey">Type of the dictionary key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key">the key to search for</param>
    /// <returns>The founded or new value</returns>
    public static TValue GetOrAddDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
        where TValue : new()
    {
        ref var valOrNew = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var exists);
        if (!exists)
        {
            valOrNew = new TValue();
        }

        return valOrNew;
    }

    /// <summary>
    /// Get the value of the Dictionary. 
    /// If no value exists for key add a default value.
    /// </summary>
    /// <typeparam name="TKey">Type of the dictionary key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key">the key to search for</param>
    /// <param name="defaultValue">the default value to add if not existing</param>
    /// <returns>The founded or new value</returns>
    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        where TKey : notnull
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = valueFactory(key);
            dictionary.Add(key, value);
        }

        return value;
    }

    /// <summary>
    /// Add a single item to the collection inside the dictionary
    /// Adds a new collection if it doesn't exist
    /// </summary>
    /// <typeparam name="TKey">the type of the key</typeparam>
    /// <typeparam name="TCollection">the type of the collection</typeparam>
    /// <typeparam name="TValue">the type of an item inside the collection</typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key">the key for the collection</param>
    /// <param name="value">the item to add to the collection</param>
    public static void Add<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, TKey key, TValue value)
        where TCollection : ICollection<TValue>, new()
        where TKey : notnull
    {
        var coll = dictionary.GetOrAddDefault(key);
        coll.Add(value);
    }

    /// <summary>
    /// Add a single item to multiple collections inside the dictionary
    /// Adds a new collection if it doesn't exist
    /// </summary>
    /// <typeparam name="TKey">the type of the key</typeparam>
    /// <typeparam name="TCollection">the type of the collection</typeparam>
    /// <typeparam name="TValue">the type of an item inside the collection</typeparam>
    /// <param name="dictionary"></param>
    /// <param name="keys">the keys for the collection</param>
    /// <param name="value">the item to add to the collection</param>
    public static void Add<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, IEnumerable<TKey> keys, TValue value)
        where TCollection : ICollection<TValue>, new()
        where TKey : notnull
    {
        foreach (var key in keys)
        {
            dictionary.Add(key, value);
        }
    }

    /// <summary>
    /// Gets the collection for the key or returns an empty collection
    /// </summary>
    /// <typeparam name="TKey">The type of the key</typeparam>
    /// <typeparam name="TCollection">the type of the collection</typeparam>
    /// <typeparam name="TValue">the type of the value inside the collection</typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key">the key for the collection</param>
    /// <returns></returns>
    public static IEnumerable<TValue> GetOrEmpty<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, TKey key)
        where TCollection : IEnumerable<TValue>
        where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var value)
            ? value :
            Enumerable.Empty<TValue>();
    }
}
