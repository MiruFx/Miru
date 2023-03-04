using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Mailing;
using Miru.Urls;

namespace Miru.Mvc;

public static class UrlHelperExtensions
{
    public static string For<TRequest>(this IUrlHelper urlHelper, TRequest request) where TRequest : class, new()
    {
        return Build(urlHelper, new TRequest());
    }
        
    public static UrlBuilder<TRequest> Build<TRequest>(this IUrlHelper urlHelper) where TRequest : class, new()
    {
        return Build(urlHelper, new TRequest());
    }
        
    public static UrlBuilder<TRequest> Build<TRequest>(this IUrlHelper urlHelper, TRequest request) where TRequest : class
    {
        var urlOptions = urlHelper.ActionContext.HttpContext.RequestServices.GetRequiredService<UrlOptions>();

        var urlMaps = urlHelper.ActionContext.HttpContext.RequestServices.GetService<IUrlMaps>();
        var urlPrefix = urlHelper.ActionContext.HttpContext.RequestServices.GetService<UrlPrefix>();
            
        return new UrlBuilder<TRequest>(request, urlOptions, urlMaps, urlPrefix);
    }
        
    public static string Full<TRequest>(this IUrlHelper urlHelper) where TRequest : class, new()
    {
        return Full(urlHelper, new TRequest());
    }
        
    public static string Full<TRequest>(this IUrlHelper urlHelper, TRequest request) where TRequest : class
    {
        var urlLookup = urlHelper.ActionContext.HttpContext.RequestServices.GetRequiredService<UrlLookup>();

        return urlLookup.FullFor(request);
    }
}