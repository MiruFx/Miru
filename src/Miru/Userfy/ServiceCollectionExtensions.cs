using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Miru.Mvc;
using Miru.Security;

namespace Miru.Userfy
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthorizationRules<TAuthorizationConfig>(this IServiceCollection services)
            where TAuthorizationConfig : class, IAuthorizationRules
        {
            return services.AddScoped<IAuthorizationRules, TAuthorizationConfig>();
        }

        public static IServiceCollection AddUserfy<TUser, TDbContext>(
            this IServiceCollection services,
            Action<IdentityOptions> options = null,
            Action<CookieAuthenticationOptions> cookie = null) 
                where TUser : UserfyUser
                where TDbContext : UserfyDbContext<TUser>
        {
            return services.AddUserfy<TUser, IdentityRole<long>, TDbContext>(options, cookie);
        }
        
        public static IServiceCollection AddUserfy<TUser, TRole, TDbContext>(
            this IServiceCollection services,
            Action<IdentityOptions> options = null,
            Action<CookieAuthenticationOptions> cookie = null) 
                where TUser : UserfyUser
                where TRole : IdentityRole<long>
                where TDbContext : UserfyDbContext<TUser, TRole>
        {
            // miru services setup
            services.AddTransient<IUserSession, UserfyUserSession<TUser>>();
            services.AddTransient<IUserSession<TUser>, UserfyUserSession<TUser>>();
            services.AddTransient<ISessionStore, HttpSessionStore>();
            services.AddTransient<Authorizer>();
            services.AddTransient<ICurrentUser, UserfyCurrentUser>();

            // asp.net identity
            services.AddIdentity<TUser, IdentityRole<long>>(options)
                .AddEntityFrameworkStores<TDbContext>();

            services.ConfigureApplicationCookie(cookie);

            services.AddAuthorization();
            
            // in case of asp.net identity UI
            services.AddRazorPages();
            
            return services;
        }
    }
}