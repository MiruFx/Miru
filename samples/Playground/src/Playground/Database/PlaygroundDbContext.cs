using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Userfy;
using Playground.Domain;

namespace Playground.Database
{
    public class PlaygroundDbContext : UserfyDbContext<User>
    {
        public PlaygroundDbContext(
            DbContextOptions options,
            IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
        {
        }
        
        // Your entities
        // public DbSet<>  { get; set; } 
        
    }
}
