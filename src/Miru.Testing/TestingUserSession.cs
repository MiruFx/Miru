using Miru.Userfy;

namespace Miru.Testing
{
    public class TestingCurrentUser : ICurrentUser
    {
        public static UserfyUser User;
        
        public long Id => User.Id;
        
        public string Display => User?.Display ?? string.Empty;
        
        public bool IsLogged => User != null;
        
        public bool IsAnonymous => !IsLogged;
    }
}
