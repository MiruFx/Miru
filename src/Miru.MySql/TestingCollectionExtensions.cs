using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;

namespace Miru.MySql
{
    public static class TestingServiceCollectionExtensions
    {
        public static IServiceCollection AddMySqlDatabaseCleaner(this IServiceCollection services)
        {
            return services.AddTransient<IDatabaseCleaner, MySqlDatabaseCleaner>();
        }
    }
}