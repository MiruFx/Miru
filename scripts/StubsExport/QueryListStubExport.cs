namespace Scripts.StubsExport;

public class QueryListStubExport : StubExport
{
    public QueryListStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        var featureDir = Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Tickets";
        var testDir = Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Tickets";
        var pageTestDir = Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Tickets";
            
        ExportFile(featureDir / "TicketDone.cs", "List-Query", "List");
        ExportFile(featureDir / "Done.cshtml", "List-Query.cshtml", "List");
            
        ExportFile(testDir / "TicketDoneTest.cs", "List-QueryTest", "List");
        ExportFile(pageTestDir / "TicketDonePageTest.cs", "List-QueryPageTest", "List");
    }
}