using System;
using System.Threading;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Logging;
using Miru.Queuing;

namespace Miru.Tests.Queuing;

public class QueueingTest : MiruCoreTest
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
                x.MinimumLevel.Fatal();
                x.WriteToTestConsole();
            });
    }

    [OneTimeSetUp]
    public void SetupOnce()
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

    public class CustomerNew : MiruJob<CustomerNew>
    {
        // job info
        public long CustomerId { get; set; }

        // for the test
        public static bool Processed { get; set; }

        public class Handler : IRequestHandler<CustomerNew, CustomerNew>
        {
            public Task<CustomerNew> Handle(CustomerNew request, CancellationToken cancellationToken)
            {
                request.CustomerId.ShouldBe(123);

                Processed = true;

                return Task.FromResult(request);
            }
        }
    }

    public class ScopedJob : MiruJob<ScopedJob>
    {
        public static bool Processed { get; set; }
        
        public class ScopedHandler : IRequestHandler<ScopedJob, ScopedJob>
        {
            private readonly IServiceProvider _serviceProvider;

            public ScopedHandler(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }
            
            public async Task<ScopedJob> Handle(ScopedJob request, CancellationToken cancellationToken)
            {
                var service1 = _serviceProvider.GetService<SomeService>();
                var service2 = _serviceProvider.GetService<SomeService>();
                
                service1.ShouldBe(service2);

                Processed = true;

                return await Task.FromResult(request);
            }
        }
    }
}
    
