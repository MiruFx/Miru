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

namespace Miru.Postgres;

public static class EfCorePostgresRegistry
{
    public static IServiceCollection AddEfCorePostgres<TDbContext>(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = null) 
        where TDbContext : DbContext
    {
        services.AddEfCoreServices<TDbContext>();
            
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            options.UseNpgsql(dbOptions.ConnectionString);
                
            // TODO: investigate more
            // TODO: https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                
            var hostEnvironment = sp.GetService<IHostEnvironment>();
                
            if (hostEnvironment != null && hostEnvironment.IsDevelopmentOrTest())
            {
                options.EnableSensitiveDataLogging();
            }
                
            if (optionsAction != null)
                optionsAction.Invoke(sp, options);
        });

        services.AddMigrator<TDbContext>(mb => mb.AddPostgres92());
            
        return services;
    }
}