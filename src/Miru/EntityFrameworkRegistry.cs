using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Databases;
using Miru.Databases.EntityFramework;
using Miru.Settings;

namespace Miru;

public static class EntityFrameworkRegistry
{
    public static IServiceCollection AddEfCoreServices<TDbContext>(this IServiceCollection services) 
        where TDbContext : DbContext
    {
        services.AddScoped<IDataAccess, EntityFrameworkDataAccess>();
            
        services.AddTransient<IDatabaseCreator, EntityFrameworkDatabaseCreator>();
            
        // Forward DbContext to TDbContext
        services.ForwardScoped<DbContext, TDbContext>();

        // ConnectionString transformation
        services.PostConfigureAll<DatabaseOptions>(settings =>
        {
            if (App.Solution != null)
            {
                var dbDir = App.Solution.StorageDir / "db" / ".";
                
                settings.ConnectionString = 
                    settings.ConnectionString?.Replace("{{ db_dir }}", dbDir);
                
                Directories.CreateIfNotExists(dbDir);   
            }
        });

        services.AddTransient<IInterceptor, QueryFiltersInterceptor>();
            
        return services;
    }
}