using System;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Databases.EntityFramework;
using Miru.Databases.Migrations;
using Miru.Hosting;

namespace Miru.Sqlite;

public static class EfCoreSqliteRegistry
{
    public static IServiceCollection AddEfCoreSqlite<TDbContext>(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = null,
        string connectionString = null) 
        where TDbContext : DbContext
    {
        services.AddEfCoreServices<TDbContext>();
            
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            if (connectionString == null)
            {
                var dbConfig = sp.GetService<IOptions<DatabaseOptions>>().Value;

                options.UseSqlite(dbConfig.ConnectionString);
            }
            else
            {
                options.UseSqlite(connectionString);
            }

            var environment = sp.GetService<IHostEnvironment>();
            
            if (environment != null && sp.GetService<IHostEnvironment>().IsDevelopmentOrTest())
            {
                options.EnableSensitiveDataLogging();
            }
            
            if (optionsAction != null)
                optionsAction.Invoke(sp, options);
        });

        services.AddMigrator<TDbContext>(mb => mb.AddSQLite());
            
        return services;
    }
}