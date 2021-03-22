using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Miru.Foundation.Hosting
{
    public class WebMiruHost : IMiruHost
    {
        private readonly IHost _host;
        private readonly ILogger<IMiruHost> _logger;

        public WebMiruHost(IHost host, ILogger<IMiruHost> logger)
        {
            _host = host;
            _logger = logger;
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
            var addresses = _host.Services.GetService<IServer>().Features.Get<IServerAddressesFeature>()?.Addresses;
            
            if (addresses != null)
            {
                _logger.LogCritical($"\tApp running at: {addresses.Join(", ")}");
                _logger.LogCritical(string.Empty);
            }
        }
    }
}