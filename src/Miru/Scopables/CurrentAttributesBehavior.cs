using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Scopables;

public class CurrentAttributesBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICurrentAttributes _currentScope;

    public CurrentAttributesBehavior(ICurrentAttributes currentScope)
    {
        _currentScope = currentScope;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
    {
        await _currentScope.BeforeAsync(request, ct);

        var result = await next();
        
        await _currentScope.AfterAsync(request, ct);

        return result;
    }
}