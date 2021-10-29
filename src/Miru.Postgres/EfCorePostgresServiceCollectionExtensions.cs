using System;
using System.Text;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Miru.Databases.EntityFramework;
using Miru.Databases.Migrations;
using Miru.Foundation.Hosting;
using Miru.Settings;

namespace Miru.Postgres
{
    public static class EfCorePostgresServiceCollectionExtensions
    {
        public static IServiceCollection AddEfCorePostgres<TDbContext>(
            this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddEfCoreServices<TDbContext>();
            
            services.AddDbContext<TDbContext>((sp, options) =>
            {
                var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

                options.UseNpgsql(dbOptions.ConnectionString);
                
                // TODO: investigate more
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                
                var hostEnvironment = sp.GetService<IHostEnvironment>();
                
                if (hostEnvironment != null && hostEnvironment.IsDevelopmentOrTest())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            services.AddMigrator<TDbContext>(mb => mb.AddPostgres92());
            
            return services;
        }
    }
}