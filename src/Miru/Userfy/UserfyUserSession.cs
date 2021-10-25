using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy
{
    public class UserfyUserSession<TUser> : IUserSession<TUser> where TUser : UserfyUser
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly ICurrentUser _currentUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserfyUserSession(
            SignInManager<TUser> signInManager, 
            ICurrentUser currentUser, 
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _currentUser = currentUser;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<TUser> GetUserAsync() => 
            _signInManager.UserManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        public async Task<SignInResult> LoginAsync(string userName, string password, bool remember = false)
        {
            return await _signInManager.PasswordSignInAsync(
                userName, 
                password, 
                remember, 
                lockoutOnFailure: false);
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();

        public long CurrentUserId => _currentUser.Id;
        
        public string Display => _currentUser.Display;

        public bool IsLogged => _currentUser.IsLogged;

        public bool IsAnonymous => IsLogged == false;
    }
}