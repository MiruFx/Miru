using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy;

public interface IUserLogin<TUser> where TUser : UserfyUser
{
    Task<SignInResult> LoginAsync(string userName, string password, bool remember = false);

    Task LogoutAsync();
}