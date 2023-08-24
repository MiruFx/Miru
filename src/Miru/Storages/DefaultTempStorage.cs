namespace Miru.Storages;

public class DefaultTempStorage : LocalDiskStorage, ITempStorage
{
    public DefaultTempStorage(MiruSolution solution) : base(solution)
    {
        Root = StorageDir / "temp";
    }
}