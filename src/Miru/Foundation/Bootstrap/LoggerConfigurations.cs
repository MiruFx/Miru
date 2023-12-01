using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Miru.Foundation.Bootstrap;

public class LoggerConfigurations
{
    public static readonly string TimestampOutputTemplate = "{Timestamp:HH:mm:ss.fff} {Message}{NewLine}{Exception}";
        
    public static readonly string SimpleOutputTemplate = "{Message}{NewLine}{Exception}";
        
    public static readonly string DetailedOutputTemplate = "{Message} - {Level} {SourceContext} {NewLine} {Exception}";
        
    public static readonly ILogger Default = ForCli();

    public static readonly LoggerConfiguration DefaultConfiguration = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console(outputTemplate: SimpleOutputTemplate);
        
    public static ILogger GetLogger(ArgsConfiguration argsConfig) => 
        argsConfig.IsRunningCli ? ForCli() : ForWeb();
        
    public static ILogger ForCli() => new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console(outputTemplate: SimpleOutputTemplate)
        .CreateLogger();
        
    public static ILogger ForWeb() => new LoggerConfiguration()
        .MinimumLevel.Debug()
        // .WriteTo.Console(outputTemplate: TimestampOutputTemplate)
        .CreateLogger();

    public static ILoggerFactory CreateLoggerFactory(Action<LoggerConfiguration> config = null)
    {
        return LoggerFactory.Create(builder =>
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Warning();

            config?.Invoke(loggerConfiguration);

            loggerConfiguration
                .WriteTo.Console(outputTemplate: SimpleOutputTemplate);
                
            builder.AddSerilog(loggerConfiguration.CreateLogger());
        });
    }
}