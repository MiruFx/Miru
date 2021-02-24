using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Userfy;
using Supportreon.Domain;

namespace Supportreon.Database
{
    public class SupportreonDbContext : UserfyDbContext<User>
    {
        public SupportreonDbContext(
            DbContextOptions options,
            IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
        {
        }
        
        public DbSet<Project> Projects { get; set; } 
        public DbSet<Donation> Donations { get; set; } 
        public DbSet<Category> Categories { get; set; }
    }
}
