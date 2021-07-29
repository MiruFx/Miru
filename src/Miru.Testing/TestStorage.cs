using Miru.Core;
using Miru.Storages;

namespace Miru.Testing
{
    public class TestStorage : LocalDiskStorage
    {
        public TestStorage(MiruSolution solution) : base(solution)
        {
            StorageDir = solution.StorageDir / "tests";
        }
    }
}