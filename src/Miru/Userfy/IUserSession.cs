using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy
{
    public interface IUserSession<TUser> : IUserSession where TUser : UserfyUser
    {
        Task<TUser> GetUser();
    }

    public interface IUserSession
    {
        Task<SignInResult> LoginAsync(string userName, string password, bool remember = false);
        
        Task LogoutAsync();
        
        long CurrentUserId { get; }
        
        string Display { get; }
        
        bool IsLogged { get; }
        
        bool IsAnonymous { get; }
    }
}