using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace Miru.Scheduling
{
    public static class TasksSchedulingServiceCollectionExtensions
    {
        public static IServiceCollection AddTaskScheduling<TSchedule>(this IServiceCollection services) where TSchedule : ScheduledTask
        {
            /*services.AddHostedService<TaskHostedService>();
            
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            
            scheduler.JobFactory = new TaskFactory(services.BuildServiceProvider());
            
            services.AddSingleton(scheduler);
            
            return services.AddSingleton<Tasks>();*/
            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
    
            // register quartz adding tasks schedules
            services.AddQuartz(q =>
            {
                var tasksConfig = (TSchedule)Activator.CreateInstance(typeof(TSchedule));
                tasksConfig?.Config(q);
            });

            return services;
        }
    }
}