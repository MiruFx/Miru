using System.Threading.Tasks;
using Miru;
using Miru.Security;
using Miru.Userfy;
using Mong.Domain;

namespace Mong.Config
{
    public class AuthorizationConfig : IAuthorizationRules
    {
        private readonly IUserSession<User> _userSession;

        public AuthorizationConfig(IUserSession<User> userSession)
        {
            _userSession = userSession;
        }

        public async Task<AuthorizationResult> Evaluate<TRequest>(TRequest request, FeatureInfo feature)
        {
            if (feature.IsIn("Admin"))
            {
                if (_userSession.IsAnonymous || (await _userSession.GetUserAsync())?.IsAdmin == false)
                {
                    return AuthorizationResult.Fail("Unauthorized access");
                }
            }

            if (feature.Implements<IMustBeAuthenticated>() && _userSession.IsAnonymous)
            {
                return AuthorizationResult.Fail("Authentication is required");
            }
            
            return AuthorizationResult.Succeed();
        }
    }
}