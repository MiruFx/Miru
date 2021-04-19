using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.Foundation.Bootstrap;

namespace Miru.Foundation.Hosting
{
    public class MiruRunner
    {
        private readonly IEnumerable<IMiruHost> _hosts;
        private readonly ArgsConfiguration _argsConfig;
        private readonly MiruSolution _solution;
        private readonly IHostEnvironment _hostEnvironment;

        public MiruRunner(
            IEnumerable<IMiruHost> hosts, 
            ArgsConfiguration argsConfig, 
            MiruSolution solution,
            IHostEnvironment hostEnvironment)
        {
            _hosts = hosts;
            _argsConfig = argsConfig;
            _solution = solution;
            _hostEnvironment = hostEnvironment;
        }

        public async Task RunAsync()
        {
            if (!_argsConfig.RunCli)
            {
                App.Framework.Fatal("-----------------------------------------------------------");
                App.Framework.Fatal($"Starting {_solution.Name} {App.Assembly?.GetName().Version}");
                App.Framework.Fatal(string.Empty);
                    
                App.Framework.Fatal($"\tEnvironment: {_hostEnvironment.EnvironmentName}");
                App.Framework.Fatal($"\tSolution directory: {_solution.RootDir}");
                    
                if (_argsConfig.CliArgs.Length > 0)
                    App.Framework.Information($"\tArguments: {_argsConfig.CliArgs.Join(" ")}");
                
                App.Framework.Fatal(string.Empty);
            }

            var host = GetMiruHost();

            await host.RunAsync();
        }

        public IMiruHost GetMiruHost()
        {
            if (_argsConfig.RunCli)
            {
                return _hosts.SingleOrDefault(host => host is CliMiruHost);
            }
            
            return _hosts.SingleOrDefault(host => host is WebMiruHost);
        }
    }
}