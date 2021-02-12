using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Miru.Userfy;

namespace Miru.Testing
{
    public class TestingUserSession<TUser> : IUserSession where TUser : UserfyUser
    {
        private readonly IMiruApp _app;
        private static UserfyUser _currentUser;

        public TestingUserSession(IMiruApp app)
        {
            _app = app;
        }

        public void Login(UserfyUser user)
        {
            _currentUser = user;
        }

        public async Task<SignInResult> LoginAsync(string userName, string password, bool remember = false)
        {
            _app.WithScope(scope =>
            {
                var user = scope.Get<DbContext>().Set<TUser>().SingleOrDefault(x => x.UserName == userName);
                Login(user);
            });
            
            return await Task.FromResult(SignInResult.Success);
        }

        public Task LogoutAsync()
        {
            _currentUser = null;
            return Task.CompletedTask;
        }

        public long CurrentUserId => _currentUser.Id;
        
        public string Display => _currentUser?.Display ?? string.Empty;
        
        public bool IsLogged => _currentUser != null;
        
        public bool IsAnonymous => !IsLogged;
    }
}
