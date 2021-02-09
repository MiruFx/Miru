using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Miru.Userfy
{
    public abstract class UserfyDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, long> 
        where TUser : IdentityUser<long> 
        where TRole : IdentityRole<long>
    {
        public UserfyDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}