using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Miru.Mvc;
using Miru.Security;

namespace Miru.Userfy
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthorization<TAuthorizationConfig>(this IServiceCollection services)
            where TAuthorizationConfig : class, IAuthorizationRules
        {
            return services.AddScoped<IAuthorizationRules, TAuthorizationConfig>();
        }

        public static IServiceCollection AddUserfy<TUser, TDbContext>(
            this IServiceCollection services) 
                where TUser : UserfyUser
                where TDbContext : UserfyDbContext<TUser>
        {
            // miru services setup
            services.AddTransient<IUserSession, IdentityUserSession<TUser>>();
            services.AddTransient<ISessionStore, HttpSessionStore>();
            services.AddTransient<Authorizer>();
            services.AddTransient<IUserSession<TUser>, UserSession<TUser>>();

            // asp.net identity
            services.AddIdentity<TUser, IdentityRole<long>>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<TDbContext>();

            services.AddAuthorization();
            
            // in case of asp.net identity UI
            services.AddRazorPages();
            
            return services;
        }
        
        public static IServiceCollection AddUserfy<TUser, TRole, TDbContext>(
            this IServiceCollection services) 
                where TUser : UserfyUser
                where TRole : IdentityRole<long>
                where TDbContext : UserfyDbContext<TUser, TRole>
        {
            // miru services setup
            services.AddTransient<IUserSession, IdentityUserSession<TUser>>();
            services.AddTransient<ISessionStore, HttpSessionStore>();
            services.AddTransient<Authorizer>();
            services.AddTransient<IUserSession<TUser>, UserSession<TUser>>();

            // asp.net identity
            services.AddIdentity<TUser, TRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<TDbContext>();

            services.AddAuthorization();
            
            services.AddRazorPages();
            
            return services;
        }
    }
}