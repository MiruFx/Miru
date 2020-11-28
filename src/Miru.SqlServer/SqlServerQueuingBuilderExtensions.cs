using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Miru.Queuing;
using Miru.Settings;

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