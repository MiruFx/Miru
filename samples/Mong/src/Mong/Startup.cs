using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru;
using Miru.Databases.EntityFramework;
using Miru.Foundation.Hosting;
using Miru.Foundation.Logging;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Pipeline;
using Miru.Queuing;
using Miru.Turbolinks;
using Miru.Userfy;
using Mong.Config;
using Mong.Database;
using Mong.Domain;
using Mong.Payments;
using Serilog.Events;

// #startup
namespace Mong
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMiru<Startup>()
            
                // .AddSerilogConfig(config =>
                // {
                //     config.MinimumLevel.Override("SolutionName", LogEventLevel.Debug);
                // })
                
                // pipeline
                .AddDefaultPipeline<Startup>()

                // database
                .AddEfCoreSqlite<MongDbContext>()
                
                // user security
                .AddUserfy<User>(options =>
                {
                    options.LoginPath = "/Accounts/Login";
                })
                .AddAuthorization<AuthorizationConfig>()
                
                // mailing
                .AddMailing(_ =>
                {
                    _.EmailDefaults(email => email.From("noreply@mong.com", "Mong"));
                })
                .AddSenderStorage()
                // .AddSenderSmtp()
                
                // #addserver
                // queuing
                .AddQueuing(_ => 
                {
                    // _.UseSqlite();
                    _.UseLiteDb();
                })
                .AddHangfireServer()
                // #addserver
                
                // ui
                .AddTurbolinks()
                
                // app services
                .AddSingleton<IPayPau, AlmostRealPayPau>()
                .AddSingleton<IMobileProvider, MobileProvider>();
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
            
            // #jobdashboard
            app.UseRouting();
            app.UseAuthentication();
            app.UseQueueAdminDashboard<User>();
            
            app.UseEndpoints(e =>
            {
                e.MapDefaultControllerRoute();
            });
            // #jobdashboard
        }
    }
}
// #startup