using System;
using System.IO;
using System.Threading;
using MediatR;
using Miru.Config;
using Miru.Hosting;
using Miru.Pipeline;
using Miru.Queuing;
using StringWriter = System.IO.StringWriter;

namespace Miru.Tests.Pipelines;

public class NotifyConsolableTest
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
    public async Task Should_notify_and_publish_a_notification_request()
    {
        // arrange
        var host = MiruHost.CreateMiruHost(
                "miru", "notify", "OrderCreated", "--OrderId", "123", "-e", "Production")
            .ConfigureServices(x => x
                .AddMiruApp<NotifyConsolableTest>()
                .AddConsolables<ConfigShowConsolable>()
                .AddPipeline<NotifyConsolableTest>());
                
        // act
        await host.RunMiruAsync();
            
        // assert
        var output = _outWriter.ToString();
            
        output.ShouldContain("Notified OrderCreated #123");
    }

    public class OrderCreated
    {
        public class Notification : IntegratedEvent2
        {
            public long OrderId { get; set; }
        }
        
        public class Handler : INotificationHandler<Notification>
        {
            public async Task Handle(Notification request, CancellationToken cancellationToken)
            {
                Console.WriteLine($"Notified OrderCreated #{request.OrderId}");

                await Task.CompletedTask;
            }
        }
    }
}