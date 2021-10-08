using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers
{
    // [Description("Make a new Mailable", Name = "make:mail")]
    // public class MakeMailConsolable : OaktonConsolableSync<MakeMailConsolable.Input>
    // {
    //     private readonly MiruSolution _solution;
    //
    //     public MakeMailConsolable(MiruSolution solution)
    //     {
    //         _solution = solution;
    //     }
    //
    //     public class Input
    //     {
    //         public string In { get; set; }
    //
    //         public string Name { get; set; }
    //     }
    //     
    //     public override bool Execute(Input input)
    //     {
    //         var make = new Maker(_solution);
    //         
    //         Console2.BreakLine();
    //
    //         make.Mail(input.In, input.Name);
    //         
    //         return true;
    //     }
    // }
}