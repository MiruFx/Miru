namespace Scripts.StubsExport;

public class JobStubExport : StubExport
{
    public JobStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamCreated.cs", 
            "Job", 
            templateKey: "Job");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamCreatedTest.cs", 
            "JobTest", 
            templateKey: "Job");
    }
}