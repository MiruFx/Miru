namespace Scripts.StubsExport;

public class ConsolableStubExport : StubExport
{
    public ConsolableStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Consolables" / "SeedConsolable.cs", 
            "Consolable");
    }
}