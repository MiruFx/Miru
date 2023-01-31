using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using Miru.Tests.Queuing;

namespace Miru.Tests.Testing;

public class QueueingExtensionsTest : MiruCoreTest
{
    private Jobs _jobs;

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddQueuing(x => x.Configuration.UseMemoryStorage())
            .AddPipeline<QueueingTest>()
            .AddHangfireServer()
            .AddSingleton<BackgroundJobServer>();
    }
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _jobs = _.Get<Jobs>();
    }

    [Test]
    public void Should_return_a_enqueued_job()
    {
        // arrange
        var job = new OrderCreated
        {
            OrderId = 123
        };
            
        // act
        _jobs.Enqueue(job);
            
        // assert
        _.EnqueuedFor<OrderCreated>().OrderId.ShouldBe(123);
    }

    public class OrderCreated : MiruJob<OrderCreated>
    {
        public long OrderId { get; set; }
    }
}