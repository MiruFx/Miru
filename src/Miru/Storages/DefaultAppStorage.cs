using Miru.Core;

namespace Miru.Storages;

public class DefaultAppStorage : LocalDiskStorage, IAppStorage
{
    public DefaultAppStorage(MiruSolution solution) : base(solution)
    {
        Root = StorageDir / "app";
    }
}