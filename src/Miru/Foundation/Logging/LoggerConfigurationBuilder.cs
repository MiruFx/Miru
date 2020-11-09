using System;
using Serilog;

namespace Miru.Foundation.Logging
{
    public class LoggerConfigurationBuilder : ILoggerConfigurationBuilder
    {
        public Action<LoggerConfiguration> Config { get; }

        public LoggerConfigurationBuilder(Action<LoggerConfiguration> config)
        {
            Config = config;
        }
    }
}