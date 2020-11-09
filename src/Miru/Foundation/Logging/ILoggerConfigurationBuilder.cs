using System;
using Serilog;

namespace Miru.Foundation.Logging
{
    public interface ILoggerConfigurationBuilder
    {
        Action<LoggerConfiguration> Config { get; }
    }
}