namespace Scripts.StubsExport;

public class QueryShowStubExport : StubExport
{
    public QueryShowStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "TicketShow.cs", 
            "Show-Query", 
            "Show");
        
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Tickets" / "Show.cshtml", 
            "Show-Query.cshtml", 
            "Show");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Tickets" / "TicketShowTest.cs", 
            "Show-QueryTest", 
            "Show");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Tickets" / "TicketShowPageTest.cs",
            "Show-QueryPageTest", 
            "Show");
    }
}