using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Foundation.Logging;

public class DumpRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.LogIt();
        var response = await next();
        return response;
    }
}