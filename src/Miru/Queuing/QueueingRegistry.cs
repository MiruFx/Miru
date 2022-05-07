using System;
using Hangfire;
using Hangfire.LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Consolables;
using Miru.Core;
using Miru.Storages;

namespace Miru.Queuing;

public static class QueueingRegistry
{
    public static IServiceCollection AddQueuing(
        this IServiceCollection services,
        Action<QueuingBuilder> queuingBuilder)
    {
        services.AddSingleton<QueueingOptions>();
            
        services.AddHangfire((sp, configuration) =>
        {
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseActivator(sp.GetService<MiruJobActivator>());
                
            var builder = new QueuingBuilder(sp, configuration, services);
                
            queuingBuilder.Invoke(builder);
        });
            
        services.AddTransient<MiruJobActivator>();
            
        services.AddTransient(sp => new BackgroundJobServer(
            new BackgroundJobServerOptions(),
            sp.GetService<JobStorage>()));

        services.AddConsolable<QueueRunConsolable>();
            
        services.AddQueueCleaner<NullQueueCleaner>();
            
        return services.AddSingleton<Jobs>();
    }

    public static IServiceCollection AddQueuing(
        this IServiceCollection services, 
        Action<IServiceProvider, IGlobalConfiguration> overrides = null)
    {
        services.AddHangfire((sp, configuration) =>
        {
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseActivator(sp.GetService<MiruJobActivator>());
                
            overrides?.Invoke(sp, configuration);
        });

        services.AddTransient<MiruJobActivator>();
            
        services.AddTransient(sp => new BackgroundJobServer(
            new BackgroundJobServerOptions(),
            sp.GetService<JobStorage>()));

        services.AddSingleton<Jobs>();

        services.AddConsolable<QueueRunConsolable>();
            
        return services;
    }
        
    public static IServiceCollection AddQueueCleaner<TQueueCleaner>(this IServiceCollection services)
        where TQueueCleaner : class, IQueueCleaner
    {
        services.ReplaceTransient<IQueueCleaner, TQueueCleaner>();
            
        return services;
    }
        
    public static void UseLiteDb(this QueuingBuilder builder)
    {
        var storage = builder.ServiceProvider.GetRequiredService<LocalDiskStorage>();
        var env = builder.ServiceProvider.GetRequiredService<IHostEnvironment>();
        var queueOptions = builder.ServiceProvider.GetRequiredService<QueueingOptions>();

        var dbPath = storage.StorageDir / "db" / $"Queue_{env.EnvironmentName}.db";

        dbPath.Dir().EnsureDirExist();
            
        queueOptions.ConnectionString = dbPath;

        builder.Configuration.UseLiteDbStorage(queueOptions.ConnectionString);
    }
}