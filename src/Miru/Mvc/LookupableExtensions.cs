using System;
using System.Collections.Generic;
using System.Linq;
using Miru.Domain;

namespace Miru.Mvc
{
    public static class EnumerableExtensions
    {
        public static Lookups ToLookups<T>(
            this IEnumerable<T> list, 
            Func<T, object> idFunc, 
            Func<T, object> descriptionFunc)
        {
            var lookups = new Lookups();

            foreach (var item in list)
            {
                lookups.Add(new Lookup(idFunc(item), descriptionFunc(item)));
            }

            return lookups;
        }
        
        public static Lookups ToLookups<T, TValue>(
            this IEnumerable<Enumeration<T, TValue>> list) where T : Enumeration<T, TValue> where TValue : IComparable
        {
            var lookups = new Lookups();

            foreach (var item in list)
            {
                lookups.Add(new Lookup(item.Value, item.Name));
            }

            return lookups;
        }
        
        public static Lookups ToLookups<T>(this IEnumerable<T> list) where T : Enum
        {
            var lookups = new Lookups();

            foreach (var item in list)
            {
                lookups.Add(new Lookup(item.ToInt(), item.DisplayName()));
            }

            return lookups;
        }
        
        public static Lookups ToLookups<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var lookups = new Lookups();

            foreach (var item in dictionary.OrderBy(x => x.Value))
            {
                lookups.Add(new Lookup(item.Key, item.Value));
            }

            return lookups;
        }
    }
}