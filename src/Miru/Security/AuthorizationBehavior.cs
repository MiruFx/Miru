using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru.Queuing;

namespace Miru.Security;

public class AuthorizationBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IAuthorizationRules _rules;

    public AuthorizationBehavior(IAuthorizationRules rules)
    {
        _rules = rules;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var featureInfo = new FeatureInfo(typeof(TRequest));
            
        // always ignore requests made inside the queues
        if (request is IQueueable)
            return await next();
            
        var result = await _rules.Evaluate(request, featureInfo);
                
        if (result.IsAuthorized)
            return await next();

        throw new UnauthorizedException(result.FailureMessage);
    }
}