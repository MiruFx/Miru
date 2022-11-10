using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using StackExchange.Profiling;

namespace Miru.Diagnostics;

public class MiniProfilerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly MiruMiniProfilerOptions _options;

    public MiniProfilerBehavior(IOptions<MiruMiniProfilerOptions> options)
    {
        _options = options.Value;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using (MiniProfiler.Current.Step($"Handling {request.GetType()}"))
        {
            if (_options.ShouldIgnore(request) == false)
                MiniProfiler.Current.CustomTiming("Feature Handler", request.Inspect());
                
            var response = await next();
                
            if (_options.ShouldIgnore(response) == false)
                MiniProfiler.Current.CustomTiming("Feature Handler", response.Inspect());
                
            return response;
        }
    }
}