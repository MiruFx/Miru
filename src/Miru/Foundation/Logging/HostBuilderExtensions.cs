using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Miru.Foundation.Logging;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder builder, Action<LoggerConfiguration> config)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ILoggerConfigurationBuilder>(new LoggerConfigurationBuilder(config));
                
            services.TryAddSingleton(sp =>
            {
                var loggerConfiguration = new LoggerConfiguration();
            
                var configBuilders = sp.GetServices<ILoggerConfigurationBuilder>();
            
                foreach (var configBuilder in configBuilders)
                {
                    configBuilder.Config(loggerConfiguration);
                }
                    
                var logger = loggerConfiguration.CreateLogger();
            
                return new RegisteredLogger(logger);
            });   
                
            services.ReplaceSingleton<ILoggerFactory>(sp =>
            {
                var logger = sp.GetRequiredService<RegisteredLogger>().Logger;
                    
                Log.Logger = logger;

                var factory = new SerilogLoggerFactory(logger, true);

                return factory;
            });
        });
            
        return builder;
    }
}