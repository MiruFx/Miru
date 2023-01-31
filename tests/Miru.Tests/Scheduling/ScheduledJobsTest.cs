using System.Threading;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using Miru.Scheduling;
using Miru.Sqlite;

namespace Miru.Tests.Scheduling;

public class ScheduledJobsTest : MiruCoreTest
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
    public void If_job_exists_then_should_update_cron()
    {
        // arrange
        _jobs.Add(new SomeTask.Command(), Cron.Daily(1));
        
        // act
        _jobs.Add(new SomeTask.Command(), Cron.Daily(10));
        
        // assert
        var allJobs = _jobs.GetAll();

        allJobs.ShouldCount(1);
        
        var savedJob = allJobs.First();
        savedJob.Id.ShouldBe("SomeTask");
        savedJob.Cron.ShouldBe(Cron.Daily(10));
    }
    
    [Test]
    public void If_there_are_jobs_not_in_scheduled_job_config_then_it_should_be_deleted()
    {
        // arrange
        RecurringJob.AddOrUpdate<JobFor<ObsoleteTask.Command>>(
            "ObsoleteTask", m => m.Execute(new ObsoleteTask.Command(), default, null, "default"), Cron.Daily);
        
        // act
        _jobs.DeleteAllObsolete();
        
        // assert
        var allJobs = _jobs.GetAll();

        allJobs.ShouldBeEmpty();
    }
    
    [Test]
    public void If_there_are_configured_jobs_then_should_delete_only_obsoletes()
    {
        // arrange in the Setup()
        RecurringJob.AddOrUpdate<JobFor<ObsoleteTask.Command>>(
            "ObsoleteTask", m => m.Execute(new ObsoleteTask.Command(), default, null, "default"), Cron.Daily);
        
        _jobs.Add(new SomeTask.Command(), Cron.Daily(1));
        
        // act
        _jobs.DeleteAllObsolete();
        
        // assert
        var allJobs = _jobs.GetAll();

        allJobs.ShouldCount(1);
        
        var savedJob = allJobs.First();
        savedJob.Id.ShouldBe("SomeTask");
        savedJob.Cron.ShouldBe(Cron.Daily(1));
    }
    
    [Test]
    public void Should_add_a_scheduled_job_with_suffix()
    {
        // act
        _jobs.Add(new SomeTask.Command(), Cron.Daily(15), suffix: "15th");
        
        // assert
        var allJobs = _jobs.GetAll();

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
    
    public class SomeOtherTask
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
    
    public class ObsoleteTask
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