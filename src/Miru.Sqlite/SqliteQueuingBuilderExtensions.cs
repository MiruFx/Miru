using Hangfire.LiteDB;
using Hangfire.Storage.SQLite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Queuing;
using Miru.Settings;
using Miru.Storages;

namespace Miru.Sqlite
{
    public static class SqliteQueuingBuilderExtensions
    {
        public static void UseLiteDb(this QueuingBuilder builder)
        {
            var storage = builder.ServiceProvider.GetService<Storage>();
            var env = builder.ServiceProvider.GetService<IHostEnvironment>();
            var queueOptions = builder.ServiceProvider.GetRequiredService<QueueOptions>();

            queueOptions.ConnectionString = storage.MakePath("db", $"Queue_{env.EnvironmentName}.db");

            builder.Configuration.UseLiteDbStorage(queueOptions.ConnectionString);
        }
    }
}