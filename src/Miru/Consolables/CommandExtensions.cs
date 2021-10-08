using System.CommandLine;

namespace Miru.Consolables
{
    public static class CommandExtensions
    {
        public static Option WithAlias(this Option option, string alias)
        {
            option.AddAlias(alias);
            return option;
        }
    }
}