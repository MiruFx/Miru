using System.Threading.Tasks;
using Quartz;

namespace Miru.Scheduling
{
    public class Tasks 
    {
        private readonly IScheduler _scheduler;

        public Tasks(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task Schedule<TTask>(string cronExpression) where TTask : ScheduledTask
        {
            var jobDetails = JobBuilder
                .Create<TTask>()
                .Build();
        
            var trigger = TriggerBuilder
                .Create()
                .StartNow()
                .WithCronSchedule(cronExpression)
                .Build();

            return _scheduler.ScheduleJob(jobDetails, trigger);
        }
    }
}