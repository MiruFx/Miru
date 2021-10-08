using Humanizer;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers
{
    // [Description("Make a Entity", Name = "make:entity")]
    // public class MakeEntityConsolable : OaktonConsolableSync<MakeEntityConsolable.Input>
    // {
    //     private readonly Maker _maker;
    //
    //     public class Input
    //     {
    //         public string Name { get; set; }
    //     }
    //     
    //     public MakeEntityConsolable(Maker maker)
    //     {
    //         _maker = maker;
    //     }
    //
    //     public override bool Execute(Input input)
    //     {
    //         Console2.BreakLine();
    //         
    //         _maker.Entity(input.Name);
    //
    //         Console2.BreakLine();
    //         
    //         Console2.GreyLine("\tConsider adding to your DbContext:");
    //         Console2.BreakLine();
    //         Console2.WhiteLine($"\t\tpublic DbSet<{input.Name}> {input.Name.Pluralize()} {{ get; set; }}");
    //         
    //         return true;
    //     }
    // }
}