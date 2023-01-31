using System.Reflection;
using Serilog;

namespace Miru;

public class App
{
    public static Func<DateTime> Now { get; set; } = () => DateTime.Now;
        
    public static ILogger Log => Serilog.Log.ForContext<App>();

    internal static ILogger Framework => Serilog.Log.ForContext<App>();

    public static string Name { get; internal set; }

    internal static IServiceProvider ServiceProvider { get; set; }
        
    public static MiruSolution Solution { get; set; }
        
    public static Assembly Assembly { get; internal set; }
}