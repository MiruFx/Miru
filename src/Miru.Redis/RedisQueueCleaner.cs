using System.Threading.Tasks;
using Miru.Queuing;
using StackExchange.Redis;

namespace Miru.Redis;

public class RedisQueueCleaner : IQueueCleaner
{
    private readonly ConnectionMultiplexer _connection;

    public RedisQueueCleaner(ConnectionMultiplexer connection)
    {
        _connection = connection;
    }

    public async Task ClearAsync()
    {
        await _connection.GetDatabase().ExecuteAsync("FLUSHDB");
    }
}