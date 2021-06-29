using System;
using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miru;
using Miru.Behaviors.BelongsToUser;
using Miru.Fabrication;
using Miru.Foundation.Hosting;
using Miru.Globalization;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Pipeline;
using Miru.Queuing;
using Miru.Sqlite;
using Miru.Userfy;
using Playground.Config;
using Playground.Database;
using Playground.Domain;

namespace Playground
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMiru<Startup>()

                .AddDefaultPipeline<Startup>()
                
                .AddGlobalization("de-DE", "en-GB", "en-US", "pt-BR", "pt-PT")

                .AddEfCoreSqlite<PlaygroundDbContext>()

                // user register, login, logout
                .AddUserfy<User, PlaygroundDbContext>(
                    cookie: cfg =>
                    {
                        cfg.Cookie.Name = App.Name;
                        cfg.Cookie.HttpOnly = true;
                        cfg.ExpireTimeSpan = TimeSpan.FromHours(2);
                        cfg.LoginPath = "/Accounts/Login";
                    },
                    identity: cfg =>
                    {
                        cfg.SignIn.RequireConfirmedAccount = false;
                        cfg.SignIn.RequireConfirmedEmail = false;
                        cfg.SignIn.RequireConfirmedPhoneNumber = false;

                        cfg.Password.RequiredLength = 3;
                        cfg.Password.RequireUppercase = false;
                        cfg.Password.RequireNonAlphanumeric = false;
                        cfg.Password.RequireLowercase = false;

                        cfg.User.RequireUniqueEmail = true;
                    })
                .AddAuthorizationRules<AuthorizationRulesConfig>()
                .AddBelongsToUser<User>()

                .AddMailing(_ =>
                {
                    _.EmailDefaults(email => email.From("noreply@skeleton.com", "Skeleton"));
                })
                .AddSenderStorage()

                .AddQueuing(_ =>
                {
                    _.UseLiteDb();
                })
                .AddHangfireServer();

            services
                .AddFabrication<PlaygroundFabricator>();
            
            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();

            // your app services
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // The Middlewares here are configured in order of executation
            // Here, they are organized for Miru defaults. Changing the order might break some functionality 

            if (env.IsDevelopmentOrTest())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            
            app.UseStaticFiles();

            app.UseRequestLocalization();
            
            app.UseRequestLogging();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseExceptionLogging();

            app.UseHangfireDashboard();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            
            app.UseEndpoints(e =>
            {
                e.MapDefaultControllerRoute();
                e.MapRazorPages();
                
                if (env.IsDevelopmentOrTest())
                    e.MapEmailsStorage();
            });
        }
    }
}
