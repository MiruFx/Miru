using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Foundation;
using Miru.Foundation.Logging;
using Miru.Html;
using Miru.Mvc;
using Miru.Storages;
using Miru.Urls;
using Serilog.Events;
using Vereyon.Web;

namespace Miru
{
    public static class MiruServiceCollectionExtensions
    {
        public static IServiceCollection AddMiru<TStartup>(
            this IServiceCollection services,
            Action<MvcOptions> mvcOptions = null)
        {
            var configFinder = new ConfigFinder<TStartup>();
            
            // scan for configs
            foreach (var config in configFinder.CreateInstanceOfAllConfigs())
            {
                config.ConfigureService(services);
            }

            services.AddMiruMvc(
                configFinder.Find<HtmlConvention>() as HtmlConvention ?? new HtmlConvention(),
                mvcOptions);
            
            services.AddSingleton(
                configFinder.Find<ObjectResultConfiguration>() as ObjectResultConfiguration ?? new DefaultObjectResultConfig());
            
            services.AddSingleton(
                configFinder.Find<ExceptionResultConfiguration>() as ExceptionResultConfiguration ?? new DefaultExceptionResultConfig());
                
            // default logging level for app is Information
            services.AddSerilogConfig(config =>
            {
                config.MinimumLevel.Override(typeof(TStartup).Assembly.GetName().Name, LogEventLevel.Information);
            });
                
            services.AddFlashMessage();
            services.AddMiruUrls();
            services.AddConsolables<TStartup>();
            services.AddSingleton<IJsonConverter, JsonConverter>();

            services.AddSingleton<ISessionStore, HttpSessionStore>();
            
            services.AddStorage();
            
            return services;
        }

        public static IServiceCollection AddMiruApp(this IServiceCollection services)
        {
            return services
                .AddSingleton<IMiruApp>(sp => new MiruApp(sp))
                .AddTransient<ScopedServices, ScopedServices>();
        }
    }
}
