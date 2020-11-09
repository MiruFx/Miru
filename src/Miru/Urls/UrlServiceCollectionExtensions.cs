using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Config;

namespace Miru.Urls
{
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
            
            services.AddSingleton<UrlLookup>();
            
            services.AddSingleton<IUrlMaps, DefaultUrlMaps>();
            
            services.AddSingleton(convention);
            
            services.Configure<MvcOptions>(x => x.Conventions.Add(convention));

            var urlOptions = new UrlOptions();
            
            options?.Invoke(urlOptions);         
            
            services.AddSingleton(urlOptions);
            
            services.AddSingleton(new QueryStringConfig());
            
            return services;
        }
    }
}
