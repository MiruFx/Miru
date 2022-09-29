using System.Threading.Tasks;
using Miru.Queuing;
using StackExchange.Redis;

namespace Miru.Redis;

public class RedisQueueCleaner : IQueueCleaner
{
    private readonly ConnectionMultiplexer _connection;
    private readonly QueueingOptions _queueingOptions;

    public RedisQueueCleaner(ConnectionMultiplexer connection, QueueingOptions queueingOptions)
    {
        _connection = connection;
        _queueingOptions = queueingOptions;
    }

    public async Task ClearAsync()
    {
        var command = @$"eval ""for _,k in ipairs(redis.call('keys','{_queueingOptions.Prefix}:*')) do redis.call('del',k) end"" 0";
        
        await _connection.GetDatabase().ExecuteAsync(command);
    }
}