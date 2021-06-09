using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Foundation.Logging;
using Miru.Userfy;
using Serilog;

namespace Miru
{
    public class App
    {
        public static Func<DateTime> Now { get; set; } = () => DateTime.Now;
        
        public static ILogger Log => ServiceProvider.GetService<AppLoggerFactory>().GetLogger();

        internal static ILogger Framework => Serilog.Log.ForContext<App>();

        public static string Name { get; internal set; }

        internal static IServiceProvider ServiceProvider { get; set; }
        
        public static MiruSolution Solution { get; set; }
        
        public static Assembly Assembly { get; internal set; }
    }
}
