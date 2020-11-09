using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;

namespace Miru
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseMiru(this IWebHostBuilder builder, IMiruApp app)
        {
            // var solution = app.Get<Solution>();
            //
            // builder.UseContentRoot(solution.AppDir);
            //     
            // builder.ConfigureServices(services =>
            // {
            //     var container = app.Get<IServiceProvider>();
            //     services.AddSingleton(container);
            // });

            return builder;
        }
        
        public static IWebHostBuilder UseKestrelAnyLocalPort(this IWebHostBuilder host) =>
            host.UseKestrel(options => options.Listen(IPAddress.Loopback, 0));
        
        public static IWebHost BuildForMiru(this IWebHostBuilder builder, IMiruApp app)
        {
            return builder.UseMiru(app).Build();
        }
    }
}