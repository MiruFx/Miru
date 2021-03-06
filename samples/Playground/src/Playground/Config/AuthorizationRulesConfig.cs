using System.Threading.Tasks;
using Miru;
using Miru.Security;
using Miru.Userfy;
using Playground.Domain;

namespace Playground.Config
{
    public class AuthorizationRulesConfig : IAuthorizationRules
    {
        private readonly IUserSession<User> _userSession;
        
        public AuthorizationRulesConfig(IUserSession<User> userSession) => _userSession = userSession;

        public async Task<AuthorizationResult> Evaluate<TRequest>(TRequest request, FeatureInfo feature)
        {
            // if (_userSession.IsLogged)
            // {
            //     var user = await _userSession.GetUserAsync();
            //     
            //     if (feature.IsIn("Admin") && user?.IsAdmin == false)
            //         return AuthorizationResult.Fail();
            // }
            //
            // if (feature.Implements<IMustBeAuthenticated>() && _userSession.IsAnonymous)
            //     return AuthorizationResult.Fail("Authentication is required");
            
            // return AuthorizationResult.Succeed();

            return await Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
