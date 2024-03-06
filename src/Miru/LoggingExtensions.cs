using Serilog;

namespace Miru;

public static class LoggingExtensions
{
    /// <summary>
    /// Dump the current object properties into the log in debug mode (App.Log.Debug)
    /// </summary>
    /// <returns>Returns the own object</returns>
    public static T Dump<T>(this ILogger logger, T value)
    {
        logger.Debug($"{value.GetType().ActionName()}: {Environment.NewLine}{Yml.Dump(value)}");
        return value;
    }
}