using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy;

public interface IUserRegister<TUser> where TUser : UserfyUser
{
    Task<IdentityResult> RegisterAsync(
        TUser user, 
        string login, 
        string password, 
        CancellationToken ct);
}