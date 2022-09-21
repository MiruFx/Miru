using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using MediatR;

namespace Miru.Queuing;

public class JobFor<TRequest>
{
    private readonly IMiruApp _app;

    public JobFor(IMiruApp app)
    {
        _app = app;
    }

    [DisplayName("{0}")]
    [Queue("{3}")]
    public async Task Execute(
        TRequest request, 
        CancellationToken ct, 
        PerformContext performContext, 
        string queueName)
    {
        App.Framework.Information("Creating scope and processing Job {Request}", request);
        
        await _app.ScopedSendAsync(request as IBaseRequest, ct);
    }
}