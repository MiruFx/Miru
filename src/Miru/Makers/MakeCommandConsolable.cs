using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers
{
    // [Description("Make a new Command", Name = "make:command")]
    // public class MakeCommandConsolable : OaktonConsolableSync<MakeCommandConsolable.Input>
    // {
    //     private readonly MiruSolution _solution;
    //
    //     public MakeCommandConsolable(MiruSolution solution)
    //     {
    //         _solution = solution;
    //     }
    //
    //     public class Input
    //     {
    //         public string In { get; set; }
    //
    //         public string Name { get; set; }
    //         
    //         public string Action { get; set; }
    //     }
    //     
    //     public override bool Execute(Input input)
    //     {
    //         var make = new Maker(_solution);
    //         
    //         Console2.BreakLine();
    //
    //         make.Command(input.In, input.Name, input.Action);
    //         
    //         return true;
    //     }
    // }
}