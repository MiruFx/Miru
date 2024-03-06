using Hangfire;
using Miru.Domain;
using Miru.Queuing;

namespace Miru.Testing;

public static class QueueingTestFixtureExtensions
{
    public static int EnqueuedCount(this ITestFixture fixture, string queue = "default")
    {
        return fixture.Get<JobStorage>()
            .GetMonitoringApi()
            .EnqueuedJobs(queue, 0, 1000)
            .Count;
    }

    public static bool AnyEnqueuedFor<TQueueable>(this ITestFixture fixture, string queue = "default") 
        where TQueueable : IQueueable
    {
        return fixture.Get<JobStorage>()
            .GetMonitoringApi()
            .EnqueuedJobs(queue, 0, 1000)
            .Select(result => result.Value)
            .Count(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TQueueable)) == 1;
    }
        
    public static TJob EnqueuedFor<TJob>(this ITestFixture fixture, string queue = "default")
    {
        var entry = fixture.Get<JobStorage>()
            .GetMonitoringApi()
            .EnqueuedJobs(queue, 0, 1000)
            .Select(result => result.Value)
            .FirstOrDefault(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TJob));
            
        if (entry == null)
            throw new ShouldAssertException($"No job of type {typeof(TJob).FullName} found at queue '{queue}'");

        return (TJob) entry.Job.Args[0];
    }
        
    public static IEnumerable<TJob> AllEnqueuedFor<TJob>(
        this ITestFixture fixture,
        string queue = "default") where TJob : IQueueable =>
        fixture.Get<JobStorage>()
            .GetMonitoringApi()
            .EnqueuedJobs(queue ?? "default", 0, 1000)
            .Select(result => result.Value)
            .Where(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TJob))
            .Select(enqueueJob => (TJob) enqueueJob.Job.Args[0])
            .ToList();
        
    public static TEvent DomainEventOf<TEvent>(this EntityEventable entity) 
        where TEvent : class, IDomainEvent =>
        entity.DomainEvents
            .OfType<TEvent>()
            .First();
    
    public static TEvent IntegratedEventOf<TEvent>(this EntityEventable entity) 
        where TEvent : class =>
        entity.EnqueueEvents
            .OfType<Func<IIntegratedEvent>>()
            .Select(x => x().GetEvent() as TEvent)
            .First();
    
    public static IEnumerable<TJob> AllScheduled<TJob>(this ITestFixture fixture) 
        where TJob : IQueueable =>
        fixture.Get<JobStorage>()
            .GetMonitoringApi()
            .ScheduledJobs(0, 100)
            .Select(result => result.Value)
            .Where(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TJob))
            .Select(enqueueJob => (TJob) enqueueJob.Job.Args[0])
            .ToList();
    
    public static IEnumerable<object> AllScheduled(this ITestFixture fixture) =>
        fixture.Get<JobStorage>()
            .GetMonitoringApi()
            .ScheduledJobs(0, 100)
            .Select(result => result.Value)
            .Select(enqueueJob => enqueueJob.Job.Args[0])
            .ToList();

    public static int CountScheduled(this ITestFixture fixture) =>
        fixture.Get<JobStorage>()
            .GetMonitoringApi()
            .ScheduledJobs(0, 100)
            .Count;

}