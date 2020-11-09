using FluentEmail.Core.Interfaces;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
using Miru.Urls;
using Miru.Userfy;
using NSubstitute;

namespace Miru.Testing
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeatureTesting(this IServiceCollection services)
        {
            services
                .AddSingleton<IUserSession, TestingUserSession>()
                .AddSenderMemory()
                .AddSingleton<IUrlMaps, StubUrlMaps>();
            
            services.AddHangfire((sp, configuration) => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage());
            
            return services;
        }

        public static IServiceCollection AddSqlServerDatabaseCleaner(this IServiceCollection services)
        {
            return services.AddSingleton<IDatabaseCleaner, SqlServerDatabaseCleaner>();
        }
        
        public static IServiceCollection AddSqliteDatabaseCleaner(this IServiceCollection services)
        {
            return services.AddSingleton<IDatabaseCleaner, SqliteDatabaseCleaner>();
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
    }
}