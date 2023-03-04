using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Miru.Urls;

public static class UrlServiceCollectionExtensions
{
    public static IServiceCollection AddMiruUrls(
        this IServiceCollection services,
        Action<UrlOptions> options = null)
    {
        var routesInfo = new Dictionary<string, ModelToUrlMap>();
            
        var convention = new MiruRoutingDiscoverer(routesInfo);

        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            
        services.AddScoped(x => {
            var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                
            var factory = x.GetRequiredService<IUrlHelperFactory>();
                
            return factory.GetUrlHelper(actionContext);
        });
            
        services.AddScoped<UrlLookup>();
        services.AddScoped<UrlPrefix>();
            
        services.AddSingleton<IUrlMaps, DefaultUrlMaps>();
            
        services.AddSingleton(convention);
            
        services.Configure<MvcOptions>(x => x.Conventions.Add(convention));

        if (options != null) services.Configure(options);
        services.AddSingleton(x => x.GetRequiredService<IOptions<UrlOptions>>().Value);
            
        services.AddSingleton(new QueryStringConfig());

        services.AddSingleton<UrlMapsScanner>();
            
        return services;
    }
}