using Miru.Foundation;
using Miru.Foundation.Bootstrap;
using Miru.Storages;
using Serilog;

namespace Miru.Testing;

public static class TestLoggerConfigurations
{
    public static ILogger ForTests(IStorage storage) => new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteToTestConsole()
        .WriteTo.File(storage.App / "temp" / "logs" / "FeatureTest.txt", outputTemplate: LoggerConfigurations.TimestampOutputTemplate)
        .CreateLogger();

    public static ILogger ForPageTest(IStorage storage)
    {
        var pageTestLog = storage.App / "temp" / "logs" / "PageTest.txt";

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteToTestConsole()
            .WriteTo.File(pageTestLog, outputTemplate: LoggerConfigurations.TimestampOutputTemplate);
            
        return loggerConfiguration.CreateLogger();
    }

    public static LoggerConfiguration WriteToTestConsole(this LoggerConfiguration loggerConfiguration)
    {
        if (TestMiruHost.UsingTestRunner)
            loggerConfiguration.WriteTo.NUnitOutput(outputTemplate: LoggerConfigurations.TimestampOutputTemplate);
        else
            loggerConfiguration.WriteTo.Console(outputTemplate: LoggerConfigurations.TimestampOutputTemplate);

        return loggerConfiguration;
    }
}