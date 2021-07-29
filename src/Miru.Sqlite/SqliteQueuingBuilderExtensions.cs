using Hangfire.LiteDB;
using Hangfire.Storage.SQLite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.Queuing;
using Miru.Settings;
using Miru.Storages;

namespace Miru.Sqlite
{
    public static class SqliteQueuingBuilderExtensions
    {
        public static void UseLiteDb(this QueuingBuilder builder)
        {
            var storage = builder.ServiceProvider.GetRequiredService<IStorage>();
            var env = builder.ServiceProvider.GetRequiredService<IHostEnvironment>();
            var queueOptions = builder.ServiceProvider.GetRequiredService<QueueOptions>();

            var dbPath = storage.StorageDir / "db" / $"Queue_{env.EnvironmentName}.db";

            dbPath.Dir().EnsureDirExist();
            
            queueOptions.ConnectionString = dbPath;

            builder.Configuration.UseLiteDbStorage(queueOptions.ConnectionString);
        }
    }
}