using System;
using Hangfire;
using Hangfire.LiteDB;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using Miru.Sqlite;
using Miru.Storages;
using Miru.Tests.Queuing;

namespace Miru.Tests.Testing;

public class QueueingExtensionsTest
{
    private ServiceProvider _sp;
    private BackgroundJobServer _server;
    private Jobs _jobs;
    private ITestFixture _;

    [OneTimeSetUp]
    public void Setup()
    {
        _sp = new ServiceCollection()
            .AddMiruApp()
            .AddMiruSolution(new MiruTestSolution())
            .AddStorage()
            .AddTestStorage()
            .AddQueuing(x => x.UseLiteDb())
            .AddHangfireServer()
            .AddMediatR(typeof(QueueingTest).Assembly)
            .AddScoped<SomeService>()
            .AddMiruCoreTesting()
            .BuildServiceProvider();

        _ = _sp.GetService<ITestFixture>();

        // _server = _sp.GetService<BackgroundJobServer>();

        _jobs = _sp.GetService<Jobs>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        // _server.Dispose();
    }
        
    [Test]
    public void Should_process_job()
    {
        // arrange
        var job = new OrderCreated
        {
            OrderId = 123
        };
            
        // act
        _jobs.Enqueue(job);
            
        // assert
        _.EnqueuedJob<OrderCreated>().OrderId.ShouldBe(123);
    }

    public class OrderCreated : IMiruJob
    {
        public long OrderId { get; set; }
    }
}