using System.Text;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Databases.EntityFramework;
using Miru.Databases.Migrations;
using Miru.Foundation.Hosting;
using Miru.Settings;

namespace Miru.MySql
{
    public static class EfCoreMySqlServiceCollectionExtensions
    {
        public static IServiceCollection AddEfCoreMySql<TDbContext>(
            this IServiceCollection services,
            ServerVersion serverVersion) where TDbContext : DbContext
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
            services.AddEfCoreServices<TDbContext>();
            
            services.AddDbContext<TDbContext>((sp, options) =>
            {
                var dbConfig = sp.GetService<DatabaseOptions>();

                options.UseMySql(dbConfig.ConnectionString, ServerVersion.FromString("15"));

                if (sp.GetService<IHostEnvironment>().IsDevelopmentOrTest())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            services.AddMigrator<TDbContext>(mb => mb.AddMySql5());
            
            return services;
        }
    }
}