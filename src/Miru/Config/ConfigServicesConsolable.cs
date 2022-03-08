using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Config
{
    public class ConfigServicesConsolable : Consolable
    {
        public ConfigServicesConsolable() : 
            base("config.container", "Show services registered in the container")
        {
        }

        public class ConsolableHandler : IConsolableHandler
        {
            private readonly IServiceCollection _services;

            public ConsolableHandler(IServiceCollection services) =>
                _services = services;

            public Task Execute()
            {
                var services = _services.Where(x =>
                    !x.ServiceType.Namespace.StartsWith("Microsoft") &&
                    !x.ServiceType.Namespace.StartsWith("System"));

                // if (input.Namespace.NotEmpty())
                // TODO: add options to filter assemblies
                services = services.Where(x => 
                        x.ServiceType.Namespace.StartsWith("Mediat") ||
                        x.ServiceType.Namespace.StartsWith("Miru"))
                    .OrderBy(x => x.ServiceType.FullName);
            
                foreach(var service in services)
                {
                    Console2.WhiteLine(service.ServiceType.FullName);
                    Console2.White("\t");
                    Console2.WhiteLine(service.Lifetime.ToString());
                    Console2.White("\t");
                    Console2.WhiteLine(service.ImplementationType?.FullName ?? "None");
                }

                return Task.CompletedTask;
            }
        }
    }
}