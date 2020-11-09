using System;
using System.Collections.Generic;

namespace Miru
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }
        
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> add)
        {
            if (dictionary.TryGetValue(key, out var fake))
                return fake;

            fake = add();
            dictionary.Add(key, fake);
            return fake;
        }

        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary.Count == 0;
        }
    }
}
