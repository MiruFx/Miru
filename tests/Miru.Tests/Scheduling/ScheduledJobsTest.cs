using System.Threading;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using Miru.Scheduling;
using Miru.Sqlite;

namespace Miru.Tests.Scheduling;

public class ScheduledJobsTest : MiruCoreTesting
{
    private ScheduledJobs _jobs;

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddLiteDbQueueing()
            .AddHangfireServer()
            .AddScheduledJob<ScheduledJobConfig>();
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        JobStorage.Current = _.Get<JobStorage>();
        
        _jobs = _.Get<ScheduledJobs>();
    }

    [SetUp]
    public void Setup()
    {
        _.ClearQueue();
    }
    
    [Test]
    public void Should_add_a_scheduled_job()
    {
        // act
        _jobs.Add(new SomeTask.Command(), Cron.Daily(15));
        
        // assert
        var allJobs = _.Get<JobStorage>().GetConnection().GetRecurringJobs();

        allJobs.ShouldCount(1);
        allJobs.First().Id.ShouldBe("SomeTask");
    }
    
    [Test]
    public void Should_add_a_scheduled_job_with_suffix()
    {
        // act
        _jobs.Add(new SomeTask.Command(), Cron.Daily(15), suffix: "15th");
        
        // assert
        var allJobs = _.Get<JobStorage>().GetConnection().GetRecurringJobs();

        allJobs.ShouldCount(1);
        allJobs.First().Id.ShouldBe("SomeTask-15th");
    }
    
    public class ScheduledJobConfig : IScheduledJobConfig
    {
        public void Configure(ScheduledJobs jobs)
        {
        }
    }
    
    public class SomeTask
    {
        public class Command : MiruJob<Command>, IScheduledJob
        {
        }

        public class Handler : JobHandler<Command>
        {
            public override async Task Handle(Command request, CancellationToken ct)
            {
                await Task.CompletedTask;
            }
        }
    }
}