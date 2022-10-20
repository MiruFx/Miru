using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Miru.Domain;

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

    public async Task<SignInResult> LoginAsync(string userName, string password, bool remember = false)
    {
        var user = await _userManager.FindByEmailAsync(userName);

        if (user != null)
        {
            var result = await _userSession.LoginAsync(
                userName, 
                password, 
                remember);
                
            return result;
        }

        return SignInResult.Failed;
    }

    public async Task LogoutAsync()
    {
        await _userSession.LogoutAsync();
    }
}