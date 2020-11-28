using System;
using Baseline;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Miru.Domain;
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

        public static IServiceCollection AddUserfy<TUser>(this IServiceCollection services, Action<UserfyOptions> setupAction)
            where TUser : class, IUser, IEntity
        {
            var userfyOptions = new UserfyOptions();

            setupAction(userfyOptions);
            
            if (userfyOptions.CookieName.IsEmpty())
                userfyOptions.CookieName = App.Name;
            
            // .net core services setup
            // services.AddSession(opt =>
            // {
            //     opt.Cookie.SameSite = SameSiteMode.Strict;
            //     opt.Cookie.HttpOnly = true;
            //     opt.Cookie.Name = userfyOptions.CookieName;
            // });
            
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Cookie.SameSite = SameSiteMode.Strict;
                        options.Cookie.HttpOnly = true;
                        options.Cookie.Name = userfyOptions.CookieName;
                        options.LoginPath = new PathString(userfyOptions.LoginPath);
                    });
            
            // miru services setup
            services.AddTransient<IUserSession, WebUserSession>();
            services.AddTransient<ISessionStore, HttpSessionStore>();
            services.AddTransient<Authorizer>();
            services.AddTransient<IUserSession<TUser>, UserSession<TUser>>();

            services.AddSingleton(userfyOptions);
            
            return services;
        }
    }
}