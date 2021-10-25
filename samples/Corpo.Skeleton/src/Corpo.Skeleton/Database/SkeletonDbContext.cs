using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Userfy;

namespace Corpo.Skeleton.Database;

public class SkeletonDbContext : UserfyDbContext<User>
{
    public SkeletonDbContext(
        DbContextOptions options,
        IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
    {
    }
        
    // Your entities
    public DbSet<Team> Teams { get; set; } 
    public DbSet<Category> Categories { get; set; }
}