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
        private readonly ICurrentUser _currentUser;

        public TestingUserSession(IMiruApp app, ICurrentUser currentUser)
        {
            _app = app;
            _currentUser = currentUser;
        }

        public async Task<SignInResult> LoginAsync(string userName, string password, bool remember = false)
        {
            _app.WithScope(scope =>
            {
                var user = scope.Get<DbContext>().Set<TUser>().SingleOrDefault(x => x.UserName == userName);
                
                TestingCurrentUser.User = user;
            });
            
            return await Task.FromResult(SignInResult.Success);
        }

        public Task LogoutAsync()
        {
            TestingCurrentUser.User = null;
            return Task.CompletedTask;
        }

        public long CurrentUserId => _currentUser.Id;
        
        public string Display => _currentUser.Display;
        
        public bool IsLogged => _currentUser.IsLogged;
        
        public bool IsAnonymous => !_currentUser.IsLogged;
    }
}
