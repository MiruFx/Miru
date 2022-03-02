using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Policy;
using Baseline;
using Baseline.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Miru.Urls;

public class UrlBuilder2<TInput> where TInput : class
{
    
    public TInput Request { get; }
        
    private readonly Dictionary<Accessor, object> _withProperties = new Dictionary<Accessor, object>();
    private readonly List<Accessor> _withoutProperties = new List<Accessor>();
    private readonly List<KeyValuePair<Accessor, object>> _withoutPropertyAndValues = new List<KeyValuePair<Accessor, object>>();
    private readonly IUrlMaps _urlMaps;
    private readonly UrlOptions _urlOptions;

    public UrlBuilder2(
        TInput request, 
        UrlOptions urlOptions,
        IUrlMaps urlMaps)
    {
        Request = request;
        _urlOptions = urlOptions;
        _urlMaps = urlMaps;
    }

    public static implicit operator string(UrlBuilder2<TInput> input)
    {
        return input.ToString();
    }
        
    public UrlBuilder2<TInput> With<TProperty>(
        Expression<Func<TInput, TProperty>> property,
        TProperty value)
    {
        _withProperties.AddOrUpdate(ReflectionHelper.GetAccessor(property), value);
        return this;
    }

    public UrlBuilder2<TInput> With<TProperty>(
        Expression<Func<TInput, TProperty>> property,
        object value)
    {
        _withProperties.AddOrUpdate(ReflectionHelper.GetAccessor(property), value);
        return this;
    }
        
    public UrlBuilder2<TInput> Without<TProperty>(Expression<Func<TInput, TProperty>> property)
    {
        _withoutProperties.Add(ReflectionHelper.GetAccessor(property));
        return this;
    }

    public UrlBuilder2<TInput> Without<TProperty>(Expression<Func<TInput, TProperty>> property, object value)
    {
        _withoutPropertyAndValues.Add(ReflectionHelper.GetAccessor(property), value);
        return this;
    }
        
    public override string ToString()
    {
        var queryString = BuildQueryString();

        var path = _urlMaps.UrlFor(Request, queryString);
            
        if (path == null)
            throw new UrlNotMappedException($@"Could not build url for {Request.GetType().ActionName()}.

Check in your controller's action if is set the right Feature.

Also, check if there is a [Route] with constraints for the parameters. Maybe the constraints are not being met
");

        // TODO: Configurable
        if (Request.GetType().IsRequestCommand())
        {
            var questionMarkPos = path.IndexOf('?');
                
            if (questionMarkPos > 0)
            {
                return path.Substring(0, questionMarkPos);
            }
        }
            
        return path;
    }

    private RouteValueDictionary BuildQueryString()
    {
        // var dic = GenerateDictionary()
            
        // var dictGenerator = HtmlHelper.ObjectToDictionary(Request);
            
        // // TODO: Make as UrlOptions.QueryStrings filter
        // if (Request.GetType().IsRequestCommand())
        //     return new RouteValueDictionary();
            
        // var queryStrings = dictGenerator.Generate(
        //     Request, 
        //     urlOptions: _urlOptions,
        //     withoutProperties: _withoutProperties,
        //     withProperties: _withProperties,
        //     withoutPropertyAndValues: _withoutPropertyAndValues);

        return new RouteValueDictionary();
    }

    private Dictionary<string, object> GenerateDictionary<T>(
        T model, 
        string prefix = "", 
        Dictionary<string, object> dictionary = null)
    {
        dictionary ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            
        // var dictGenerator = new RouteValueDictionaryGenerator();
        foreach (var helper in PropertyHelper.GetVisibleProperties(Request.GetType()))
        {
            var value = helper.GetValue(Request);

            if (value == null || value.Equals(helper.Property.PropertyType.GetDefaultValue()))
                continue;

            // if (helper.IsEnumerable)
            // {
            //     
            // }
            //     GenerateDictionary(value, $"{prefix}{helper.Name}[i]"helper.Name)
                
            dictionary[helper.Name] = helper.GetValue(Request);
        }

        return dictionary;
    }
}

public static class TypeExtensions
{
    //a thread-safe way to hold default instances created at run-time
    private static readonly ConcurrentDictionary<Type, object> TypeDefaults = new();

    public static object GetDefaultValue(this Type type)
    {
        return type.IsValueType
            ? TypeDefaults.GetOrAdd(type, Activator.CreateInstance)
            : null;
    }
}