using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy
{
    public class UserfyUserSession<TUser> : IUserSession where TUser : UserfyUser
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly ICurrentUser _currentUser;

        public UserfyUserSession(SignInManager<TUser> signInManager, ICurrentUser currentUser)
        {
            _signInManager = signInManager;
            _currentUser = currentUser;
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

        public long CurrentUserId => _currentUser.Id;
        
        public string Display => _currentUser.Display;

        public bool IsLogged => _currentUser.IsLogged;

        public bool IsAnonymous => IsLogged == false;
    }
}