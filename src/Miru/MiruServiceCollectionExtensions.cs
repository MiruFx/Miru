using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Miru.Behaviors.BelongsToUser;
using Miru.Behaviors.TimeStamp;
using Miru.Config;
using Miru.Consolables;
using Miru.Core;
using Miru.Foundation;
using Miru.Foundation.Logging;
using Miru.Html;
using Miru.Makers;
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
                configFinder.Find<HtmlConfiguration>() as HtmlConfiguration ?? new HtmlConfiguration(),
                opt => opt.UseEnumerationModelBinding());
            
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

            services.AddMiruCliHost();

            services.AddConsolables<TStartup>();
            services.AddConsolables<MiruApp>();
            
            services.AddMakers();
            
            services.AddSingleton<IJsonConverter, JsonConverter>();
            services.AddStorage();
            services.AddConsolable<StorageLinkConsolable>();

            services.AddSingleton<ISessionStore, HttpSessionStore>();

            return services;
        }

        public static IServiceCollection AddMiruApp(this IServiceCollection services)
        {
            return services
                .AddSingleton<IMiruApp>(sp => new MiruApp(sp))
                .AddTransient<ScopedServices, ScopedServices>();
        }
        
        public static IServiceCollection AddMiruSolution(
            this IServiceCollection services,
            MiruSolution solution)
        {
            return services.ReplaceSingleton(solution);
        }
    }
}
