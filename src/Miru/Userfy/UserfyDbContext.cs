using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Miru.Userfy
{
    public abstract class UserfyDbContext<TUser> : UserfyDbContext<TUser, IdentityRole<long>> 
        where TUser : IdentityUser<long> 
    {
        // FIXME: Create a type that holds all dependencies.
        // If a feature is added to dbcontext, will not break applications
        public UserfyDbContext(DbContextOptions options, IEnumerable<IInterceptor> interceptors)
            : base(options, interceptors)
        {
        }
    }
    
    public abstract class UserfyDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, long> 
        where TUser : IdentityUser<long> 
        where TRole : IdentityRole<long>
    {
        private readonly IEnumerable<IInterceptor> _interceptors;

        public UserfyDbContext(DbContextOptions options, IEnumerable<IInterceptor> interceptors)
            : base(options)
        {
            _interceptors = interceptors;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_interceptors);
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseIdentity<TUser, TRole>();
        }
    }
}