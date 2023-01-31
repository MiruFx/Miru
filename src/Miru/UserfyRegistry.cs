using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Miru.Mvc;
using Miru.Security;
using Miru.Userfy;

namespace Miru;

public static class UserfyRegistry
{
    public static IServiceCollection AddAuthorizationRules<TAuthorizationConfig>(this IServiceCollection services)
        where TAuthorizationConfig : class, IAuthorizationRules
    {
        return services.AddScoped<IAuthorizationRules, TAuthorizationConfig>();
    }

    public static IServiceCollection AddUserfy<TUser, TDbContext>(
        this IServiceCollection services,
        Action<IdentityOptions> identity = null,
        Action<CookieAuthenticationOptions> cookie = null,
        Action<UserfyOptions> userfy = null) 
        where TUser : UserfyUser
        where TDbContext : UserfyDbContext<TUser>
    {
        return services.AddUserfy<TUser, IdentityRole<long>, TDbContext>(identity, cookie, userfy);
    }
        
    public static IServiceCollection AddUserfy<TUser, TRole, TDbContext>(
        this IServiceCollection services,
        Action<IdentityOptions> identity = null,
        Action<CookieAuthenticationOptions> cookie = null,
        Action<UserfyOptions> userfy = null) 
        where TUser : UserfyUser
        where TRole : IdentityRole<long>
        where TDbContext : UserfyDbContext<TUser, TRole>
    {
        // config
        if (userfy != null) 
            services.Configure(userfy);
            
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<UserfyOptions>>().Value);
            
        // miru services setup
        services.AddTransient<IUserSession, UserfyUserSession<TUser>>();
        services.AddTransient<IUserSession<TUser>, UserfyUserSession<TUser>>();
        services.AddTransient<IUserLogin<TUser>, UserfyUserLogin<TUser>>();
        services.AddTransient<IUserRegister<TUser>, UserfyUserRegister<TUser>>();
        services.AddTransient<ICurrentUser, UserfyCurrentUser>();
            
        services.AddTransient<ISessionStore, HttpSessionStore>();

        // asp.net identity
        services.AddIdentity<TUser, IdentityRole<long>>(identity)
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders();

        if (cookie != null)
            services.ConfigureApplicationCookie(cookie);

        services.AddAuthorization();
        services.AddLogging();
            
        // in case of asp.net identity UI
        services.AddRazorPages();
            
        return services;
    }
}