using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Miru.Config;
using Miru.Settings;

namespace Miru.Databases.EntityFramework
{
    public static class DatabaseConfigExtensions
    {
        // public static DatabaseConfig UseEfCore<TDbContext>(
        //     this DatabaseConfig config,
        //     Action<DbContextOptionsBuilder, AppSettings> optionsAction) where TDbContext : DbContext
        // {
        //     config.Config.Services.AddDbContext<TDbContext>((services, options) =>
        //     {
        //         optionsAction(options, services.GetService<AppSettings>());
        //     });
        //     
        //     // config.Config.Services2
        //     //     .For<TDbContext>().ContainerScoped();
        //     //
        //     // config.Config.Services2.Forward<TDbContext, DbContext>();
        //
        //     return config;
        // }
    }
}