using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Miru.Core;

namespace MiruCli
{
    public class SetupCommand : Command
    {
        public SetupCommand(string commandName) : base(commandName)
        {
            Handler = CommandHandler.Create(Execute);
            IsHidden = true;
        }

        private void Execute()
        {
            var asm = new EmbeddedFiles<Program>();

            var miruShellDestination = OS.IsWindows ? 
                Path.Combine(OS.DotNetToolsPath, "miru.cmd") :
                Path.Combine(OS.DotNetToolsPath, "miru");
            
            asm.ExtractFile(OS.IsWindows ? "miru.cmd" : "miru.sh", miruShellDestination);
            
            if (OS.IsWindows == false)
                OS.ShellToConsole($"chmod +x {miruShellDestination}");
            
            Console.WriteLine();
            Console2.GreenLine("Miru setup successfully. From now use the command anywhere: miru");
            Console.WriteLine();
            Console.WriteLine("Good luck!");
        }
    }
}