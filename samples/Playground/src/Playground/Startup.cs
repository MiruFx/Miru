using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru;
using Miru.Fabrication;
using Miru.Hosting;
using Miru.Html.HtmlConfigs;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Queuing;
using Miru.Security;
using Miru.Sqlite;
using Playground.Config;
using Playground.Database;
using Playground.Domain;

namespace Playground;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMiru<Startup>()

            .AddHtmlConventions<HtmlConfig>()
            
            .AddDefaultPipeline<Startup>()

            .AddGlobalization(defaultCulture: "en-GB", "de-DE", "en-US", "pt-BR", "pt-PT")

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
            .AddBelongsToUser()

            .AddMailing(_ =>
            {
                _.EmailDefaults(email => email.From("noreply@skeleton.com", "Skeleton"));
            })
            .AddSmtpSender()

            // TODO: one line call .AddLiteDbQueueing() with DefaultQueueAuthorizer set by default
            .AddScoped<IQueueAuthorizer, DefaultQueueAuthorizer>()
            .AddLiteDbQueueing()
            .AddHangfireServer()

            .AddScheduledJob<ScheduledJobConfig>();

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
            // app.UseDeveloperExceptionPage();
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

        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
            
        app.UseQueueDashboard();
        
        app.UseEndpoints(e =>
        {
            e.MapDefaultControllerRoute();
            e.MapRazorPages();
        });
    }
}