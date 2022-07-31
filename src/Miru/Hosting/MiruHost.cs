using System.Collections.Generic;
using System.Reflection;
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
using Miru.Queuing;
using Miru.Settings;
using Miru.Urls;
using Serilog;
using Serilog.Events;

namespace Miru.Hosting;

public static class MiruHost
{
    /// <summary>
    /// Create a basic Miru host for consoles and automated tests
    /// </summary>
    public static IHostBuilder CreateMiruHost(params string[] args) =>
        Host.CreateDefaultBuilder()
            .UseMiruSolution()
            .AddMiruHost(args);

    /// <summary>
    /// Create a Miru host for web application
    /// </summary>
    public static IHostBuilder CreateMiruWebHost<TStartup>(params string[] args) where TStartup : class =>
        Host.CreateDefaultBuilder()
            .UseMiruSolution()
            // web host should come first. miru host will override the default asp.net host configurations
            .AddWebHost<TStartup>()
            .AddMiruHost(args)
            .ConfigureServices(services =>
            {
                services.AddAppLogger<TStartup>();
            });

    public static IHostBuilder AddMiruHost(this IHostBuilder builder, params string[] args) =>
        builder
            .UseEnvironment("Development")
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
                
                services.AddServiceCollection();
                services.AddSingleton(argsConfig);
                
                // Miru Host
                if (argsConfig.IsRunningWebApp)
                    services.AddSingleton<IMiruHost, WebMiruHost>();
                else
                    services.AddSingleton<IMiruHost, CliMiruHost>();

                // AppConfig
                services.Configure<DatabaseOptions>(host.Configuration.GetSection("Database"));
                services.Configure<MailingOptions>(host.Configuration.GetSection("Mailing"));
                services.Configure<UrlOptions>(host.Configuration.GetSection("Url"));
                services.Configure<QueueingOptions>(host.Configuration.GetSection("Queueing"));

                services.AddSingleton(sp => sp.GetRequiredService<IOptions<DatabaseOptions>>().Value);

                services.AddMiruApp();
            });
        
    public static IHostBuilder AddWebHost<TStartup>(this IHostBuilder builder) 
        where TStartup : class =>
        builder
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseStartup<TStartup>()
                    .UseContentRoot(App.Solution.AppDir);
            });
        
    public static IHostBuilder UseMiruSolution(this IHostBuilder builder)
    {
        // if can't find solution, maybe it is running from compiled binaries
        var solution = 
            new SolutionFinder().FromCurrentDir().Solution ?? 
            new UnknownSolution();

        App.Name = solution.Name;
        App.Solution = solution;
        App.Assembly = Assembly.GetEntryAssembly();

        builder.ConfigureServices(services =>
        {
            services.AddMiruSolution(solution);
        });
            
        return builder;
    }
}