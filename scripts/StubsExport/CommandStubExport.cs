using System;

namespace Scripts.StubsExport;

public class CommandStubExport : StubExport
{
    public CommandStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "TicketEdit.cs", 
            "Command", 
            "Edit");
        
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "Edit.cshtml", 
            "Command.cshtml", 
            "Edit");
        
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "_Edit.turbo.cshtml", 
            "_Command.turbo.cshtml", 
            "Edit");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Tickets" / "TicketEditTest.cs", 
            "CommandTest", 
            "Edit");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Tickets" / "TicketEditPageTest.cs", 
            "CommandPageTest", 
            "Edit");
    }
}