using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.Foundation.Bootstrap;

namespace Miru.Hosting;

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
        if (_argsConfig.IsRunningCli == false)
        {
            LogStartupIntro();
        }

        var host = GetMiruHost();

        await host.RunAsync();
    }

    public IMiruHost GetMiruHost()
    {
        if (_argsConfig.IsRunningCli)
        {
            return _hosts.SingleOrDefault(host => host is ICliMiruHost);
        }
            
        return _hosts.SingleOrDefault(host => host is WebMiruHost);
    }
        
    private void LogStartupIntro()
    {
        App.Framework.Fatal("-----------------------------------------------------------");
        App.Framework.Fatal("Starting {SolutionName} {Version}", _solution.Name, App.Assembly?.GetName().Version);
        App.Framework.Fatal("");

        App.Framework.Fatal("\tEnvironment: {Environment}", _hostEnvironment.EnvironmentName);
        App.Framework.Fatal("\tSolution directory: {SolutionDirectory}", _solution.RootDir);

        if (_argsConfig.CliArgs.Length > 0)
            App.Framework.Information("\tArguments: {Arguments}", _argsConfig.CliArgs.Join(" "));

        App.Framework.Fatal("");
    }
}