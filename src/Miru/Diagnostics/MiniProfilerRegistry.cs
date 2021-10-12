using System;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Diagnostics
{
    public static class MiniProfilerRegistry
    {
        public static IServiceCollection AddMiruMiniProfiler(
            this IServiceCollection services, 
            Action<MiruMiniProfilerOptions> optionsConfig = null)
        {
            services.AddOptions<MiruMiniProfilerOptions>();
            
            services.Configure(optionsConfig);
            
            services.AddMiniProfiler(x =>
                {
                    x.TrackConnectionOpenClose = false;
                })
                .AddEntityFramework();
            
            return services;
        }
    }
}