using Hangfire.LiteDB;
using Hangfire.Storage.SQLite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Storages;

namespace Miru.Queuing
{
    public static class QueuingBuilderExtensions
    {
        public static void UseLiteDb(this QueuingBuilder builder)
        {
            var storage = builder.ServiceProvider.GetService<Storage>();
            var env = builder.ServiceProvider.GetService<IHostEnvironment>();
            
            builder.Configuration.UseLiteDbStorage(storage.MakePath("db", $"Queue_{env.EnvironmentName}.db"));
        }
        
        public static void UseSqlite(this QueuingBuilder builder)
        {
            var storage = builder.ServiceProvider.GetService<Storage>();
            var env = builder.ServiceProvider.GetService<IHostEnvironment>();
            
            builder.Configuration.UseSQLiteStorage(storage.MakePath("db", $"Queue_{env.EnvironmentName}.db"));
        }
    }
}