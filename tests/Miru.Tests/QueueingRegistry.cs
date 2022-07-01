using Hangfire.LiteDB;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using Miru.Security;
using Miru.Sqlite;
using Miru.Storages;

namespace Miru.Tests;

public static class QueueingRegistry
{
    public static IServiceCollection AddInMemoryQueueing(this IServiceCollection services)
    {
        services.AddQueuing(x =>
        {
            x.Configuration.UseMemoryStorage();
        });
        
        services.AddScoped<IQueueAuthorizer, DefaultQueueAuthorizer>();
        
        return services;
    }
}