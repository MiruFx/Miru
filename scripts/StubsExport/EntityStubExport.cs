namespace Scripts.StubsExport;

public class EntityStubExport : StubExport
{
    public EntityStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Domain" / "Team.cs", 
            "Entity");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Domain" / "TeamTest.cs", 
            "EntityTest");
    }
}