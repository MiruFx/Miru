using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Ardalis.SmartEnum;
using Baseline.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Miru.Urls;

public class RouteValueDictionaryGenerator
{
    private readonly UrlOptions _urlOptions;

    public RouteValueDictionaryGenerator(UrlOptions urlOptions)
    {
        _urlOptions = urlOptions;
    }

    public RouteValueDictionary Generate<TRequest>(
        TRequest request,
        UrlBuilderModifiers modifiers = null)
    {
        if (modifiers == null)
        {
            modifiers = new UrlBuilderModifiers();
        }
        
        var dictionary = new RouteValueDictionary();

        var hasModifiers = modifiers.HasModifiers;
        
        var ignoreFilters = _urlOptions?.QueryStrings
            .GetQueryStringBuilderFilters(request)
            .ToImmutableList();
        
        var ignoreWhenHasModifiers = _urlOptions?.QueryStrings.GetIgnoredWhenModified();
        
        foreach (var property in PropertyHelper.GetVisibleProperties(request.GetType()))
        {
            if (ShouldIgnoreType(property, ignoreWhenHasModifiers, hasModifiers))
                continue;
            
            var rawValue = property.GetValue(request);

            if (ShouldFilterPropertyValue(property, rawValue, ignoreFilters))
                continue;

            if (property.IsEnumerable && rawValue is IEnumerable list and not string)
            {
                var i = 0;
                
                foreach (var item in list)
                {
                    var addItem = true;
                    
                    // var value = GetValue(property, item, modifiers);
                    var value = item;

                    if (ShouldIgnoreValue(property, value, modifiers))
                        continue;
                    
                    foreach (var withoutPropertyAndValue in modifiers.WithoutValues)
                    {
                        if (withoutPropertyAndValue.Key == property.Property.Name &&
                            withoutPropertyAndValue.Value.Equals(item))
                        {
                            addItem = false;
                            break;
                        }
                    }

                    if (addItem)
                    {
                        dictionary[$"{property.Name}[{i}]"] = item;
                    }
                    
                    i++;
                }
                
                if (modifiers.With.TryGetValue(property.Name, out var customValue))
                {
                    dictionary[$"{property.Name}[{i}]"] = customValue;
                    i++;

                    modifiers.With.Remove(property.Name);
                }
            }
            else
            {
                var (originalValue, isModifier) = GetValue(property, rawValue, modifiers);

                if (ShouldIgnoreValue(property, originalValue, modifiers))
                    continue;

                if (isModifier == false)
                {
                    if (originalValue is null)
                        continue;
                    
                    if (originalValue.Equals(property.Property.PropertyType.GetDefaultValue()))
                        continue;
                }

                string value;
                
                if (originalValue is DateTime dateTime)
                {
                    value = dateTime.ToShortDateString();
                }
                else if (originalValue is ISmartEnum)
                {
                    // at the moment we support only SmartEnum<int, TEnum>
                    value = typeof(SmartEnum<>)
                        .MakeGenericType(property.Property.PropertyType)
                        .GetProperty("Value")?
                        .GetValue(originalValue)?
                        .ToString();
                }
                // else if (property.Property.PropertyType.ImplementsGenericOf(typeof(Enumeration<,>)))
                // {
                //     value = typeof(Enumeration<,>)
                //         .MakeGenericType(
                //             property.Property.PropertyType,
                //             property.Property.PropertyType.BaseType?.GetGenericArguments()[1])
                //         .GetProperty("Value")?
                //         .GetValue(value);
                // }
                else
                {
                    value = originalValue.ToString();
                }
                
                // else if (property.Property.PropertyType.ImplementsGenericOf(typeof(Enumeration<,>)))
                // {
                //     value = typeof(Enumeration<,>)
                //         .MakeGenericType(
                //             property.Property.PropertyType,
                //             property.Property.PropertyType.BaseType?.GetGenericArguments()[1])
                //         .GetProperty("Value")?
                //         .GetValue(value);
                // }
                
                dictionary[property.Name] = value;
            }
        }

        foreach (var with in modifiers.With)
        {
            if (dictionary.ContainsKey(with.Key) == false)
            {
                dictionary.Add(with.Key, with.Value);
            }
        }
        
        return dictionary;
    }

    private bool ShouldIgnoreValue(
        PropertyHelper property,
        object value,
        UrlBuilderModifiers modifiers)
    {
        foreach (var withoutProperty in modifiers.Without)
            if (withoutProperty == property.Name)
                return true;
            
        foreach (var without in modifiers.WithoutValues)
            if (without.Key == property.Name && without.Value.Equals(value))
                return true;
        
        return false;
    }

    private (object, bool) GetValue(PropertyHelper property, object rawValue, UrlBuilderModifiers modifiers)
    {
        if (modifiers.With.TryGetValue(property.Name, out var customValue))
        {
            // modifiers.With.Remove(property.Name);
            
            return (customValue, true);
        }
        
        return (rawValue, false);
    }

    private bool ShouldIgnoreType(
        PropertyHelper property,
        Func<PropertyInfo, bool> ignoreWhenHasModifiers, 
        bool hasModifiers)
    {
        if (property.Property.PropertyType.IsGenericOf(typeof(IEnumerable<>)))
            return true;

        if (property.Property.PropertyType.IsGenericOf(typeof(IDictionary<,>)))
            return true;
            
        if (property.Property.PropertyType.IsGenericOf(typeof(IReadOnlyList<>)))
            return true;
            
        if (property.Property.PropertyType.IsGenericOf(typeof(IReadOnlyCollection<>)))
            return true;
        
        if (property.Property.HasAttribute<UrlIgnore>())
            return true;

        if (property.Property.CanWrite == false)
            return true;

        if (hasModifiers && ignoreWhenHasModifiers != null)
            if (ignoreWhenHasModifiers(property.Property))
                return true;
        
        return false;
    }
    
    private bool ShouldFilterPropertyValue(
        PropertyHelper property,
        object value,
        IEnumerable<Func<PropertyInfo, object, bool>> ignoreFilters)
    {
        foreach (var ignoreFilter in ignoreFilters)
            if (ignoreFilter(property.Property, value))
                return true;

        return false;
    }
}