using Microsoft.Extensions.DependencyInjection;
using Miru.Scheduling;
using Quartz;

namespace Miru;

public static class SchedulingRegistry
{
    public static IServiceCollection AddTaskScheduling<TScheduleConfig>(this IServiceCollection services) 
        where TScheduleConfig : IScheduledTaskConfiguration, new()
    {
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
            
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            var taskConfig = new TScheduleConfig();
            taskConfig.Configure(q);
        });
            
        return services;
    }
}