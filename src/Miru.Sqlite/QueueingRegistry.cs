using Hangfire.LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.Queuing;
using Miru.Security;
using Miru.Storages;

namespace Miru.Sqlite;

public static class QueueingRegistry
{
    public static IServiceCollection AddLiteDbQueueing(this IServiceCollection services)
    {
        services.AddQueuing(x =>
        {
            x.UseLiteDb();
        });
        
        services.AddScoped<IQueueAuthorizer, DefaultQueueAuthorizer>();

        services.AddStorage<DbStorage>();

        services.AddQueueCleaner<LiteDbQueueCleaner>();
        
        return services;
    }
    
    public static void UseLiteDb(this QueuingBuilder builder)
    {
        var storage = builder.ServiceProvider.GetRequiredService<DbStorage>();
        var env = builder.ServiceProvider.GetService<IHostEnvironment>();
        var queueOptions = builder.ServiceProvider.GetRequiredService<QueueingOptions>();

        var dbPath = storage.Path / $"Queue_{env?.EnvironmentName ?? "All"}.db";

        dbPath.Dir().EnsureDirExist();
            
        queueOptions.ConnectionString = dbPath;

        builder.Configuration.UseLiteDbStorage(queueOptions.ConnectionString);
    }
}