using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Miru.Core;
using MiruCli.Process;

namespace MiruCli
{
    public class AtPageTestCommand : Command
    {
        private readonly MiruSolution _miruSolution;

        public AtPageTestCommand(string commandName, MiruSolution miruSolution) : base(commandName)
        {
            _miruSolution = miruSolution;
            
            AddArgument(new Argument<string>("executable") { Arity = ArgumentArity.ExactlyOne });
            AddArgument(new Argument<string[]>("args") { Arity = ArgumentArity.ZeroOrMore });
            
            Handler = CommandHandler.Create((string executable, string[] args) => Execute(executable, args));
        }

        private void Execute(string executable, string[] args)
        {
            var processRunner = new MiruProcessRunner(prefix: string.Empty);

            var proc = processRunner.RunAsync(new ProcessSpec()
            {
                Executable = executable,
                Arguments = args,
                WorkingDirectory = _miruSolution.AppPageTestsDir
            });

            Task.WaitAll(proc);
        }
    }
}