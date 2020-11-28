using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;

namespace Miru.Queuing
{
    public static class QueuesServiceCollectionExtensions
    {
        public static IServiceCollection AddQueuing(
            this IServiceCollection services,
            Action<QueuingBuilder> queuingBuilder)
        {
            services.AddSingleton<QueueOptions>();
            
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
    }
}