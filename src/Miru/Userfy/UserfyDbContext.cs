using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SmartEnum.EFCore;
using Z.EntityFramework.Plus;

namespace Miru.Userfy;

public abstract class UserfyDbContext<TUser> : UserfyDbContext<TUser, IdentityRole<long>> 
    where TUser : IdentityUser<long> 
{
    // FIXME: Create a type that holds all dependencies.
    // If a feature is added to dbcontext, will not break applications
    public UserfyDbContext(DbContextOptions options, IEnumerable<IInterceptor> interceptors)
        : base(options, interceptors)
    {
        foreach (var interceptor in interceptors)
        {
            if (interceptor is QueryFiltersInterceptor queryFiltersInterceptor)
            {
                foreach (var queryFilter in queryFiltersInterceptor.Filters)
                {
                    queryFilter.Apply(this);
                }
            }
        }
    }
}
    
public abstract class UserfyDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, long> 
    where TUser : IdentityUser<long> 
    where TRole : IdentityRole<long>
{
    protected IEnumerable<IInterceptor> Interceptors { get; }

    public UserfyDbContext(DbContextOptions options, IEnumerable<IInterceptor> interceptors)
        : base(options)
    {
        Interceptors = interceptors;
        
        QueryFilterManager.InitilizeGlobalFilter(this);
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(Interceptors);
    }
        
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.UseIdentity<TUser, TRole>();
    }
}