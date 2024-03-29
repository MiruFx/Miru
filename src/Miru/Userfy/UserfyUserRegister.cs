using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Miru.Userfy;

public class UserfyUserRegister<TUser> : IUserRegister<TUser> where TUser : UserfyUser
{
    private readonly IUserStore<TUser> _userStore;
    private readonly IUserEmailStore<TUser> _emailStore;
    private readonly UserManager<TUser> _userManager;

    public UserfyUserRegister(
        IUserStore<TUser> userStore, 
        UserManager<TUser> userManager)
    {
        _userStore = userStore;
        _emailStore = (IUserEmailStore<TUser>)_userStore;
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterAsync(
        TUser user, 
        string login,
        string password = "",
        CancellationToken ct = default)
    {
        await _userStore.SetUserNameAsync(user, login, ct);
        await _emailStore.SetEmailAsync(user, login, ct);

        user.NormalizedEmail = login.Normalize().ToUpperInvariant();
        user.NormalizedUserName = login.Normalize().ToUpperInvariant();
                
        if (string.IsNullOrEmpty(password))
            return await _userManager.CreateAsync(user);
            
        return await _userManager.CreateAsync(user, password);
    }
}