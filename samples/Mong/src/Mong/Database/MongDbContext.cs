using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Userfy;
using Mong.Domain;

namespace Mong.Database
{
    public class MongDbContext : UserfyDbContext<User, Role>
    {
        public MongDbContext(
            DbContextOptions<MongDbContext> options, 
            IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
        {
        }
        
        public DbSet<Topup> Topups { get; set; }
        public DbSet<Provider> Providers { get; set; }
    }
}