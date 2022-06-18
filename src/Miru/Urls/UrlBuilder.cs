using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using Baseline;
using Baseline.Reflection;
using FastExpressionCompiler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Miru.Urls;

public class UrlBuilder<TInput> where TInput : class
{
    public TInput Request { get; }
        
    private readonly Dictionary<string, object> _withProperties = new();
    private readonly List<string> _withoutProperties = new();
    private readonly List<KeyValuePair<string, object>> _withoutPropertyAndValues = new();
    private readonly IUrlMaps _urlMaps;
    private readonly UrlOptions _urlOptions;

    public UrlBuilder(
        TInput request, 
        UrlOptions urlOptions,
        IUrlMaps urlMaps)
    {
        Request = request;
        _urlOptions = urlOptions;
        _urlMaps = urlMaps;
    }

    public static implicit operator string(UrlBuilder<TInput> input)
    {
        return input.ToString();
    }
        
    public UrlBuilder<TInput> With<TProperty>(
        Expression<Func<TInput, TProperty>> property,
        TProperty value)
    {
        _withProperties.AddOrUpdate(ReflectionHelper.GetProperty(property).Name, value);
        return this;
    }

    public UrlBuilder<TInput> With<TProperty>(
        Expression<Func<TInput, TProperty>> property,
        object value)
    {
        _withProperties.AddOrUpdate(ReflectionHelper.GetProperty(property).Name, value);
        return this;
    }
        
    public UrlBuilder<TInput> Without<TProperty>(Expression<Func<TInput, TProperty>> property)
    {
        _withoutProperties.Add(ReflectionHelper.GetProperty(property).Name);
        return this;
    }

    public UrlBuilder<TInput> Without<TProperty>(Expression<Func<TInput, TProperty>> property, object value)
    {
        _withoutPropertyAndValues.Add(ReflectionHelper.GetProperty(property).Name, value);
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
        // TODO: Make as UrlOptions.QueryStrings filter
        // if (Request.GetType().IsRequestCommand())
        // {
        //     return new RouteValueDictionary();
        // }

        var modifiers = new UrlBuilderModifiers(
            _withProperties,
            _withoutProperties,
            _withoutPropertyAndValues);

        return new RouteValueDictionaryGenerator(_urlOptions)
            .Generate(Request, modifiers);
    }
}