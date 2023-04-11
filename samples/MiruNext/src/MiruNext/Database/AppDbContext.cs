using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Userfy;
using MiruNext.Domain;

namespace MiruNext.Database;

public class AppDbContext : UserfyDbContext<User>
{
    public AppDbContext(
        DbContextOptions options,
        IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
    {
    }
        
    public DbSet<Todo> Todos { get; set; }
}