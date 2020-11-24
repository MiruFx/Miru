using Baseline;
using Microsoft.Extensions.DependencyInjection;
using Miru.Config;
using Miru.Core;
using Miru.Makers;
using Oakton.Help;

namespace Miru.Consolables
{
    public static class ServiceTaskExtensions
    {
        public static IServiceCollection AddConsolableHost(this IServiceCollection services) 
        {
            services.AddSingleton<MiruCommandCreator>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<Maker>();
            
            services.AddConsolable<ConfigShowConsolable>();
            
            services.Scan(scan => scan
                .FromAssemblies(typeof(ServiceTaskExtensions).Assembly)
                .AddClasses(classes =>
                {
                    classes
                        .AssignableTo<IConsolable>()
                        .InNamespaceOf<MakeConsolableConsolable>();
                })
                .AsSelf()
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            
            services.AddScoped<HelpCommand>();

            return services;
        }
        
        public static IServiceCollection AddConsolable<TConsolable>(this IServiceCollection services) 
            where TConsolable : class, IConsolable
        {
            services.AddScoped<IConsolable, TConsolable>();
            services.AddScoped<TConsolable>();
        
            return services;
        }

        public static IServiceCollection AddConsolables<TAssemblyOfType>(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblies(typeof(TAssemblyOfType).Assembly)
                .AddClasses(classes =>
                {
                    classes.AssignableTo<IConsolable>();
                })
                .AsSelf()
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        
            return services;
        }
    }
}