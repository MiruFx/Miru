using Microsoft.Extensions.DependencyInjection;
using Oakton.Help;

namespace Miru.Consolables
{
    public static class ServiceTaskExtensions
    {
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

            services.AddScoped<HelpCommand>();

            return services;
        }
    }
}