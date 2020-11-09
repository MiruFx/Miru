using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Miru
{
    public static class WebHostExtensions
    {
        public static string ListeningAddress(this IWebHost host)
        {
            return host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
        }
        
        public static string ListeningAddress(this IHost host)
        {
            return host.Services.GetService<IServer>()?.Features.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault();
        }
    }
}