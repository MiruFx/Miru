using Miru.Core;
using Miru.Storages;

namespace Miru.Html;

public interface IAssetsStorage : IStorage
{
}

public class AssetsStorage : LocalDiskStorage, IAssetsStorage
{
    public AssetsStorage(MiruSolution solution) : base(solution)
    {
        Root = StorageDir / "assets";
    }
}