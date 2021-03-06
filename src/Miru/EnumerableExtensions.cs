﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Miru.Domain;

namespace Miru
{
    public static class EnumerableExtensions
    {
        public static T SingleOrFail<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, string exceptionMessage)
        {
            return enumerable.SingleOrDefault(predicate) ?? throw new NotFoundException(exceptionMessage);
        }
        
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
                action(item);

            return enumerable;
        }
        
        public static string Join<T>(this IEnumerable<T> enumerable, string split)
        {
            return enumerable
                .Join2(x => x.ToString() + split)
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

        public static TList ById<TList>(this IEnumerable<TList> list, long id) where TList : Entity
        {
            return list.FirstOrDefault(e => e.Id == id);
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
    }
}
