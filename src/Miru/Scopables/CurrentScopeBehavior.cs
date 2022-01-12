using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Scopables;

public class CurrentScopeBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse>
{
    private readonly ICurrentScope _currentScope;

    public CurrentScopeBehavior(ICurrentScope currentScope)
    {
        _currentScope = currentScope;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
    {
        await _currentScope.BeforeAsync(ct);

        var result = await next();
        
        await _currentScope.AfterAsync(ct);

        return result;
    }
}