using System;
using Serilog;
using Serilog.Events;

namespace Miru.Foundation.Logging
{
    public static class LoggerExtensions
    {
        public static void Error(this ILogger logger, Exception exception)
        {
            logger.Error(string.Empty, exception);
        }
        
        public static void Debug(this ILogger logger, Func<string> log)
        {
            if (logger.IsEnabled(LogEventLevel.Debug))
                logger.Debug(log());
        }
    }
}
