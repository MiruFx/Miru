using System;
using System.Threading;
using Hangfire;
using Miru.Hosting;
using Miru.Queuing;
using Miru.Scheduling;
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
                    .AddMiruApp<ScheduledJobIntegratedTest>()
                    .AddMiruCoreTesting()
                    .AddScheduledJob<ScheduledJobConfig>()
                    .AddLiteDbQueueing()
                    .AddPipeline<ScheduledJobIntegratedTest>()
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
            var jobId = jobs.Add(new SomeTask.Command(), Cron.Monthly(15));

            RecurringJob.TriggerJob(jobId);
        }
    }
}

public class SomeTask
{
    public static bool Executed { get; set; }

    public class Command : MiruJob<Command>, IScheduledJob
    {
    }

    public class Handler : JobHandler<Command>
    {
        private readonly IHostApplicationLifetime _lifetime;

        public Handler(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public override async Task Handle(Command request, CancellationToken ct)
        {
            Executed = true;
                
            _lifetime.ApplicationStarted.Register(_lifetime.StopApplication);
                
            await Task.CompletedTask;
        }
    }
}