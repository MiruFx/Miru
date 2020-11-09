using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Databases.EntityFramework;
using Miru.Domain;
using Miru.Userfy;
using SelfImprov.Domain;
using Z.EntityFramework.Plus;

namespace SelfImprov.Database
{
    public class SelfImprovDbContext : MiruDbContext
    {
        public SelfImprovDbContext(
            DbContextOptions options, 
            IEnumerable<IBeforeSaveHandler> handlers, 
            IUserSession userSession) : 
                base(options, handlers)
        {
            // TODO: Add filters through Startup.cs
            
            this.Filter<IBelongsToUser>(
                q => q.Where(x => x.UserId == userSession.CurrentUserId));
            
            this.Filter<IInactivable>(
                q => q.Where(x => x.IsInactive == false));
        }
        
        public DbSet<User> Users { get; set; }    
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Iteration> Iterations { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
    }
}
