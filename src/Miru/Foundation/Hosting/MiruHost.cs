using System.Collections.Generic;
using System.Net;
using Baseline;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Miru.Config;
using Miru.Consolables;
using Miru.Core;
using Miru.Foundation.Bootstrap;
using Miru.Foundation.Logging;
using Miru.Mailing;
using Miru.Settings;
using Oakton.Help;
using Serilog;
using Serilog.Events;

namespace Miru.Foundation.Hosting
{
    public static class MiruHost
    {
        private static IConfigurationRoot _config;

        public static IHostBuilder CreateMiruHost(params string[] args) =>
            Host.CreateDefaultBuilder()
                .UseEnvironmentFromArgs(args)
                .ConfigureSerilog(config =>
                {
                    config
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                        .MinimumLevel.Override("System", LogEventLevel.Fatal)
                        .MinimumLevel.Override("Hangfire", LogEventLevel.Fatal)
                        .MinimumLevel.Override("Miru", LogEventLevel.Debug)
                        .WriteTo.Console(outputTemplate: LoggerConfigurations.TimestampOutputTemplate);
                })
                .UseSolution()
                .ConfigureAppConfiguration((hostingContext, cfg) =>
                {
                    cfg.AddEnvironmentVariables(prefix: "Miru_");
                    cfg.AddCommandLine(args);
                    cfg.AddEnvironmentVariables();
                    cfg.AddConfigYml(hostingContext.HostingEnvironment.EnvironmentName);
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopmentOrTest();
                })
                .ConfigureServices((host, services) =>    
                {
                    var argsConfig = new ArgsConfiguration(args);
                    
                    // Host
                    services.AddServiceCollection();
                    services.AddSingleton(argsConfig);
                    services.AddSingleton(App.Solution);
                    services.AddTransient<ScopedServices, ScopedServices>();
                    services.AddSingleton<MiruRunner>();
                    services.AddSingleton<IMiruHost, WebMiruHost>();
                    services.AddSingleton<IMiruHost, CliMiruHost>();

                    // Consolables
                    services.AddConsolableHost();
                    
                    // AppConfig
                    services.Configure<DatabaseOptions>(host.Configuration.GetSection("Database"));
                    services.Configure<MailingOptions>(host.Configuration.GetSection("Mailing"));
                    services.AddSingleton(sp => sp.GetService<IOptions<DatabaseOptions>>().Value);
                    
                    services.AddSingleton<IMiruApp>(sp => new MiruApp(sp));
                });
        
        public static IHostBuilder CreateMiruHost<TStartup>(params string[] args) where TStartup : class =>
            CreateMiruHost(args)
                .ConfigureServices(services =>
                {
                    services.AddAppLogger<TStartup>();
                })
                .AddWebHost<TStartup>(args);
        
        public static IHostBuilder AddWebHost<TStartup>(this IHostBuilder builder, string[] args) 
            where TStartup : class =>
                builder.ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseEnvironmentFromArgs(args)
                        .UseStartup<TStartup>()
                        // .UseKestrel(cfg =>
                        // {
                        //     if (_config["port"] != null)
                        //         cfg.Listen(IPAddress.Loopback, _config["port"].ToInt());
                        // })
                        .UseContentRoot(App.Solution.AppDir);
                });

        public static IHostBuilder UseEnvironmentFromArgs(this IHostBuilder builder, params string[] args) =>
            builder.UseEnvironment(GetEnvironmentName(args));

        public static IWebHostBuilder UseEnvironmentFromArgs(this IWebHostBuilder builder, params string[] args) =>
                builder.UseEnvironment(GetEnvironmentName(args));
        
        private static string GetEnvironmentName(string[] args)
        {
            // For some reason, just using AddCommandLine with switchs "-e" to HostDefaults.EnvironmentKey
            // in ConfigureHostConfiguration, was not setting the correct environment for the host
            // That's why we load environment from args twice, one for HostBuilder and other for WebHostBuilder
            
            var cfg = new ConfigurationBuilder();

            cfg.AddCommandLine(args, new Dictionary<string, string>
            {
                {"-e", HostDefaults.EnvironmentKey},
                {"-p", "port"}
            });

            _config = cfg.Build();
            
            var environmentName = _config[HostDefaults.EnvironmentKey] ?? Environments.Development;
            
            return environmentName;
        }

        private static IHostBuilder UseSolution(this IHostBuilder builder)
        {
            var solution = new SolutionFinder().FromCurrentDir().Solution;
            
            App.Name = solution.Name;
            App.Solution = solution;

            return builder;
        }
    }
}