using Miru.Core;

namespace Miru.Foundation.Bootstrap;

public static class ArgsStringExtensions
{
    public static bool RunningMiruCli(this string[] args)
    {
        return args.Length > 0 && args[0].CaseCmp("miru");
    }
}