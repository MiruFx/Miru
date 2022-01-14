using System;

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

        Func<string, string> tokens = s => s
            .Replace("List(Query request)", "{{ input.Action }}(Query request)")
            .Replace("Done", "{{ input.Action }}")
            .Replace("done", "{{ string.downcase input.Action }}");
        
        ExportFile(featureDir / "TicketDone.cs", "List-Query", "List", tokens: tokens);
        ExportFile(featureDir / "Done.cshtml", "List-Query.cshtml", "List", tokens: tokens);
            
        ExportFile(testDir / "TicketDoneTest.cs", "List-QueryTest", "List", tokens: tokens);
        ExportFile(pageTestDir / "TicketDonePageTest.cs", "List-QueryPageTest", "List", tokens: tokens);
    }
}