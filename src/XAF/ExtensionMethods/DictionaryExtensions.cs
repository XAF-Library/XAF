using System.Runtime.InteropServices;

namespace XAF.Utilities.ExtensionMethods;
public static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
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

    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        where TKey : notnull
    {
        ref var valOrNew = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var exists);
        if (!exists)
        {
            valOrNew = defaultValue;
        }

        return valOrNew;
    }

    public static void Add<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, TKey key, TValue value)
        where TCollection : ICollection<TValue>, new()
        where TKey: notnull
    {
        var coll = dictionary.GetOrAdd(key);
        coll.Add(value);
    }


    public static void Add<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, IEnumerable<TKey> keys, TValue value)
        where TCollection : ICollection<TValue>, new()
        where TKey : notnull
    {
        foreach (var key in keys)
        {
            dictionary.Add(key, value);
        }
    }

    public static IEnumerable<TValue> GetOrEmpty<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, TKey key)
        where TCollection : IEnumerable<TValue>
        where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var value) 
            ? value : 
            Enumerable.Empty<TValue>();
    }
}
