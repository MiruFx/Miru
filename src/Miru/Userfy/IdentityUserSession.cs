using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy
{
    public class IdentityUserSession<TUser> : IUserSession where TUser : UserfyUser
    {
        private readonly SignInManager<TUser> _signInManager;

        public IdentityUserSession(SignInManager<TUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(string userName, string password, bool remember = false)
        {
            return await _signInManager.PasswordSignInAsync(
                userName, 
                password, 
                remember, 
                lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public long CurrentUserId => _signInManager.Context?.User.Claims.ByType(ClaimTypes.Sid).ToLong() ?? 0;
        
        public string Display => _signInManager.Context?.User.Identity?.Name;

        public bool IsLogged => _signInManager.Context?.User.Identity?.IsAuthenticated ?? false;

        public bool IsAnonymous => IsLogged == false;
    }
}