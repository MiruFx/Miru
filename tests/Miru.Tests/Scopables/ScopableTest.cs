using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Pipeline;
using Miru.Queuing;
using Miru.Scopables;
using Miru.Security;
using Miru.Testing;
using Miru.Tests.Queuing;
using Miru.Userfy;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Scopables;

public class ScopableTest
{
    private ITestFixture _;

    [OneTimeSetUp]
    public void Setup()
    {
        _ = new ServiceCollection()
            .AddMiruApp()
            .AddPipeline<ScopableTest>(_ =>
            {
                _.UseBehavior(typeof(CurrentAttributesBehavior<,>));
            })
            .AddMiruCoreTesting()
            
            .AddCurrentAttributes<Current, CurrentScope>()
            
            .BuildServiceProvider()

            .GetService<ITestFixture>();
    }

    [Test]
    public void Should_process_current_scope()
    {
        // arrange
        using var sp = _.WithScope();
        
        // act
        sp.Get<IMediator>().Send(new Command());
        
        // assert
        sp.Get<Current>().Processed.ShouldBeTrue();
    }

    // TODO: implement
    // [Test]
    // public void Services_instance_should_be_scoped()
    // {
    //     var job = new ScopedJob();
    //         
    //     _jobs.PerformLater(job);
    //         
    //     Execute.Until(() => ScopedJob.Processed, TimeSpan.FromSeconds(1));
    //
    //     ScopedJob.Processed.ShouldBeTrue();
    // }
        
    public class Current
    {
        public bool Processed { get; set; }
    }

    public class CurrentScope : ICurrentAttributes
    {
        private readonly Current _current;

        public CurrentScope(Current current)
        {
            _current = current;
        }

        public async Task BeforeAsync<TRequest>(TRequest request, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            _current.Processed = true;
        }
    }

    public class Command : IRequest<Result>
    {
    }

    public class Result
    {
    }

    public class Handler : RequestHandler<Command, Result>
    {
        private readonly Current _current;

        public Handler(Current current)
        {
            _current = current;
        }

        protected override Result Handle(Command request)
        {
            _current.Processed.ShouldBeTrue();
            
            return new Result();
        }
    }
}
