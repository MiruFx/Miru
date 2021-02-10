using Microsoft.EntityFrameworkCore;
using Miru.Userfy;
using Mong.Domain;

namespace Mong.Database
{
    public class MongDbContext : UserfyDbContext<User, Role>
    {
        public MongDbContext(DbContextOptions<MongDbContext> options) : base(options)
        {
        }
        
        public DbSet<Topup> Topups { get; set; }
        public DbSet<Provider> Providers { get; set; }
    }
}