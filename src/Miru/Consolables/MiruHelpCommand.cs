using System;
using System.Linq;
using Miru.Core;

namespace Miru.Consolables
{
    // [Description("list all the available commands", Name = "help")]
    // public class MiruHelpCommand : OaktonCommand<HelpInput>
    // {
    //     public MiruHelpCommand()
    //     {
    //         Usage("List all the available commands").Arguments(x => x.Name);
    //         
    //         Usage("Show all the valid usages for a command");
    //     }
    //
    //     public override bool Execute(HelpInput input)
    //     {
    //         if (input.Usage != null)
    //         {
    //             input.Usage.WriteUsages(input.AppName);
    //             return false;
    //         }
    //
    //         if (input.InvalidCommandName)
    //         {
    //             WriteInvalidCommand(input.Name);
    //             ListAllCommands(input);
    //             return false;
    //         }
    //
    //         ListAllCommands(input);
    //
    //         return true;
    //     }
    //
    //     private void ListAllCommands(HelpInput input)
    //     {
    //         if (!input.CommandTypes.Any())
    //         {
    //             Console.WriteLine("There are no known commands!");
    //             return;
    //         }
    //         
    //         var commands = input.CommandTypes.Select(type => new
    //             {
    //                 Name = CommandFactory.CommandNameFor(type),
    //                 Description = CommandFactory.DescriptionFor(type)
    //             });
    //         
    //         var longestCommandName = commands
    //             .Select(m => m.Name)
    //             .Aggregate(string.Empty, (seed, f) => f?.Length > seed.Length ? f : seed);
    //
    //         var padRight = longestCommandName.Length + 2;
    //         
    //         Console2.BreakLine();
    //         
    //         commands.OrderBy(m => m.Name).ForEach(m =>
    //         {
    //             Console2.Yellow(m.Name.PadRight(padRight));
    //             Console2.Write(m.Description);
    //             Console2.BreakLine();
    //         });
    //         
    //         Console2.BreakLine();
    //         Console2.Write("For a command usage, type: ");
    //         Console2.White("miru help <command>");
    //         Console2.BreakLine();
    //         Console2.BreakLine();
    //     }
    //
    //     private void WriteInvalidCommand(string commandName)
    //     {
    //         ConsoleWriter.Line();
    //         Console.ForegroundColor = ConsoleColor.Red;
    //         ConsoleWriter.Write("'{0}' is not a command.  See available commands.", commandName);
    //         Console.ResetColor();
    //         ConsoleWriter.Line();
    //         ConsoleWriter.Line();
    //     }
    // }
}