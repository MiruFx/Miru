using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;

namespace Miru.Postgres
{
    public static class TestingServiceCollectionExtensions
    {
        public static IServiceCollection AddPostgresDatabaseCleaner(this IServiceCollection services)
        {
            return services.AddTransient<IDatabaseCleaner, PostgresDatabaseCleaner>();
        }
    }
}