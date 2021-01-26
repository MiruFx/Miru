using Corpo.Skeleton.Config;
using Corpo.Skeleton.Database;
using Corpo.Skeleton.Domain;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru;
using Miru.Foundation.Hosting;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Pipeline;
using Miru.Queuing;
using Miru.Sqlite;
using Miru.Turbolinks;
using Miru.Userfy;

namespace Corpo.Skeleton
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMiru<Startup>()
            
                // pipeline
                .AddDefaultPipeline<Startup>()
                
                // database
                .AddEfCoreSqlite<SkeletonDbContext>()
                
                // user security
                .AddUserfy<User>(options =>
                {
                    options.LoginPath = "/Accounts/Login";
                })
                .AddAuthorization<AuthorizationConfig>()
                
                // mailing
                .AddMailing(_ =>
                {
                    _.EmailDefaults(email => email.From("noreply@Skeleton.com", "Skeleton"));
                })
                .AddSenderStorage()
                
                // queuing
                .AddQueuing(_ => 
                {
                    _.UseLiteDb();
                })
                .AddHangfireServer()
                
                // ui
                .AddTurbolinks();
            
                // your app services
                // services.AddSingleton<IService, Service>();
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
            app.UseAuthentication();
            
            app.UseEndpoints(e =>
            {
                e.MapDefaultControllerRoute();
            });
        }
    }
}
