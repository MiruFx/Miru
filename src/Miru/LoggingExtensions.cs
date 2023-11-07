using System.Diagnostics;
using MediatR;

namespace Miru;

public static class LoggingExtensions
{
    /// <summary>
    /// Dump the current object properties into the log in debug mode (App.Log.Debug)
    /// </summary>
    /// <returns>Returns the own object</returns>
    public static T LogIt<T>(this T value)
    {
        App.Framework.Debug($"{value.GetType().ActionName()}: {Environment.NewLine}{value.Inspect()}");
        return value;
    }
        
    public static string Inspect<T>(this T value)
    {
        if (value == null)
            return "null";

        var yaml = value.ToYml();

        if (yaml.StartsWith("{}") || yaml.IsEmpty())
            return "Empty";

        return yaml;
    }
        
    public static void MeasureToConsole(this object current, Action action, string taskName = "Task")
    {
        Console.WriteLine($"{taskName} started");
        
        var timer = new Stopwatch();
        timer.Start();

        action();
            
        timer.Stop();
        Console.WriteLine($"{taskName} executed in {timer.ElapsedMilliseconds} ms");
    }
        
    public static T DumpToConsole<T>(this T value, string prefix = null)
    {
        if (prefix.NotEmpty()) 
            Console.Write(prefix);
                
        if (value.GetType().Implements(typeof(IRequest<>)))
            Console.WriteLine($"{value.GetType().ActionName()}: {Environment.NewLine}{value.Inspect()}");
        else 
            Console.WriteLine($"{value.GetType().GetName()}: {Environment.NewLine}{value.Inspect()}");
            
        return value;
    }
        
    public static string DumpToConsole(this string value)
    {
        Console.WriteLine(value);
        return value;
    }
}