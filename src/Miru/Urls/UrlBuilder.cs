using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Policy;
using Baseline;
using Baseline.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Miru.Urls
{
    public class UrlBuilder<TInput> where TInput : class
    {
        public TInput Request { get; }
        
        private readonly Dictionary<Accessor, object> _withProperties = new Dictionary<Accessor, object>();
        private readonly List<Accessor> _withoutProperties = new List<Accessor>();
        private readonly List<KeyValuePair<Accessor, object>> _withoutPropertyAndValues = new List<KeyValuePair<Accessor, object>>();
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
            _withProperties.AddOrUpdate(ReflectionHelper.GetAccessor(property), value);
            return this;
        }

        public UrlBuilder<TInput> With<TProperty>(
            Expression<Func<TInput, TProperty>> property,
            object value)
        {
            _withProperties.AddOrUpdate(ReflectionHelper.GetAccessor(property), value);
            return this;
        }
        
        public UrlBuilder<TInput> Without<TProperty>(Expression<Func<TInput, TProperty>> property)
        {
            _withoutProperties.Add(ReflectionHelper.GetAccessor(property));
            return this;
        }

        public UrlBuilder<TInput> Without<TProperty>(Expression<Func<TInput, TProperty>> property, object value)
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
            var dictGenerator = new RouteValueDictionaryGenerator();
            
            // // TODO: Make as UrlOptions.QueryStrings filter
            // if (Request.GetType().IsRequestCommand())
            //     return new RouteValueDictionary();
            
            var queryStrings = dictGenerator.Generate(
                Request, 
                urlOptions: _urlOptions,
                withoutProperties: _withoutProperties,
                withProperties: _withProperties,
                withoutPropertyAndValues: _withoutPropertyAndValues);

            return queryStrings;
        }
    }
}