using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers
{
    // [Description("Make a FluentMigrator Migration", Name = "make:migration")]
    // public class MakeMigrationConsolable : OaktonConsolableSync<MakeMigrationConsolable.Input>
    // {
    //     private readonly Maker _maker;
    //
    //     public class Input
    //     {
    //         public string Name { get; set; }
    //         
    //         public string TableFlag { get; set; }
    //     }
    //     
    //     public MakeMigrationConsolable(Maker maker)
    //     {
    //         _maker = maker;
    //     }
    //
    //     public override bool Execute(Input input)
    //     {
    //         Console2.BreakLine();
    //         
    //         _maker.Migration(input.Name, table: input.TableFlag);
    //
    //         return true;
    //     }
    // }
}