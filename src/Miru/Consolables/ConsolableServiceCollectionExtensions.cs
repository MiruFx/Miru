using Microsoft.Extensions.DependencyInjection;
using Oakton.Help;

namespace Miru.Consolables
{
    public static class ServiceTaskExtensions
    {
        public static IServiceCollection AddCliCommand<TCliCommand>(this IServiceCollection services) 
            where TCliCommand : class, IConsolable
        {
            services.AddTransient<IConsolable, TCliCommand>();
            services.AddTransient<TCliCommand>();
        
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
                .WithTransientLifetime());

            services.AddSingleton<HelpCommand>();

            return services;
        }
    }
}