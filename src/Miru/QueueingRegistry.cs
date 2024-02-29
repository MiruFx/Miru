using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Extensions.Serilog;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Miru.Foundation.Logging;
using Miru.Queuing;

namespace Miru;

public static class QueueingRegistry
{
    public static IServiceCollection AddQueuing(
        this IServiceCollection services,
        Action<QueuingBuilder> queuingBuilder)
    {
        services
            .AddHangfire((sp, configuration) =>
            {
                configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings();
                
                var builder = new QueuingBuilder(sp, configuration, services);

                // .UseConsole registers routes into hangfire dashboard statically
                // So in automated tests, when queue is registered in different ServiceProvider
                // instances, UseConsole unfairly throws InvalidOperationException
                // try
                // {
                //     configuration
                //         .UseSerilogLogProvider()
                //         .UseColouredConsoleLogProvider();
                // }
                // catch (InvalidOperationException)
                // {
                // }
                
                queuingBuilder?.Invoke(builder);
            });
        
        services.TryAddSingleton<QueueingOptions>();
        services.TryAddSingleton<Jobs>();

        services
            .AddConsolable<QueueRunConsolable>()
            .AddQueueCleaner<NullQueueCleaner>()
            .AddHangfireConsoleExtensions()
            .AddSerilogConfig(x =>
            {
                x.Enrich.WithHangfireContext();
                x.WriteTo.Hangfire();
            });
        
        return services;
    }

    public static IServiceCollection AddQueueCleaner<TQueueCleaner>(this IServiceCollection services)
        where TQueueCleaner : class, IQueueCleaner
    {
        services.ReplaceTransient<IQueueCleaner, TQueueCleaner>();
            
        return services;
    }
}