using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Miru.Databases.EntityFramework;

public abstract class MiruDbContext : DbContext
{
    public MiruDbContext(IMiruApp app) : base(app.Get<DbContextOptions>())
    {
        QueryFilterManager.InitilizeGlobalFilter(this);
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}