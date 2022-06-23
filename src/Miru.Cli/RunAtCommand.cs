using System.CommandLine;

namespace Miru.Cli;

public class RunAtCommand : Command
{
    public RunAtCommand(string commandName) : base(commandName)
    {
        AddArgument(new Argument<string>("executable") { Arity = ArgumentArity.ExactlyOne });
        AddArgument(new Argument<string[]>("args") { Arity = ArgumentArity.ZeroOrMore });
    }
}