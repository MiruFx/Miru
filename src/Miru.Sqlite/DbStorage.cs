using Miru.Core;
using Miru.Storages;

namespace Miru.Sqlite;

public class DbStorage : LocalDiskStorage
{
    public DbStorage(MiruSolution solution) : base(solution)
    {
        Root = solution.StorageDir / "db";
    }
}