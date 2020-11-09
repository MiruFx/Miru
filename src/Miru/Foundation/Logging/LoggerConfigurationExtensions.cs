using Serilog;
using Serilog.Events;

namespace Miru.Foundation.Logging
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration EfCoreSql(this LoggerConfiguration config, LogEventLevel level)
        {
            return config.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", level);
        }
        
        public static LoggerConfiguration AspNet(this LoggerConfiguration config, LogEventLevel level)
        {
            return config.MinimumLevel.Override("Microsoft.AspNetCore", level);
        }
        
        public static LoggerConfiguration PageTesting(this LoggerConfiguration config, LogEventLevel level)
        {
            config.Testing(level);
            
            return config.MinimumLevel.Override("Miru.PageTesting", level);
        }
        
        public static LoggerConfiguration Testing(this LoggerConfiguration config, LogEventLevel level)
        {
            return config.MinimumLevel.Override("Miru.Testing", level);
        }
    }
}