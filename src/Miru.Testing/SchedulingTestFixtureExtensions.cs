using System.Linq;
using Miru.Scheduling;
using Quartz;
using Quartz.Impl.Matchers;

namespace Miru.Testing
{
    public static class SchedulingTestFixtureExtensions
    {
        public static bool ScheduledOneTaskFor<TTask>(this ITestFixture fixture) where TTask : ScheduledTask
        {
            var scheduler = fixture.Get<Quartz.IScheduler>();
            var triggerKeys = scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
            
          //  triggerKeys.Result.Where(r => scheduler.GetTrigger(r))
            
            foreach (var triggerKey in triggerKeys.Result)
            {
                var triggerdetails =   scheduler.GetTrigger(triggerKey);
                var Jobdetails = scheduler.GetJobDetail(triggerdetails.Result.JobKey);
                
            }
            //Pegar o Jobdetails e filtar por tipo == typeof(TTask) 
            return true;
        }
    }
}