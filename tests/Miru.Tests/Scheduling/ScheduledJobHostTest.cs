using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Miru.Hosting;
using Miru.Queuing;
using Miru.Scheduling;
using Miru.Security;
using Miru.Sqlite;

namespace Miru.Tests.Scheduling;

public class ScheduledJobIntegratedTest
{
    [Test]
    public async Task Should_process_a_scheduled_task()
    {
        // arrange
        var hostBuilder = MiruHost
            .CreateMiruHost()
            .ConfigureServices(services =>
            {
                services
                    .AddMiruCoreTesting()
                    .AddScheduledJob<ScheduledJobConfig>()
                    .AddLiteDbQueueing()
                    .AddHangfireServer();
            });

        // act
        try
        {
            await hostBuilder.RunMiruAsync();
        }
        catch
        {
            // ignored
        }

        // assert
        Execute.Until(() => SomeTask.Executed, TimeSpan.FromSeconds(2)).ShouldBeTrue();
    }
        
    public class ScheduledJobConfig : IScheduledJobConfig
    {
        public void Configure(ScheduledJobs jobs)
        {
            jobs.Add<SomeTask>(Cron.Monthly(15));

            RecurringJob.TriggerJob("SomeTask");
        }
    }

    public class SomeTask : IScheduledJob
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