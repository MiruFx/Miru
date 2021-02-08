using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Config
{
    [Description("Show services registered in the container ", Name = "config:container")]
    public class ConfigServicesConsolable : ConsolableSync<ConfigServicesConsolable.Input>
    {
        public class Input
        {
            // public string Namespace { get; set; }
        }
        
        private readonly IServiceCollection _services;

        public ConfigServicesConsolable(IServiceCollection services)
        {
            _services = services;
        }

        public override bool Execute(Input input)
        {
            var services = _services.Where(x =>
                !x.ServiceType.Namespace.StartsWith("Microsoft") &&
                !x.ServiceType.Namespace.StartsWith("System"));

            // if (input.Namespace.NotEmpty())
                services = services.Where(x => 
                        x.ServiceType.Namespace.StartsWith("Mediat") ||
                        x.ServiceType.Namespace.StartsWith("Miru") ||
                        x.ServiceType.Namespace.StartsWith("Intanext"))
                    .OrderBy(x => x.ServiceType.FullName);
            
            foreach(var service in services)
            {
                Console2.WhiteLine(service.ServiceType.FullName);
                Console2.White("\t");
                Console2.WhiteLine(service.Lifetime.ToString());
                Console2.White("\t");
                Console2.WhiteLine(service.ImplementationType?.FullName ?? "None");
            }
            
            return true;
        }
    }
}