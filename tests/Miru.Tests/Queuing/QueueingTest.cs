using System;
using System.Threading;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Logging;
using Miru.Queuing;

namespace Miru.Tests.Queuing;

[Ignore("WIP")]
public class QueueingTest : MiruCoreTesting
{
    private BackgroundJobServer _server;
    private Jobs _jobs;

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddQueuing(x => x.Configuration.UseMemoryStorage())
            .AddPipeline<QueueingTest>()
            .AddScoped<SomeService>()
            .AddHangfireServer()
            .AddSingleton<BackgroundJobServer>()
            .AddSerilogConfig(x =>
            {
                x.MinimumLevel.Information();
                x.WriteToTestConsole();
            });
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _server = _.Get<BackgroundJobServer>();
        _jobs = _.Get<Jobs>();
    }

    [OneTimeTearDown]
    public void OneTearDown()
    {
        _server?.Dispose();
    }
        
    [Test]
    public void Should_process_job()
    {
        var job = new CustomerNew
        {
            CustomerId = 123
        };
            
        _jobs.Enqueue(job);
            
        Execute.Until(() => CustomerNew.Processed, TimeSpan.FromSeconds(2));

        CustomerNew.Processed.ShouldBeTrue();
    }

    [Test]
    public void Services_instance_should_be_scoped()
    {
        var job = new ScopedJob();
            
        _jobs.Enqueue(job);
            
        Execute.Until(() => ScopedJob.Processed, TimeSpan.FromSeconds(2));

        ScopedJob.Processed.ShouldBeTrue();
    }
    
    public class SomeService
    {
    }

    public class CustomerNew : IMiruJob
    {
        // job info
        public long CustomerId { get; set; }

        // for the test
        public static bool Processed { get; set; }

        public class Handler : IRequestHandler<CustomerNew>
        {
            public Task<Unit> Handle(CustomerNew request, CancellationToken cancellationToken)
            {
                request.CustomerId.ShouldBe(123);

                Processed = true;

                return Unit.Task;
            }
        }
    }

    public class ScopedJob : IMiruJob
    {
        public static bool Processed { get; set; }
        
        public class ScopedHandler : JobHandler<ScopedJob>
        {
            private readonly IServiceProvider _serviceProvider;

            public ScopedHandler(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            protected override void Handle(ScopedJob request)
            {
                var service1 = _serviceProvider.GetService<SomeService>();
                var service2 = _serviceProvider.GetService<SomeService>();
                
                service1.ShouldBe(service2);

                Processed = true;
            }
        }
    }
}
    
