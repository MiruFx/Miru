using System;
using System.Collections.Generic;
using System.Linq;
using AV.Enumeration;
using Miru.Domain;

namespace Miru.Mvc;

public static class EnumerableExtensions
{
    public static SelectLookups ToSelectLookups<T>(
        this IEnumerable<T> list, 
        Func<T, object> idFunc, 
        Func<T, object> descriptionFunc)
    {
        var lookups = new SelectLookups();

        foreach (var item in list)
        {
            lookups.Add(new Lookup(idFunc(item), descriptionFunc(item)));
        }

        return lookups;
    }
        
    // public static SelectLookups ToSelectLookups<T, TValue>(
    //     this IEnumerable<Enumeration<T, TValue>> list) where T : Enumeration<T, TValue> where TValue : IComparable
    // {
    //     var lookups = new SelectLookups();
    //
    //     foreach (var item in list)
    //     {
    //         lookups.Add(new Lookup(item.Value, item.Name));
    //     }
    //
    //     return lookups;
    // }
           
    public static SelectLookups ToSelectLookups(this IEnumerable<Enumeration> list)
    {
        var lookups = new SelectLookups();

        foreach (var item in list)
        {
            lookups.Add(new Lookup(item.Value, item.Name));
        }

        return lookups;
    }
        
    public static SelectLookups ToSelectLookups<T>(this IEnumerable<T> list) where T : Enum
    {
        var lookups = new SelectLookups();

        foreach (var item in list)
        {
            lookups.Add(new Lookup(item.ToInt(), item.DisplayName()));
        }

        return lookups;
    }
        
    public static SelectLookups ToSelectLookups<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        var lookups = new SelectLookups();

        foreach (var item in dictionary.OrderBy(x => x.Value))
        {
            lookups.Add(new Lookup(item.Key, item.Value));
        }

        return lookups;
    }
}