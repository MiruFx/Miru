using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases.EntityFramework;

namespace Miru.Tests
{
    public static class EfCoreSqliteServiceCollectionExtensions
    {
        public static IServiceCollection AddEfCoreInMemory<TDbContext>(
            this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddEfCoreServices<TDbContext>();
            
            services.AddDbContext<TDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase(typeof(TDbContext).Name);
                options.ConfigureWarnings(_ =>
                {
                    _.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                });
            });
            
            return services;
        }
    }
}