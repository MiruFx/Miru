using Miru.Userfy;

namespace Miru.Testing
{
    public class TestingCurrentUser : ICurrentUser
    {
        public static UserfyUser User;
        
        public long Id => User?.Id ?? 0;
        
        public string Display => User?.Display ?? string.Empty;
        
        public bool IsAuthenticated => User != null;
        
        public bool IsAnonymous => !IsAuthenticated;
    }
}
