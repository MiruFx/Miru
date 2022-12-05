using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Config;
using Miru.Core;
using Miru.Foundation;
using Miru.Foundation.Logging;
using Miru.Makers;
using Miru.Mvc;
using Miru.Scopables;
using Miru.Storages;
using Miru.Urls;
using Serilog.Events;
using Vereyon.Web;
using Miru.Pipeline;

namespace Miru;

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

        // services.AddMiruMvc(opt => opt.UseEnumerationModelBinding());
        // services.AddMiruMvc(opt => opt.ModelBinderProviders.Insert(0, new EnumerationQueryStringModelBinderProvider()));
        services.AddMiruMvc(opt => opt.ModelBinderProviders.Insert(0, new SmartEnumBinderProvider()));
            
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
            
        services.AddConsolable<ConfigShowConsolable>();
        services.AddConsolable<ConfigServicesConsolable>();
        services.AddConsolable<InvokeConsolable>();

        services.AddMakers();
            
        services.AddSingleton<IJsonConverter, JsonConverter>();
        services.AddStorages();
        services.AddConsolable<StorageLinkConsolable>();

        services.AddSingleton<ISessionStore, HttpSessionStore>();
            
        // default scope
        services.AddCurrentAttributes<DefaultCurrent, DefaultCurrentAttributes>();

        return services;
    }

    public static IServiceCollection AddMiruApp(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMiruApp>(sp => new MiruApp(sp))
            .AddTransient<ScopedServices, ScopedServices>();
    }
        
    public static IServiceCollection AddMiruApp<TAssemblyOfType>(this IServiceCollection services)
    {
        App.Assembly = typeof(TAssemblyOfType).Assembly;
        
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
    
    public static IServiceCollection AddMiruSolution(
        this IServiceCollection services,
        MiruPath path)
    {
        return services.ReplaceSingleton(new MiruSolution(path));
    }
}