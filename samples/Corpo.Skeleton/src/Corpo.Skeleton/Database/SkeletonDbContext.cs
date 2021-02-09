using Corpo.Skeleton.Domain;
using Microsoft.EntityFrameworkCore;
using Miru.Userfy;

namespace Corpo.Skeleton.Database
{
    public class SkeletonDbContext : UserfyDbContext<User, Role>
    {
        public SkeletonDbContext(DbContextOptions options) : base(options)
        {
        }
        
        // Your entities
        public DbSet<Team> Teams { get; set; } 
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be before .UseIdentity
            base.OnModelCreating(builder);
            
            builder.UseIdentity<User, Role>();
        }
    }
}