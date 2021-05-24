using Miru.Core;
using Miru.Storages;

namespace Miru.Testing
{
    public class TestStorage : Storage
    {
        public TestStorage(MiruSolution solution) : base(solution)
        {
            StorageDir = solution.StorageDir / "tests";
        }
    }
}