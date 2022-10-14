using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;

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
    
    public static SelectLookups ToSelectLookups<TSmartEnum>(this IReadOnlyCollection<TSmartEnum> enumValues)
        where TSmartEnum : SmartEnum<TSmartEnum>
    {
        var lookups = new SelectLookups();

        foreach (var item in enumValues.OrderBy(x => x.Value))
        {
            lookups.Add(new Lookup(item.Value, item.Name));
        }

        return lookups;
    }
}