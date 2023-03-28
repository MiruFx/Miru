using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Baseline.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Miru.Urls;

public class UrlBuilder<TInput> where TInput : class
{
    public TInput Request { get; }
        
    private readonly Dictionary<string, object> _withProperties = new();
    private readonly List<string> _withoutProperties = new();
    private readonly List<KeyValuePair<string, object>> _withoutPropertyAndValues = new();
    private readonly IUrlMaps _urlMaps;
    private readonly UrlOptions _urlOptions;
    private bool _acceptCommand;

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

    public UrlBuilder<TInput> AcceptCommand()
    {
        _acceptCommand = true;
        return this;
    }

    public UrlBuilder<TInput> With<TProperty>(
        Expression<Func<TInput, TProperty>> property,
        TProperty value)
    {
        _withProperties.AddOrUpdate(ReflectionHelper.GetProperty(property).Name, value);
        return this;
    }

    public UrlBuilder<TInput> With<TProperty>(
        string propertyName,
        TProperty value)
    {
        _withProperties.AddOrUpdate(propertyName, value);
        return this;
    }

    public UrlBuilder<TInput> With<TProperty>(
        Expression<Func<TInput, TProperty>> property,
        object value) =>
            With(ReflectionHelper.GetProperty(property).Name, value);
        
    public UrlBuilder<TInput> Without<TProperty>(Expression<Func<TInput, TProperty>> property) =>
        Without(ReflectionHelper.GetProperty(property).Name);

    public UrlBuilder<TInput> Without(string propertyName)
    {
        _withoutProperties.Add(propertyName);
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
        if (_acceptCommand == false && Request.GetType().IsRequestCommand())
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