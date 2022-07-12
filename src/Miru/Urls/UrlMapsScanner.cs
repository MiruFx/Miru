using System;
using Microsoft.AspNetCore.Builder;

namespace Miru.Urls;

public class UrlMapsScanner
{
    private readonly IServiceProvider _serviceProvider;

    public UrlMapsScanner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Scan()
    {
        var builder = new ApplicationBuilder(_serviceProvider);
        
        // forces asp.net core scan for controllers and routes
        // it is automatically scanned when using a WebHost
        // but in other situations, like Testing, Queues, Consolables, it needs 
        // to force scanning
        builder.UseRouting();
        builder.UseEndpoints(x => x.MapDefaultControllerRoute());
        builder.Build();
    }
}