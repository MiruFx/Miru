using System;
using Hangfire;
using Hangfire.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using Miru.Security;
using StackExchange.Redis;

namespace Miru.Redis;

public static class RedisRegistry
{
    public static IServiceCollection AddRedisQueueing<TAuthorizer>(
        this IServiceCollection services,
        Action<QueuingBuilder> queuingBuilder = null)
        where TAuthorizer : class, IQueueAuthorizer
    {
        services.AddQueuing(x =>
        {
            x.UseRedis();
            
            queuingBuilder?.Invoke(x);
        });
        
        services.AddScoped<IQueueAuthorizer, TAuthorizer>();

        services.AddQueueCleaner<RedisQueueCleaner>();
        
        services.AddSingleton(sp =>
        {
            var configuration = sp.GetService<IConfiguration>();

            var connectionString = configuration.GetValue<string>("Queueing:ConnectionString");
            
            return ConnectionMultiplexer.Connect(connectionString);
        });

        return services;
    }
    
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
        var queueingOptions = sp.GetRequiredService<QueueingOptions>();
        
        builder.Configuration.UseRedisStorage(redisConnection, new RedisStorageOptions
        {
            Prefix = queueingOptions.Prefix
        });
    }
}