using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Baseline;
using Miru.Core;

namespace MiruCli
{
    public class ShowProjectCommand : Command
    {
        public ShowProjectCommand(string commandName, Func<MiruSolution, string> func) : base(commandName)
        {
            Handler = CommandHandler.Create(() => ShowDir(func));
            IsHidden = true;
        }

        private void ShowDir(Func<MiruSolution, string> func)
        {
            var solutionFinder = new SolutionFinder(new FileSystem());
            
            var result = solutionFinder.FromDir(Directory.GetCurrentDirectory());

            if (result.FoundSolution)
                Console.WriteLine(func(result.Solution));
            else
                Console2.YellowLine($"{Directory.GetCurrentDirectory()} is not part of a Miru Solution structure");
        }
    }
}