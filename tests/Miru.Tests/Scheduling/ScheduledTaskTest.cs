using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Scheduling;
using Miru.Testing;
using NUnit.Framework;
using Quartz;
using Shouldly;

namespace Miru.Tests.Scheduling
{
    public class ScheduledTaskTest
    {
        private ServiceProvider _sp;
        private ITestFixture _;
        private IHostedService _server;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _sp = new ServiceCollection()
                .AddMediatR(typeof(ScheduledTaskTest).Assembly)
                .AddMiruTestFixture()
                .AddTaskScheduling<MockScheduledTaskConfiguration>()
                .BuildServiceProvider();

            _ = _sp.GetService<ITestFixture>();
            _server = _sp.GetService<IHostedService>();
            
            _server?.StartAsync(default);
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _server.StopAsync(default);
        }

        [Test]
        public void Should_Process_ScheduledTask()
        {
            Execute.Until(() => SomeTask.Processed, TimeSpan.FromSeconds(15));
            SomeTask.Processed.ShouldBeTrue();
        }
        
        public class MockScheduledTaskConfiguration : IScheduledTaskConfiguration
        {
            public void Configure(IServiceCollectionQuartzConfigurator configurator)
            {
                configurator.ScheduleJob<SomeTask>(
                    trigger => trigger
                        .StartNow() 
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(1)
                            .RepeatForever())
                );
            }
        }

        public class SomeTask : ScheduledTask
        {
            public static bool Processed { get; set; }
            
            protected override void Execute()
            {
                Processed = true;
            }
        }

    }
}