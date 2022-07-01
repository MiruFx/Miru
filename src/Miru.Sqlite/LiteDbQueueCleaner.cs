using System.Threading.Tasks;
using Baseline;
using Hangfire;
using Hangfire.LiteDB;
using Miru.Queuing;

namespace Miru.Sqlite;

public class LiteDbQueueCleaner : IQueueCleaner
{
    private readonly JobStorage _jobStorage;

    public LiteDbQueueCleaner(JobStorage jobStorage)
    {
        _jobStorage = jobStorage;
    }

    public async Task ClearAsync()
    {
        var connection = _jobStorage.As<LiteDbStorage>().Connection;

        connection.StateDataSet.DeleteAll();
        connection.StateDataHash.DeleteAll();
        connection.Job.DeleteAll();
        connection.JobQueue.DeleteAll();
            
        await Task.CompletedTask;
    }
}