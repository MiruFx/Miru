using System.Collections.Generic;
using Pantanal.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Userfy;

namespace Pantanal.Database
{
    public class PantanalDbContext : UserfyDbContext<User>
    {
        public PantanalDbContext(
            DbContextOptions options,
            IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
        {
        }
        
        // Your entities
        // public DbSet<>  { get; set; } 
        
    }
}
