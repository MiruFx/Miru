using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Miru.Scheduling
{
    public static class TasksSchedulingServiceCollectionExtensions
    {
        public static IServiceCollection AddTaskScheduling<TSchedule>(this IServiceCollection services) 
            where TSchedule : IScheduledTaskConfiguration, new()
        {
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            
            services.AddQuartz(q =>
            {
                var taskConfig = new TSchedule();
                taskConfig.Configure(q);
            });
            
            return services;
        }
    }
}