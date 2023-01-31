using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
using Miru.Queuing;

namespace Miru.SqlServer
{
    public static class SqlServerQueuingBuilderExtensions
    {
        public static void UseSqlServer(this QueuingBuilder builder)
        {
            var databaseOptions = builder.ServiceProvider.GetRequiredService<DatabaseOptions>();
         
            builder.Configuration.UseSqlServerStorage(databaseOptions.ConnectionString);
        }
    }
}