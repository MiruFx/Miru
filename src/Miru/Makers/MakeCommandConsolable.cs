// using System.CommandLine;
// using System.Threading.Tasks;
// using Miru.Consolables;
// using Miru.Core;
//
// namespace Miru.Makers;
//
// public class MakeCommandConsolable : Consolable
// {
//     public MakeCommandConsolable() :
//         base("make.command", "Make a new Command")
//     {
//         Add(new Argument<string>("in"));
//         Add(new Argument<string>("name"));
//         Add(new Argument<string>("action"));
//     }
//
//     public class ConsolableHandler : IConsolableHandler
//     {
//         private readonly MiruSolution _solution;
//     
//         public ConsolableHandler(MiruSolution solution)
//         {
//             _solution = solution;
//         }
//     
//         public string In { get; set; }
//         public string Name { get; set; }
//         public string Action { get; set; }
//         
//         public async Task Execute()
//         {
//             var make = new Maker(_solution);
//             
//             Console2.BreakLine();
//     
//             make.Command(In, Name, Action);
//             
//             Console2.BreakLine();
//
//             await Task.CompletedTask;
//         }   
//     }
// }