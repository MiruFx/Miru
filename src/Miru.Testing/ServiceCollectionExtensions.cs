using System;
using FluentEmail.Core.Interfaces;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miru.Databases;
using Miru.Queuing;
using Miru.Urls;
using Miru.Userfy;
using NSubstitute;
using NSubstitute.Extensions;

namespace Miru.Testing
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeatureTesting(this IServiceCollection services) 
        {
            services
                .AddSenderMemory()
                .AddSingleton<IUrlMaps, StubUrlMaps>()
                .AddTestStorage();
            
            return services;
        }

        public static IServiceCollection Mock<TService>(this IServiceCollection services) where TService : class
        {
            services.ReplaceSingleton(Substitute.For<TService>());
            return services;
        }
        
        public static IServiceCollection AddFrom<TConfig>(this IServiceCollection services) where TConfig : ITestConfig, new()
        {
            var config = new TConfig();
            
            config.ConfigureTestServices(services);
            
            return services;
        }
        
        public static IServiceCollection AddSenderMemory(this IServiceCollection services)
        {
            services.AddBothSingleton<ISender, MemorySender>();
            
            return services;
        }

        public static IServiceCollection AddDatabaseCleaner<TDatabaseCleaner>(
            this IServiceCollection services,
            Action<DatabaseCleanerOptions> cfg = null) 
            where TDatabaseCleaner : class, IDatabaseCleaner
        {
            services.AddTransient<IDatabaseCleaner, TDatabaseCleaner>();
            
            services.Configure<DatabaseCleanerOptions>(x =>
            {
                x.AddTableToIgnore("__MigrationHistory");
                x.AddTableToIgnore("VersionInfo");
                x.AddTableToIgnore("__efmigrationshistory");
            });

            if (cfg != null) 
                services.Configure(cfg);
            
            return services;
        }
        
        public static IServiceCollection AddMiruTestFixture(this IServiceCollection services)
        {
            return services
                .AddSingleton<IMiruApp>(sp => new MiruApp(sp))
                .AddSingleton<TestFixture>()
                .AddSingleton<ITestFixture>(sp => sp.GetRequiredService<TestFixture>());
        }
    }
}