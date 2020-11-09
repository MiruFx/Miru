using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Databases.EntityFramework;
using Skeleton.Domain;

namespace Skeleton.Database
{
    public class SkeletonDbContext : MiruDbContext
    {
        public SkeletonDbContext(DbContextOptions options, IEnumerable<IBeforeSaveHandler> handlers) : base(options, handlers)
        {
        }
        
        public DbSet<User> Users { get; set; } 
        
        // Your entities
        public DbSet<Product> Products { get; set; } 
    }
}