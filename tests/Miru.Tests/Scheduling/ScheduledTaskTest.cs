using Miru.Scheduling;
using Quartz;

namespace Miru.Tests.Scheduling;

public class ScheduledTaskTest
{
    [Test]
    public async Task Should_process_a_scheduled_task()
    {
        // arrange
        var hostBuilder = MiruHost
            .CreateMiruHost()
            .ConfigureServices(services =>
            {
                services.AddMiruTestFixture()
                    .AddTaskScheduling<ScheduledTaskConfig>();
            });

        // act
        await hostBuilder.RunMiruAsync();
            
        SomeTask.Executed.ShouldBeTrue();
    }
        
    public class ScheduledTaskConfig : IScheduledTaskConfiguration
    {
        public void Configure(IServiceCollectionQuartzConfigurator configurator)
        {
            configurator.ScheduleJob<SomeTask>(
                trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithRepeatCount(0))
            );
        }
    }

    public class SomeTask : IScheduledTask
    {
        private readonly IHostApplicationLifetime _lifetime;
            
        public static bool Executed { get; set; }

        public SomeTask(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public Task ExecuteAsync()
        {
            Executed = true;
                
            _lifetime.ApplicationStarted.Register(_lifetime.StopApplication);
                
            return Task.CompletedTask;
        }
    }
}