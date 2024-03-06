using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Miru.Testing
{
    public static class NUnitLoggerConfigurationExtensions
    {
        const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}  {Exception}";

        public static LoggerConfiguration NUnitOutput(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null,
            string outputTemplate = DefaultOutputTemplate)
        {
            if (sinkConfiguration == null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

            return sinkConfiguration.Sink(new NUnitSink(formatter), restrictedToMinimumLevel, levelSwitch);
        }
    }
}