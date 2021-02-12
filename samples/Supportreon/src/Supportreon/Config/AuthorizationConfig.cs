using System.Threading.Tasks;
using Miru;
using Miru.Security;
using Miru.Userfy;
using Supportreon.Domain;

namespace Supportreon.Config
{
    public class AuthorizationConfig : IAuthorizationRules
    {
        // private readonly IUserSession<User> _userSession;
        //
        // public AuthorizationConfig(IUserSession<User> userSession)
        // {
        //     _userSession = userSession;
        // }

        public Task<AuthorizationResult> Evaluate<TRequest>(TRequest request, FeatureInfo feature)
        {
            // if (feature.IsIn("Admin") && (await _userSession.User())?.IsAdmin == false)
            // {
            //     return AuthorizationResult.Fail();
            // }
            
            // if (feature.Implements<IMustBeAuthenticated>() && _userSession.IsAnonymous)
            // {
            //     return Task.FromResult(AuthorizationResult.Fail("Authentication is required"));
            // }
            
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
