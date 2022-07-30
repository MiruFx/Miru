using System;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Databases.EntityFramework;
using Miru.Databases.Migrations;
using Miru.Hosting;
using Miru.Settings;

namespace Miru.SqlServer
{
    public static class EfCoreSqlServerRegistry
    {
        public static IServiceCollection AddEfCoreSqlServer<TDbContext>(
            this IServiceCollection services,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = null) 
                where TDbContext : DbContext
        {
            services.AddEfCoreServices<TDbContext>();

            services.AddDbContext<TDbContext>((sp, options) =>
            {
                var dbConfig = sp.GetService<DatabaseOptions>();
                
                options.UseSqlServer(dbConfig.ConnectionString);

                // TODO: add action to configure options from outside this method
                if (sp.GetService<IHostEnvironment>().IsDevelopmentOrTest())
                {
                    options.EnableSensitiveDataLogging();
                }
                
                if (optionsAction != null)
                    optionsAction.Invoke(sp, options);
            });

            services.AddMigrator<TDbContext>(mb => mb.AddSqlServer2016());

            return services;
        }
    }
}