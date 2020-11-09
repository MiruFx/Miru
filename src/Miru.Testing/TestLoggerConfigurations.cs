using Miru.Foundation;
using Miru.Foundation.Bootstrap;
using Miru.Storages;
using Serilog;

namespace Miru.Testing
{
    public static class TestLoggerConfigurations
    {
        public static ILogger ForTests(Storage storage) => new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteToTestConsole()
            .WriteTo.File(storage.Temp() / "logs" / "FeatureTest.txt", outputTemplate: LoggerConfigurations.TimestampOutputTemplate)
            .CreateLogger();

        public static ILogger ForPageTest(Storage storage)
        {
            var pageTestLog = storage.Temp() / "logs" / "PageTest.txt";

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
}