using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Queuing
{
    public class JobsTest
    {
        private ServiceProvider _sp;
        private BackgroundJobServer _server;
        private Jobs _jobs;
        private ITestFixture _;

        [OneTimeSetUp]
        public void Setup()
        {
            _sp = new ServiceCollection()
                .AddQueuing((sp, cfg) => cfg.UseMemoryStorage())
                .AddMediatR(typeof(JobsTest).Assembly)
                .AddScoped<SomeService>()
                .AddMiruTestFixture()
                .BuildServiceProvider();

            _ = _sp.GetService<ITestFixture>();

            _server = _sp.GetService<BackgroundJobServer>();

            _jobs = _sp.GetService<Jobs>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _server.Dispose();
        }
        
        [Test]
        public void Should_process_job()
        {
            var job = new CustomerNew
            {
                CustomerId = 123
            };
            
            _jobs.PerformLater(job);
            
            Execute.Until(() => CustomerNew.Processed, TimeSpan.FromSeconds(1));

            CustomerNew.Processed.ShouldBeTrue();
        }

        [Test]
        public void Services_instance_should_be_scoped()
        {
            var job = new ScopedJob();
            
            _jobs.PerformLater(job);
            
            Execute.Until(() => ScopedJob.Processed, TimeSpan.FromSeconds(1));

            ScopedJob.Processed.ShouldBeTrue();
        }
    }
    public class SomeService
    {
    }

    public class CustomerNew : IJob
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

    public class ScopedJob : IJob
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