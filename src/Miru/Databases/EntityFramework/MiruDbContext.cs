using Microsoft.EntityFrameworkCore;

namespace Miru.Databases.EntityFramework
{
    public abstract class MiruDbContext : DbContext
    {
        protected MiruDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}