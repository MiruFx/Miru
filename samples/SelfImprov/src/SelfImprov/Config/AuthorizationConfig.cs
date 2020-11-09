using System.Threading.Tasks;
using Miru;
using Miru.Security;
using Miru.Userfy;
using SelfImprov.Domain;

namespace SelfImprov.Config
{
    public class AuthorizationConfig : IAuthorizationRules
    {
        private readonly IUserSession<User> _userSession;

        public AuthorizationConfig(IUserSession<User> userSession)
        {
            _userSession = userSession;
        }

        public Task<AuthorizationResult> Evaluate<TRequest>(TRequest request, FeatureInfo feature)
        {
            if (feature.IsIn("Accounts"))
                return Task.FromResult(AuthorizationResult.Succeed());
            
            if (_userSession.IsAnonymous)
                return Task.FromResult(AuthorizationResult.Fail("Authentication is required"));
            
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
