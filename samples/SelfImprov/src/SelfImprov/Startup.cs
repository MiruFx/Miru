using AutoMapper;
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
using Miru.SqlServer;
using Miru.Turbolinks;
using Miru.Userfy;
using SelfImprov.Config;
using SelfImprov.Database;
using SelfImprov.Domain;
using Serilog.Events;

namespace SelfImprov
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMiru<Startup>()
                
                // pipeline
                .AddDefaultPipeline<Startup>()

                // database
                .AddEfCoreSqlServer<SelfImprovDbContext>()

                // user security
                .AddUserfy<User>(options =>
                {
                    options.LoginPath = "/Accounts/Login";
                })
                .AddAuthorization<AuthorizationConfig>()

                // mailing
                .AddMailing(_ =>
                {
                    _.EmailDefaults(email => email.From("noreply@selfImprov.com", "SelfImprov"));
                })
                .AddSenderSmtp()

                // queuing
                .AddQueuing(_ =>
                {
                    _.UseSqlServer();
                })
                .AddHangfireServer()

                // ui
                .AddTurbolinks()
                
                .AddAutoMapper(typeof(Startup));

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
