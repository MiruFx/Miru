using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using StackExchange.Redis;

namespace Miru.Redis;

public static class RedisRegistry
{
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var configuration = sp.GetService<IConfiguration>();

            return ConnectionMultiplexer.Connect(configuration.GetValue<string>("Queueing:ConnectionString"));
        });
        
        return services;
    }
    
    public static void UseRedis(this QueuingBuilder builder)
    {
        var sp = builder.ServiceProvider;
            
        var redisConnection = sp.GetRequiredService<ConnectionMultiplexer>();
        
        builder.Configuration.UseRedisStorage(redisConnection);
    }
}