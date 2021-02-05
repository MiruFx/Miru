using Miru.Userfy;

namespace Miru.Testing
{
    public class TestingUserSession : IUserSession
    {
        public static IUser CurrentUser;

        public void Login(IUser user, bool remember)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        // FIXME
        // public long CurrentUserId => CurrentUser?.Id ?? 0;
        public long CurrentUserId => 0;
        
        public string Display => CurrentUser?.Display ?? string.Empty;
        
        public bool IsLogged => CurrentUser != null;
        
        public bool IsAnonymous => !IsLogged;
    }
}
