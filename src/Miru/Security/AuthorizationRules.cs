using System.Threading.Tasks;

namespace Miru.Security;

public abstract class AuthorizationRules : IAuthorizationRules
{
    Task<AuthorizationResult> IAuthorizationRules.Evaluate<TRequest>(TRequest request, FeatureInfo feature)
        => Task.FromResult(Evaluate(request, feature));
    
    protected abstract AuthorizationResult Evaluate<TRequest>(TRequest request, FeatureInfo feature);
}