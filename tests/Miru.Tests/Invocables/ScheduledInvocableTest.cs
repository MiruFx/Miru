using System;
using System.Threading;
using Hangfire;
using MediatR;
using Miru.Hosting;
using Miru.Pipeline;
using Miru.Scheduling;
using Miru.Sqlite;

namespace Miru.Tests.Invocables;

public class ScheduledInvocableTest
{
    [Test]
    public async Task Should_run_job_through_the_scheduler()
    {
        // arrange
        var hostBuilder = MiruHost
            .CreateMiruHost()
            .ConfigureServices(services =>
            {
                services
                    .AddMiruCoreTesting()
                    .AddPipeline<ScheduledInvocableTest>()
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
        Execute.Until(() => OrderDeliveryTracking.Executed, TimeSpan.FromSeconds(2)).ShouldBeTrue();
    }
    
    [Test]
    public async Task Should_run_job_through_the_command_line()
    {
        // arrange
        var hostBuilder = MiruHost.CreateMiruHost("miru", "invoke", "OrderDeliveryTracking")
            .ConfigureServices(services =>
            {
                services
                    .AddMiruCoreTesting()
                    .AddMiruApp<ScheduledInvocableTest>()
                    .AddPipeline<ScheduledInvocableTest>()
                    .AddConsolable<InvokeConsolable>();
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
        Execute.Until(() => OrderDeliveryTracking.Executed, TimeSpan.FromSeconds(2)).ShouldBeTrue();
    }
    
    public class ScheduledJobConfig : IScheduledJobConfig
    {
        public void Configure(ScheduledJobs jobs)
        {
            jobs.Add(new OrderDeliveryTracking.Command(), Cron.Hourly());
            
            RecurringJob.TriggerJob("OrderDeliveryTrackingJob");
        }
    }

    public class OrderDeliveryTracking
    {
        public static bool Executed { get; set; }

        public class Command : IRequest<Command>
        {
        }

        public class Handler : IRequestHandler<Command, Command>
        {
            private readonly IHostApplicationLifetime _lifetime;

            public Handler(IHostApplicationLifetime lifetime)
            {
                _lifetime = lifetime;
            }

            public async Task<Command> Handle(Command request, CancellationToken ct)
            {
                Executed = true;
                
                _lifetime.ApplicationStarted.Register(_lifetime.StopApplication);
                
                return await Task.FromResult(request);
            }
        }
    }
}