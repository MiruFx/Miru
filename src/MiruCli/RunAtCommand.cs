using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Miru.Core;
using MiruCli.Process;

namespace MiruCli
{
    public class RunAtCommand : Command
    {
        public RunAtCommand(string commandName) : base(commandName)
        {
            AddArgument(new Argument<string>("executable") { Arity = ArgumentArity.ExactlyOne });
            AddArgument(new Argument<string[]>("args") { Arity = ArgumentArity.ZeroOrMore });
        }
    }
}