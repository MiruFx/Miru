using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.Foundation.Bootstrap;

namespace Miru.Hosting;

public class WebMiruHost : IMiruHost
{
    private readonly MiruSolution _solution;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IHost _host;
    private readonly ArgsConfiguration _argsConfig;

    public WebMiruHost(
        IHost host, 
        MiruSolution solution, 
        IHostEnvironment hostEnvironment, 
        ArgsConfiguration argsConfig)
    {
        _host = host;
        _solution = solution;
        _hostEnvironment = hostEnvironment;
        _argsConfig = argsConfig;
    }

    public async Task RunAsync(CancellationToken token = default)
    {
        LogStartupIntro();
        
        try
        {
            await _host.StartAsync(token);

            DumpListeningAddresses();
                
            await _host.WaitForShutdownAsync(token);
        }
        finally
        {
            if (_host is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();
            else
                _host.Dispose();
        }
    }

    private void DumpListeningAddresses()
    {
        var addresses = _host.Services
            .GetService<IServer>()?
            .Features
            .Get<IServerAddressesFeature>()?
            .Addresses;
            
        if (addresses != null)
        {
            App.Framework.Fatal("\tApp running at: {Addresses}", addresses.Join(", "));
            App.Framework.Fatal("");
        }
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