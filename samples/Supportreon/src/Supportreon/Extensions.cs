using Serilog;
using Serilog.Events;

namespace Supportreon
{
    public static class Extensions
    {
        public static LoggerConfiguration EfCoreSql(this LoggerConfiguration config, LogEventLevel level)
        {
            return config.MinimumLevel.Override(
                "Microsoft.EntityFrameworkCore.Database.Command",
                level);
        }
        
        public static LoggerConfiguration AspNet(this LoggerConfiguration config, LogEventLevel level)
        {
            return config.MinimumLevel.Override(
                "Microsoft.AspNetCore",
                level);
        }
    }
}