using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Foundation.Logging
{
    public class DumpRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            request.LogIt();
            var response = await next();
            return response;
        }
    }
}