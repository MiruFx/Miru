using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Miru.Userfy;

namespace Miru.Testing
{
    public class TestingUserSession<TUser> : IUserSession<TUser> where TUser : UserfyUser
    {
        private readonly IMiruApp _app;
        private readonly ICurrentUser _currentUser;

        public TestingUserSession(IMiruApp app, ICurrentUser currentUser)
        {
            _app = app;
            _currentUser = currentUser;
        }

        public async Task<TUser> GetUserAsync() => await Task.FromResult(TestingCurrentUser.User as TUser);

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

        public long CurrentUserId => _currentUser?.Id ?? 0;
        
        public string Display => _currentUser.Display;
        
        public bool IsAuthenticated => _currentUser.IsAuthenticated;
        
        public bool IsAnonymous => !_currentUser.IsAuthenticated;
    }
}
