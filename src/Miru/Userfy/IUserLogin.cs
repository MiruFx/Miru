using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy;

public interface IUserLogin<TUser> where TUser : UserfyUser
{
    Task<LoginResult<TUser>> LoginAsync(string userName, string password, bool remember = false);

    Task LogoutAsync();
}

public class LoginResult<TUser> where TUser : UserfyUser
{
    public SignInResult Result { get; set; }
    
    public TUser User { get; set; } 
}