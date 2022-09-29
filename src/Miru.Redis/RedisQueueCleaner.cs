using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Miru.Queuing;
using StackExchange.Redis;

namespace Miru.Redis;

public class RedisQueueCleaner : IQueueCleaner
{
    private readonly ConnectionMultiplexer _connection;
    private readonly QueueingOptions _queueingOptions;

    public RedisQueueCleaner(ConnectionMultiplexer connection, IOptions<QueueingOptions> queueingOptions)
    {
        _connection = connection;
        _queueingOptions = queueingOptions.Value;
    }

    public async Task ClearAsync()
    {
        var server = _connection.GetServer(_connection.GetEndPoints().First());
        var db = _connection.GetDatabase();
        var keys = server.Keys(pattern: $"{_queueingOptions.Prefix}:*");
        
        foreach (var key in keys)
        {
            db.KeyDelete(key);
        }

        await Task.CompletedTask;
    }
}