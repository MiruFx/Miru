using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Miru.Testing;
using Serilog;

namespace Miru.PageTesting
{
    public static class TestFixtureHostExtensions
    {
        public static TestFixture StartServer(this TestFixture fixture)
        {
            var pageTestingOptions = fixture.Get<PageTestingConfig>();

            var host = fixture.Get<IHost>();

            try
            {
                host.Start();
            }
            catch (Exception exception)
            {
                throw new MiruPageTestException(
                    "Could not start host for the App. Check the Inner Exception and your Program.cs/Startup.cs configurations",
                    exception.InnerException ?? exception);
            }

            if (pageTestingOptions.StartLocalServer == false)
                return fixture;
            
            var server = fixture.Get<IServer>();
                
            var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            
            if (addresses.Count == 0)
                throw new MiruPageTestException(
                    "The App's Server has no http addresses associated to it. Maybe the App is already running in another process?");
            
            var address = addresses.First();

            pageTestingOptions.BaseUrl = address;

            fixture.SendRequestToServer(address);
            
            return fixture;
        }

        public static ILogger Log(this TestFixture fixture)
        {
            return fixture.Get<ILogger>();
        }

        public static TestFixture SendRequestToServer(this TestFixture fixture, string address)
        {
            using var httpClient = new HttpClient();
            
            MiruTest.Log.Debug($"Warming up server making a request to {address}");
            
            httpClient.GetAsync(address).GetAwaiter().GetResult();

            return fixture;
        }

        public static TestFixture QuitBrowser(this TestFixture fixture)
        {
            var nav = fixture.Get<MiruNavigator>();
            
            MiruTest.Log.Debug("Closing browser");
            
            nav.CloseDrivers();
            
            MiruTest.Log.Debug("Disposing MiruNavigator");
            
            nav.Dispose();
            
            MiruTest.Log.Debug("Browser closed");
            
            return fixture;
        }
    }
}