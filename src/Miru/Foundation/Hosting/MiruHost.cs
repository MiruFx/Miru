using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Miru.Consolables;
using Miru.Core;
using Miru.Foundation.Bootstrap;
using Miru.Foundation.Logging;
using Miru.Mailing;
using Miru.Settings;
using Miru.Urls;
using Serilog;
using Serilog.Events;

namespace Miru.Foundation.Hosting
{
    public static class MiruHost
    {
        public static IHostBuilder CreateMiruHost(params string[] args) =>
            Host.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .UseMiruSolution()
                .ConfigureHostConfiguration(cfg =>
                {
                    cfg.AddEnvironmentVariables("MIRU_");
                    cfg.AddCommandLine(args, new Dictionary<string, string>
                    {
                        {"-e", HostDefaults.EnvironmentKey},
                        {"-p", "port"}
                    });
                })
                .ConfigureSerilog(config =>
                {
                    config
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                        .MinimumLevel.Override("System", LogEventLevel.Fatal)
                        .MinimumLevel.Override("Hangfire", LogEventLevel.Fatal)
                        .MinimumLevel.Override("Miru", LogEventLevel.Fatal)
                        .WriteTo.Console(outputTemplate: LoggerConfigurations.TimestampOutputTemplate);
                })
                .ConfigureAppConfiguration((hostingContext, cfg) =>
                {
                    var env = hostingContext.HostingEnvironment.EnvironmentName;
                    
                    cfg.AddEnvironmentVariables(prefix: "MIRU_");
                    cfg.AddYamlFile("appSettings.yml", optional: true, reloadOnChange: true);
                    cfg.AddYamlFile($"appSettings.{env}.yml", optional: true, reloadOnChange: true);
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
                    services.AddSingleton<MiruRunner>();
                    services.AddSingleton<IMiruHost, WebMiruHost>();
                    services.AddSingleton<IMiruHost, CliMiruHost>();

                    // Consolables
                    services.AddConsolableHost();
                    
                    // AppConfig
                    services.Configure<DatabaseOptions>(host.Configuration.GetSection("Database"));
                    services.Configure<MailingOptions>(host.Configuration.GetSection("Mailing"));
                    services.Configure<UrlOptions>(host.Configuration.GetSection("Url"));
                    
                    services.AddSingleton(sp => sp.GetRequiredService<IOptions<DatabaseOptions>>().Value);

                    services.AddMiruApp();
                });
        
        public static IHostBuilder CreateMiruHost<TStartup>(params string[] args) where TStartup : class =>
            CreateMiruHost(args)
                .ConfigureServices(services =>
                {
                    services.AddAppLogger<TStartup>();
                })
                .AddWebHost<TStartup>();
        
        public static IHostBuilder AddWebHost<TStartup>(this IHostBuilder builder) 
            where TStartup : class =>
                builder
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder
                            .UseStartup<TStartup>()
                            .UseContentRoot(App.Solution.AppDir);
                    });
        
        private static IHostBuilder UseMiruSolution(this IHostBuilder builder)
        {
            // if can't find solution, maybe it is running from compiled binaries
            var solution = 
                new SolutionFinder().FromCurrentDir().Solution ?? 
                new UnknownSolution();

            App.Name = solution.Name;
            App.Solution = solution;

            builder.ConfigureServices(services =>
            {
                services.AddMiruSolution();
            });
            
            return builder;
        }
    }
}