using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Behaviors.BelongsToUser;
using Miru.Behaviors.TimeStamp;
using Miru.Hosting;
using Miru.Queuing;
using Miru.Sqlite;

namespace Corpo.Skeleton;

public class Startup : MiruStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMiru<Startup>()

            .AddMiruHtml<HtmlConfig>()
            
            .AddDefaultPipeline<Startup>()
            
            .AddCurrentAttributes<Current, CurrentAttributes>()
                
            .AddEfCoreSqlite<AppDbContext>()

            // miru extensions
            .AddUserfy<User, AppDbContext>(
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
            
            // miru behaviors
            .AddBelongsToUser()
            .AddTimeStamp()

            .AddMailing(_ =>
            {
                _.EmailDefaults(email => email.From("noreply@company.com", "Skeleton"));
            })
            
            // SenderFileStorage saves emails at /storage/temp/emails
            // for sending emails throught smtp, use SmtpSender instead of FileStorageSender:
            // .AddSmtpSender()
            // .AddFileStorageSender()

            .AddQueuing(_ =>
            {
                _.UseLiteDb();
            })
            .AddHangfireServer();
            
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
            e.MapHangfireDashboard("/Hangfire", new DashboardOptions
            {
                AsyncAuthorization = new[] { new HangfireAuthorizationFilter() }
            });
        });
    }

    public Startup(IHostEnvironment env, IConfiguration configuration) : base(env, configuration)
    {
    }
}