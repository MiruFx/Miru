using System.Threading.Tasks;
using Baseline;
using Hangfire;
using Hangfire.LiteDB;
using Miru.Queuing;

namespace Miru.Sqlite
{
    public class LiteDbQueueCleaner : IQueueCleaner
    {
        private readonly JobStorage _jobStorage;

        public LiteDbQueueCleaner(JobStorage jobStorage)
        {
            _jobStorage = jobStorage;
        }

        public Task Clear()
        {
            var connection = _jobStorage.As<LiteDbStorage>().Connection;
            
            connection.Job.DeleteAll();
            connection.JobQueue.DeleteAll();
            
            return Task.CompletedTask;
        }
    }
}