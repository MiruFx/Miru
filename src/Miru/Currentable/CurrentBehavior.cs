using MediatR;

namespace Miru.Currentable;

public class CurrentBehavior<TRequest, TResponse>(ICurrentHandler handler) :
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        await handler.Handle(request, ct);

        return await next();
    }
}