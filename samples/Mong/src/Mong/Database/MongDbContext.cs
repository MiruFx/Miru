using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Miru.Databases.EntityFramework;
using Mong.Domain;

namespace Mong.Database
{
    public class MongDbContext : MiruDbContext
    {
        public MongDbContext(
            DbContextOptions<MongDbContext> options,
            IEnumerable<IBeforeSaveHandler> preSaveHandlers) : base(options, preSaveHandlers)
        {
        }
        
        public DbSet<Topup> Topups { get; set; }
        public DbSet<User> Users { get; set; } 
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}