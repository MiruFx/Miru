using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Miru.Scheduling
{
    public static class SchedulingServiceCollectionExtensions
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
}