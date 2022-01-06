namespace Scripts.StubsExport;

public class ConfigStubExport : StubExport
{
    public ConfigStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "appSettings-example.yml", 
            "AppSettings");
    }
}