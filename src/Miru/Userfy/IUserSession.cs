using System.Threading.Tasks;

namespace Miru.Userfy
{
    public interface IUserSession<TUser> : IUserSession where TUser : IUser
    {
        Task<TUser> User();
    }

    public interface IUserSession
    {
        void Login(IUser user, bool remember = false);
        
        void Logout();
        
        long CurrentUserId { get; }
        
        string Display { get; }
        
        bool IsLogged { get; }
        
        bool IsAnonymous { get; }
    }
}