using System;
using System.Collections.Generic;
using System.Linq;

namespace Miru.Core;

public static class EnumerableExtensions
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
            action(item);

        return enumerable;
    }
        
    public static string Join<T>(this IEnumerable<T> enumerable, string split)
    {
        if (enumerable == null || enumerable.None())
            return String.Empty;
                
        return enumerable
            .Join2(x => $"{x}{split}")
            .TrimEnd(split.ToCharArray());
    }

    public static string Join2<T>(this IEnumerable<T> enumerable, Func<T, string> action)
    {
        return enumerable
            .Aggregate(string.Empty, (current, item) => current + action(item));
    }

    /// <summary>
    /// Shortcut for enumerable.ElementAt(index);
    /// </summary>
    public static T At<T>(this IEnumerable<T> enumerable, int index)
    {
        return enumerable.ElementAt(index);
    }

    public static bool None<TSource>(this IEnumerable<TSource> source)
    {
        return !source.Any();
    }
        
    public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return !source.Any(predicate);
    }

    public static TList By<TList>(this IEnumerable<TList> list, TList by)
    {
        return list.FirstOrDefault(e => e.Equals(by));
    }
        
    public static T Second<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.At(1);
    }
        
    public static T Third<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.At(2);
    }
        
    public static T Fourth<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.At(3);
    }
    
    public static T Fifth<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.At(4);
    }
    
    public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> self, int startAt = 0)
    {
        return self.Select((item, index) => (item, index + startAt));
    }
    
    // TODO: uncomment when remove Baseline
    // public static string Join(this string[] values, string separator) => 
    //     string.Join(separator, values);
    //
    // public static string Join(this IEnumerable<string> values, string separator) => 
    //     values.ToArray().Join(separator);
    
    // public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
    // {
    //     if (list is List<T> listT)
    //     {
    //         listT.AddRange(items);
    //     }
    //     else
    //     {
    //         foreach (T item in items)
    //         {
    //             list.Add(item);
    //         }
    //     }
    // }
}