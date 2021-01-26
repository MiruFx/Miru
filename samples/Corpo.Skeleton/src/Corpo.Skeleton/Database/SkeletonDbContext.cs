using System.Collections.Generic;
using Corpo.Skeleton.Domain;
using Microsoft.EntityFrameworkCore;
using Miru.Databases.EntityFramework;

namespace Corpo.Skeleton.Database
{
    public class SkeletonDbContext : MiruDbContext
    {
        public SkeletonDbContext(DbContextOptions options, IEnumerable<IBeforeSaveHandler> handlers) : base(options, handlers)
        {
        }
        
        public DbSet<User> Users { get; set; } 
        
        // Your entities
        public DbSet<Product> Products { get; set; } 
        public DbSet<Category> Categories { get; set; } 
    }
}