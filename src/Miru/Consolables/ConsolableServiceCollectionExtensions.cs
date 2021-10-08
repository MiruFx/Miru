using System;
using System.CommandLine;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Hosting;

namespace Miru.Consolables
{
    public static class ServiceTaskExtensions
    {
        public static IServiceCollection AddConsolable(this IServiceCollection services, Type consolableType)
        {
            var handlerType = consolableType.Assembly.GetType($"{consolableType.FullName}+ConsolableHandler");
            
            if (handlerType is null)
                throw new ArgumentException(
                    $"Consolable of type {consolableType.FullName} must have a subclass inheriting ConsolableHandler");
            
            services.AddSingleton(typeof(Consolable), consolableType);
            services.AddScoped(handlerType);
        
            return services;
        }
        
        public static IServiceCollection AddConsolable<TConsolable>(this IServiceCollection services) 
            where TConsolable : Consolable
        {
            return services.AddConsolable(typeof(TConsolable));
        }

        public static IServiceCollection AddConsolables<TAssemblyOfType>(this IServiceCollection services)
        {
            var commandTypes = typeof(TAssemblyOfType).Assembly
                .ExportedTypes
                .Where(x => x.Implements<Consolable>() && x.IsAbstract == false);

            foreach (var commandType in commandTypes)
            {
                services.AddConsolable(commandType);
            }
            
            return services;
        }
        
        public static IServiceCollection AddNewConsolableHost(this IServiceCollection services)
        {
            return services.AddSingleton<ICliMiruHost, CliMiruHost>();
        }
    }
}