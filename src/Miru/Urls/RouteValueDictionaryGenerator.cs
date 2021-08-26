using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Baseline.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Miru.Domain;

namespace Miru.Urls
{
    public class RouteValueDictionaryGenerator
    {
        private static readonly ConcurrentDictionary<string, CacheOfTypes> Cache = new ConcurrentDictionary<string, CacheOfTypes>();

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCaches = new ConcurrentDictionary<Type, PropertyInfo[]>();
        
        public RouteValueDictionary Generate(
            object model, 
            string prefix = "",
            RouteValueDictionary dict = null,
            UrlOptions urlOptions = null,
            List<Accessor> withoutProperties = null,
            Dictionary<Accessor, object> withProperties = null,
            List<KeyValuePair<Accessor, object>> withoutPropertyAndValues = null)
        {
            if (model == null)
                return dict;

            Type modelType = model.GetType();

            dict ??= new RouteValueDictionary();

            var filters = urlOptions?.QueryStrings.GetQueryStringBuilderFilters(model);

            var filtersWhenModified = urlOptions?.QueryStrings.GetIgnoredWhenModified();

            var hasWithOrWithout =
                (withProperties != null && withProperties.Any()) ||
                (withoutProperties != null && withoutProperties.Any()) ||
                (withoutPropertyAndValues != null && withoutPropertyAndValues.Any());

            foreach (PropertyInfo p in PropertyCaches.GetOrAdd(modelType, t1 => System.Reflection.TypeExtensions.GetProperties(t1)))
            {
                var theCache = Cache.GetOrAdd(modelType.FullName + prefix + p.Name, _ => new CacheOfTypes
                {
                    Attributes = p.GetCustomAttributes().ToArray(),
                    
                    PropType = IsEnum(p.PropertyType) || IsConvertible(p.PropertyType) || IsEnumeration(p.PropertyType) 
                        ? PropType.Simple 
                        : (IsEnumerable(p.PropertyType) 
                            ? PropType.Enumerable
                            : (SimpleGetter(p) 
                                ? PropType.Complex 
                                : PropType.Unknown)),
                    
                    MemberAccessor = new MemberAccessor(modelType, p.Name)
                });

                var propType = theCache.PropType;
                var attributes = theCache.Attributes;
                var accessor = theCache.MemberAccessor;
                var originalValue = accessor.Get(model);
                
                if (ShouldExcludeProperty(filters, p, originalValue))
                    continue;
                
                if (propType == PropType.Simple)
                {
                    var val = GetValueForSimpleProperty(p, accessor, withProperties, model);

                    if (ShouldIgnoreProperty(p, filters, filtersWhenModified, hasWithOrWithout, withoutProperties, withoutPropertyAndValues, val))
                        continue;
                    
                    if (val == null || Equals(val, GetDefault(p.PropertyType)) && attributes.OfType<FromRouteAttribute>().Any() == false)
                    {
                        continue;
                    }

                    if (val is DateTime dateTime)
                    {
                        val = dateTime.ToShortDateString();
                    }
                    else if (p.PropertyType.ImplementsGenericOf(typeof(Enumeration<,>)))
                    {
                        model = typeof(Enumeration<,>)
                            .MakeGenericType(p.PropertyType, p.PropertyType.BaseType?.GetGenericArguments()[1])
                            .GetMethod("FromValue")?
                            .Invoke(null, new object[] { val });
                    }

                    dict.Add(prefix + p.Name, val);
                }
                else if (propType == PropType.Enumerable)
                {
                    var i = 0;
                    var added = new List<object>();
                    
                    foreach (object sub in (IEnumerable)accessor.Get(model) ?? new object[0])
                    {
                        if (sub == null)
                            continue;

                        var subType = sub.GetType();
                        
                        var subPropType = IsEnum(subType) || IsConvertible(subType) ? PropType.Simple : PropType.Unknown;

                        if (subPropType == PropType.Simple)
                        {
                            if (ShouldIgnoreProperty(p, filters, filtersWhenModified, hasWithOrWithout, withoutProperties, withoutPropertyAndValues, sub))
                                continue;
                            
                            added.Add(sub);
                            dict.Add(prefix + p.Name + "[" + (i++) + "]", sub);
                        }
                        else
                        {
                            Generate(sub, prefix + p.Name + "[" + (i++) + "].", dict);
                        }
                    }

                    if (withProperties != null)
                    {
                        var withCustomProperties = withProperties.Where(a => a.Key.Name == p.Name);

                        foreach (var withCustomProperty in withCustomProperties)
                        {
                            if (added.Contains(withCustomProperty.Value))
                                continue;
                        
                            dict.Add(prefix + p.Name + "[" + (i++) + "]", withCustomProperty.Value);
                        }
                    }
                }
                else if (propType == PropType.Complex)
                {
                    Generate(accessor.Get(model), prefix + p.Name + ".", dict);
                }
            }

            // add with
            if (withProperties != null)
                foreach (var withProperty in withProperties?.Where(p => p.Key.PropertyType.Implements<IEnumerable>() == false))
                {
                    // if key/value was not added yet
                    if (!dict.Any(x => x.Key == withProperty.Key.Name && x.Value.Equals(withProperty.Value)))
                        dict.Add(withProperty.Key.Name, withProperty.Value.ToString());
                }
            
            return dict;
        }

        private object GetValueForSimpleProperty(
            PropertyInfo propertyInfo,
            MemberAccessor accessor,
            Dictionary<Accessor, object> withProperties, 
            object model)
        {
            if (withProperties == null)
                return accessor.Get(model);
            
            var withCustomProperty = withProperties.LastOrDefault(a => a.Key.Name == propertyInfo.Name);

            return withCustomProperty.Key == null ? accessor.Get(model) : withCustomProperty.Value;
        }

        private bool ShouldExcludeProperty(IEnumerable<Func<PropertyInfo, object, bool>> filters, PropertyInfo propertyInfo, object propertyValue)
        {
            if (propertyInfo.CanWrite == false)
                return true;

            if (propertyInfo.PropertyType.IsGenericOf(typeof(IDictionary<,>)))
                return true;
            
            if (propertyInfo.PropertyType.IsGenericOf(typeof(IReadOnlyList<>)))
                return true;
            
            if (propertyInfo.PropertyType.IsGenericOf(typeof(IReadOnlyCollection<>)))
                return true;

            if (propertyInfo.PropertyType.IsGenericOf(typeof(IEnumerable<>)))
                return true;
                
            if (filters != null )
                foreach (var filter in filters)
                    if (filter(propertyInfo, propertyValue) == false)
                        return true;

            return false;
        }
        
        private bool ShouldIgnoreProperty(
            PropertyInfo propertyInfo,
            IEnumerable<Func<PropertyInfo, object, bool>> filters,
            Func<PropertyInfo, bool> ignoreWhenModifieldFilters,
            bool modelWasModified, 
            List<Accessor> withoutProperties,
            List<KeyValuePair<Accessor, object>> withoutPropertyAndValues,
            object propertyValue)
        {
            if (filters != null )
                foreach (var filter in filters)
                    if (filter(propertyInfo, propertyValue) == false)
                        return true;

            if (withoutProperties != null)
                foreach (var withoutProperty in withoutProperties)
                    if (withoutProperty.Name == propertyInfo.Name)
                        return true;
                
            if (withoutPropertyAndValues != null)
                foreach (var withoutPropertyAndValue in withoutPropertyAndValues)
                    if (withoutPropertyAndValue.Key.Name == propertyInfo.Name && withoutPropertyAndValue.Value.Equals(propertyValue))
                        return true;
                    
            if (modelWasModified && ignoreWhenModifieldFilters != null)
                if (ignoreWhenModifieldFilters(propertyInfo))
                    return true;
            
            return false;
        }

        private static bool IsEnum(Type t)
        {
            if (t.GetTypeInfo().IsEnum)
                return true;
            
            var nullableType = Nullable.GetUnderlyingType(t);
            
            return nullableType != null && nullableType.GetTypeInfo().IsEnum;
        }
        
        private static bool IsEnumeration(Type t)
        {
            return t.ImplementsGenericOf(typeof(Enumeration<,>))
                   || t.ImplementsGenericOf(typeof(Enumeration<>));
        }

        private static readonly List<Type> ConvertibleTypes = new List<Type>
        {
            typeof (bool), 
            typeof (byte), 
            typeof (char),
            typeof (DateTime), 
            typeof(DateTimeOffset), 
            typeof (decimal), 
            typeof (double), 
            typeof (float), 
            typeof (int),
            typeof (long), 
            typeof (sbyte), 
            typeof (short), 
            typeof (string), 
            typeof (uint),
            typeof (ulong), 
            typeof (ushort), 
            typeof(Guid), 
            typeof(TimeSpan)
        };

        /// <summary>
        /// Returns true if this Type matches any of a set of Types.
        /// </summary>
        /// <param name="type">This type.</param>
        /// <param name="types">The Types to compare this Type to.</param>
        private static bool In(Type type, IEnumerable<Type> types)
        {
            foreach (Type t in types)
            {
                if (t.IsAssignableFrom(type) || (Nullable.GetUnderlyingType(type) != null && t.IsAssignableFrom(Nullable.GetUnderlyingType(type))))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if this Type is one of the types accepted by Convert.ToString() 
        /// (other than object).
        /// </summary>
        private static bool IsConvertible(Type t)
        {
            return In(t, ConvertibleTypes);
        }

        /// <summary>
        /// Gets whether this type is enumerable.
        /// </summary>
        private static bool IsEnumerable(Type t)
        {
            return typeof(IEnumerable).IsAssignableFrom(t);
        }

        /// <summary>
        /// Returns true if this property's getter is public, has no arguments, and has no 
        /// generic type parameters.
        /// </summary>
        private static bool SimpleGetter(PropertyInfo info)
        {
            MethodInfo method = info.GetGetMethod(false);
            return method != null && method.GetParameters().Length == 0 && method.GetGenericArguments().Length == 0;
        }

        private static readonly Dictionary<Type, object> Defaults = new Dictionary<Type, object>()
        {
            {typeof (bool), false},
            {typeof (int), new int()},
            {typeof (DateTime), new DateTime()},
            {typeof (decimal), new decimal()},
            {typeof (float), new float()},
            {typeof (double), new double()},
        };

        private static object GetDefault(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                object val;
                if (Defaults.TryGetValue(type, out val))
                    return val;
                return Activator.CreateInstance(type);
            }
            return null;
        }
        
        public enum PropType
        {
            Simple,
            Enumerable,
            Complex,
            Unknown
        }

        private class CacheOfTypes
        {
            public PropType PropType { get; set; }
            public Attribute[] Attributes { get; set; }
            public MemberAccessor MemberAccessor { get; set; }
        }
    }
}