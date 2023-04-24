using System.CommandLine;

namespace Miru.Cli;

public class BuildRunAtCommand : Command
{
    public BuildRunAtCommand(string commandName) : base(commandName)
    {
        AddArgument(new Argument<string>("command") { Arity = ArgumentArity.ExactlyOne });
        AddArgument(new Argument<string[]>("args") { Arity = ArgumentArity.ZeroOrMore });
    }
}