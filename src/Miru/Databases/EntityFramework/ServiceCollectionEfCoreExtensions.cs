using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Databases.Migrations;
using Miru.Databases.Migrations.FluentMigrator;
using Miru.Foundation.Bootstrap;
using Miru.Settings;

namespace Miru.Databases.EntityFramework
{
    public static class ServiceCollectionEfCoreExtensions
    {
        public static IServiceCollection AddEfCoreServices<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<IDataAccess, EntityFrameworkDataAccess>();
            
            services.AddTransient<IDatabaseCreator, EntityFrameworkDatabaseCreator>();
            
            // Forward
            services.ForwardScoped<DbContext, TDbContext>();
            
            services
                .AddBeforeSaveHandler<TimeStampedBeforeSaveHandler>()
                .AddBeforeSaveHandler<BelongsToUserBeforeSaveHandler>();

            return services;
        }
    }
}