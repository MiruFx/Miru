using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy;

public class UserfyUserLogin<TUser> : IUserLogin<TUser> where TUser : UserfyUser
{
    private readonly UserManager<TUser> _userManager;
    private readonly IUserSession<TUser> _userSession;

    public UserfyUserLogin(UserManager<TUser> userManager, IUserSession<TUser> userSession)
    {
        _userManager = userManager;
        _userSession = userSession;
    }

    public async Task<LoginResult<TUser>> LoginAsync(string userName, string password, bool remember = false)
    {
        var user = await _userManager.FindByEmailAsync(userName);

        if (user != null)
        {
            var result = await _userSession.LoginAsync(
                userName, 
                password, 
                remember);
                
            return new LoginResult<TUser>
            {
                User = user,
                Result = result
            };
        }

        return new LoginResult<TUser>
        {
            Result = SignInResult.Failed
        };
    }

    public async Task LogoutAsync()
    {
        await _userSession.LogoutAsync();
    }
}