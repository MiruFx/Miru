using System;
using System.CommandLine;
using System.IO;
using System.Threading;
using MediatR;
using Miru.Config;
using Miru.Hosting;
using Miru.Pipeline;
using StringWriter = System.IO.StringWriter;

namespace Miru.Tests.Pipelines;

public class InvokableConsolableTest
{
    private TextWriter _defaultConsoleWriter;
    private StringWriter _outWriter;
    private TextWriter _defaultConsoleError;
    private StringWriter _errorWriter;

    [SetUp]
    public void Setup()
    {
        _defaultConsoleWriter = Console.Out;
        _defaultConsoleError = Console.Error;
            
        _outWriter = new StringWriter();
        _errorWriter = new StringWriter();
            
        Console.SetOut(_outWriter);
        Console.SetError(_errorWriter);
    }

    [TearDown]
    public void Teardown()
    {
        Console.SetOut(_defaultConsoleWriter);
        Console.SetError(_defaultConsoleError);
    }

    [Test]
    public async Task Should_run_invokable_without_args()
    {
        // arrange
        var host = MiruHost.CreateMiruHost("miru", "invoke", "OrderExport").ConfigureServices(x => x
            .AddMiruApp<InvokableConsolableTest>()
            .AddConsolables<ConfigShowConsolable>()
            .AddPipeline<InvokableConsolableTest>());
                
        // act
        await host.RunMiruAsync();
            
        // assert
        var output = _outWriter.ToString();
            
        Console.WriteLine(output);

        output.ShouldContain("Exporting");
    }

    [Test]
    public async Task Should_run_invokable_parsing_args_to_create_command_instance()
    {
        // arrange
        var host = MiruHost.CreateMiruHost("miru", "invoke", "OrderArchive", "--orderId", "123", "--archiveType", "Temporarily").ConfigureServices(x => x
            .AddMiruApp<InvokableConsolableTest>()
            .AddConsolables<ConfigShowConsolable>()
            .AddPipeline<InvokableConsolableTest>());
                
        // act
        await host.RunMiruAsync();
            
        // assert
        var output = _outWriter.ToString();
            
        output.ShouldContain("123 Temporarily");
    }

    public class OrderArchive
    {
        public class Command : Invokable, IRequest<Command>
        {
            public Command()
            {
                Add(new Option<long>("--orderId"));
                Add(new Option<ArchiveType>("--archiveType"));
            }

            public long OrderId { get; set; }
            public ArchiveType ArchiveType { get; set; }
        }
            
        public class Handler : IRequestHandler<Command, Command>
        {
            public async Task<Command> Handle(Command request, CancellationToken cancellationToken)
            {
                Console.WriteLine($"{request.OrderId} {request.ArchiveType}");
                return await Task.FromResult(request);
            }
        }
    }

    public enum ArchiveType
    {
        Permanent,
        Temporarily
    }
    
    public class OrderExport
    {
        public class Command : IRequest<Command>, IInvokable
        {
        }
            
        public class Handler : IRequestHandler<Command, Command>
        {
            public async Task<Command> Handle(Command request, CancellationToken cancellationToken)
            {
                Console.WriteLine($"Exporting");
                return await Task.FromResult(request);
            }
        }
    }
}