using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Miru.Userfy;

namespace Miru.Testing
{
    public class TestingUserSession<TUser> : IUserSession where TUser : UserfyUser
    {
        public static UserfyUser CurrentUser;
        private DbContext _db;

        public TestingUserSession(DbContext db)
        {
            _db = db;
        }

        // private readonly UserManager<TUser> _userManager;
        //
        // public TestingUserSession(UserManager<TUser> userManager)
        // {
        //     _userManager = userManager;
        // }

        public void Login(UserfyUser user)
        {
            CurrentUser = user;
        }

        public async Task<SignInResult> LoginAsync(string userName, string password, bool remember = false)
        {
            var user = await _db.Set<TUser>().SingleOrDefaultAsync(x => x.UserName == userName);
            Login(user);
            return SignInResult.Success;
        }

        public Task LogoutAsync()
        {
            CurrentUser = null;
            return Task.CompletedTask;
        }

        public long CurrentUserId => CurrentUser.Id;
        
        public string Display => CurrentUser?.Display ?? string.Empty;
        
        public bool IsLogged => CurrentUser != null;
        
        public bool IsAnonymous => !IsLogged;
    }
}
