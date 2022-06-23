using System.CommandLine.Builder;
using System.CommandLine.IO;
using System.Reflection;

namespace Miru.Cli;

public static class Extensions
{
    public static CommandLineBuilder UseMiruVersionOption(
        this CommandLineBuilder builder)
    {
        var versionOption = new MiruVersionOption(builder);

        builder.Command.AddOption(versionOption);

        builder.AddMiddleware(async (context, next) =>
        {
            if (context.ParseResult.FindResultFor(versionOption) is { })
            {
                // if (context.ParseResult.Errors.Any(e => e.SymbolResult?.Symbol is MiruVersionOption))
                // {
                //     context.InvocationResult = new ParseErrorResult(null);
                // }
                // else
                // {
                //     context.Console.Out.WriteLine(_assemblyVersion.Value);
                // }
                
                context.Console.Out.WriteLine($"Miru.Cli: {GetAssemblyVersion(Assembly.GetEntryAssembly())}");
                context.Console.Out.WriteLine($"App Miru.dll: {GetAssemblyVersion(Assembly.GetEntryAssembly())}");
                context.Console.Out.WriteLine($"App: {GetAssemblyVersion(Assembly.GetEntryAssembly())}");
            }
            else
            {
                await next(context);
            }
        });

        return builder;
    }

    public static string GetAssemblyVersion(Assembly assembly)
    {
        var assemblyVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        if (assemblyVersionAttribute is null)
        {
            return assembly.GetName().Version?.ToString() ?? "";
        }
        else
        {
            return assemblyVersionAttribute.InformationalVersion;
        }
    }
}