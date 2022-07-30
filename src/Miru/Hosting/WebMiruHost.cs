using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Miru.Hosting;

public class WebMiruHost : IMiruHost
{
    private readonly IHost _host;

    public WebMiruHost(IHost host)
    {
        _host = host;
    }

    public async Task RunAsync(CancellationToken token = default)
    {
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
}