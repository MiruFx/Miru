using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;

namespace Miru.Sqlite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteDatabaseCleaner(this IServiceCollection services)
        {
            return services.AddTransient<IDatabaseCleaner, SqliteDatabaseCleaner>();
        }
    }
}