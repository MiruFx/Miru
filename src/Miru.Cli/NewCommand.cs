using System.CommandLine;
using System.CommandLine.Invocation;
using Miru.Core;
using Miru.Core.Makers;

namespace Miru.Cli
{
    public class NewCommand : Command
    {
        public NewCommand(string commandName) : base(commandName)
        {
            AddArgument(new Argument<string>("name", "Solution's name. Ex: StackOverflow, Amazon"));
            
            Handler = CommandHandler.Create((string name) => Execute(name));
        }

        private void Execute(string name)
        {
            var m = new Maker(new MiruSolution(A.Path / name));

            m.New(name);
        }
    }
}