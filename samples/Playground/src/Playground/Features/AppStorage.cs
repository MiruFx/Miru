using Miru.Core;
using Miru.Storages;

namespace Playground.Features;

public class AppStorage : LocalDiskStorage, IAppStorage
{
    public AppStorage(MiruSolution solution) : base(solution)
    {
        Root = "app";
    }
}