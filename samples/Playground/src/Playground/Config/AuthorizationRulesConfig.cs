using Miru;
using Miru.Security;
using Playground.Domain;

namespace Playground.Config;

public class AuthorizationRulesConfig : AuthorizationRules
{
    protected override AuthorizationResult Evaluate<TRequest>(TRequest request, FeatureInfo feature)
    {
        return AuthorizationResult.Succeed();
    }
}