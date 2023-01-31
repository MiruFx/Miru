using System;
using System.Collections.Generic;

namespace Miru;

public static class ListExtensions
{
    public static void Add<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> list, TKey key, TValue value)
    {
        list.Add(new KeyValuePair<TKey, TValue>(key, value));
    }
        
    public static bool IsEmpty<T>(this IList<T> list)
    {
        return list.Count == 0;
    }
        
    public static int FindIndex<T>(this IList<T> list, Predicate<T> match, int startIndex = 0)
    {
        for (int i = startIndex; i < list.Count; i++)
        {
            if (match(list[i]))
            {
                return i;
            }
        }
            
        return -1;
    }
    
    public static void AddIfNone<T>(this IList<T> items, Func<T, bool> condition, T itemToAdd)
    {
        if (items.None(condition))
            items.Add(itemToAdd);
    }
}