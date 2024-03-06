using Miru.Storages;

namespace Miru.Testing;

public class AppTestStorage : LocalDiskStorage, IAppStorage
{
    public AppTestStorage(MiruSolution solution) : base(solution)
    {
        Root = solution.StorageDir / "tests" / "app";
    }
}