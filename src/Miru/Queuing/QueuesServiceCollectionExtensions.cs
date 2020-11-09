using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Queuing
{
    public static class QueuesServiceCollectionExtensions
    {
        public static IServiceCollection AddQueuing(
            this IServiceCollection services,
            Action<QueuingBuilder> queuingBuilder)
        {
            services.AddHangfire((sp, configuration) =>
            {
                configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseActivator(sp.GetService<MiruJobActivator>());
                
                var builder = new QueuingBuilder(sp, configuration);
                
                queuingBuilder.Invoke(builder);
            });
            
            services.AddTransient<MiruJobActivator>();
            
            services.AddTransient(sp => new BackgroundJobServer(
                new BackgroundJobServerOptions(),
                sp.GetService<JobStorage>()));

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

            return services;
        }
    }
}