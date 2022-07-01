using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Miru.Scheduling;

namespace Miru;

public static class ScheduledJobRegistry
{
    public static IServiceCollection AddScheduledJob<TConfig>(
        this IServiceCollection services,
        Action<ScheduledJobOptions> actionConfig = null) 
            where TConfig : class, IScheduledJobConfig, new()
    {
        services.AddOptions<ScheduledJobOptions>();

        var options = new ScheduledJobOptions
        {
            QueueName = "scheduled"
        };
        
        actionConfig?.Invoke(options);

        services.AddSingleton(options);

        if (options.SkipScheduling == false)
        {
            services.AddHostedService<ScheduledJobHostedService>();
        }

        if (options.SkipRunning == false)
        {
            services.AddHangfireServer(x =>
            {
                x.Queues = new[] { options.QueueName };
                x.WorkerCount = 1;
            });
        }

        services.AddSingleton<IScheduledJobConfig, TConfig>();
        services.AddSingleton<ScheduledJobs>();

        return services;
    }
}