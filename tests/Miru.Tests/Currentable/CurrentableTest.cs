using System.Threading;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Currentable;

namespace Miru.Tests.Currentable;

// TODO: tests for default current
public class CurrentableTest
{
    private ITestFixture _fix;

    [OneTimeSetUp]
    public void Setup()
    {
        _fix = new ServiceCollection()
            .AddMiruApp()
            .AddPipeline<CurrentableTest>(_ =>
            {
                _.UseBehavior(typeof(CurrentBehavior<,>));
            })
            .AddMiruCoreTesting()
            
            .AddCurrent<Current, CurrentHandler>()
            
            .BuildServiceProvider()

            .GetService<ITestFixture>();
    }

    [Test]
    public void Should_process_current_scope()
    {
        // arrange
        using var sp = _fix.WithScope();
        
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

    public class CurrentHandler(Current current) : ICurrentHandler
    {
        public async Task Handle<TRequest>(TRequest request, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            current.Processed = true;
        }
    }

    public class Command : IRequest<Result>
    {
    }

    public class Result
    {
    }

    public class Handler(Current current) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            current.Processed.ShouldBeTrue();
            
            return await Task.FromResult(new Result());
        }
    }
}
