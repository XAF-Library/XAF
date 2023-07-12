using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Utilities.ExtensionMethods;
public static class DictionaryExtensions
{
    public static TValue GetOrAddNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TValue : new()
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = new TValue();
            dictionary.Add(key, value);
        }

        return value;
    }

    public static void Add<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, TKey key, TValue value)
        where TCollection : ICollection<TValue>, new()
    {
        var coll = dictionary.GetOrAddNew(key);
        coll.Add(value);
    }

    public static void Add<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, IEnumerable<TKey> keys, TValue value)
        where TCollection : ICollection<TValue>, new()
    {
        foreach (var key in keys)
        {
            dictionary.Add(key, value);
        }
    }

    public static IEnumerable<TValue> GetOrEmpty<TKey, TCollection, TValue>(this Dictionary<TKey, TCollection> dictionary, TKey key)
        where TCollection : IEnumerable<TValue>
    {
        return dictionary.TryGetValue(key, out var value) 
            ? value : 
            Enumerable.Empty<TValue>();
    }
}
