using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.Foundation.Hosting;
using Miru.Mvc;
using Miru.Urls;

namespace Miru.Tests
{
    public class MiruTestWebHost<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
    }

    public class MiruTestWebHost : WebApplicationFactory<MiruTestWebHost.Startup>
    {
        public static Action<IServiceProvider> Action;
        public static Action<IServiceCollection> ServicesConfig;
        
        private readonly IHostBuilder _builder;

        public MiruTestWebHost(Action<IServiceCollection> servicesConfig)
        {
            ServicesConfig = servicesConfig;
            
            _builder = MiruHost.CreateMiruHost();
        }

        public MiruTestWebHost(IHostBuilder builder, Action<IServiceCollection> servicesConfig = null)
        {
            ServicesConfig = servicesConfig;
                
            _builder = builder;
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return _builder;
        }

        public IHostBuilder GetConfiguredHostBuilder()
        {
            return _builder.ConfigureWebHost(webBuilder =>
            {
                var tempSolution = new UnknownSolution();

                webBuilder
                    .UseStartup<Startup>()
                    .UseContentRoot(tempSolution.AppDir)
                    .UseTestServer();
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = GetConfiguredHostBuilder().Build();
            host.Start();
            return host;
        }

        public void WithHttpContext(Action<IServiceProvider> action)
        {
            var client = CreateClient();

            MiruTestWebHost.Action = action;

            client.GetAsync("/_miru_host_test").Result.EnsureSuccessStatusCode();
        }
        
        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                if (ServicesConfig != null)
                    ServicesConfig(services);
                else
                {
                    services
                        .AddMvcCore()
                        .AddMiruActionResult()
                        .AddMiruNestedControllers();
                    
                    services.AddMiruUrls();
                
                    services.AddControllersWithViews();
                }
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseRouting();

                app.UseEndpoints(e => { e.MapDefaultControllerRoute(); });
            }
        }
        
        [Route("/_miru_host_test")]
        public class MiruHostTestController
        {
            private readonly IServiceProvider _serviceProvider;

            public MiruHostTestController(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }
            
            public bool Index()
            {
                MiruTestWebHost.Action(_serviceProvider);
                return true;
            }
        }
    }
}