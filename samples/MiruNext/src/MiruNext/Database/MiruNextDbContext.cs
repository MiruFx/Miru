using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Userfy;
using MiruNext.Domain;

namespace MiruNext.Database;

public class MiruNextDbContext : UserfyDbContext<User>
{
    public MiruNextDbContext(
        DbContextOptions options,
        IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
    {
    }
        
    // Your entities
    // public DbSet<>  { get; set; } 
        
}