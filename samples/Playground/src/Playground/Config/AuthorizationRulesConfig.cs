using Miru;
using Miru.Security;
using Playground.Domain;

namespace Playground.Config;

public class AuthorizationRulesConfig : AuthorizationRules
{
    private readonly Current _current;
        
    public AuthorizationRulesConfig(Current current) => _current = current;

    protected override AuthorizationResult Evaluate<TRequest>(TRequest request, FeatureInfo feature)
    {
        // if (_current.IsAuthenticated == false)
        // {
        //     return AuthorizationResult.Fail("Authentication is required");
        // }

        return AuthorizationResult.Succeed();
    }
}