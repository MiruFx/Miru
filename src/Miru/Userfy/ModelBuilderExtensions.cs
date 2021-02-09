using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Miru.Userfy
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder UseIdentity<TUser, TRole>(this ModelBuilder builder)
            where TUser : IdentityUser<long>
            where TRole : IdentityRole<long>
        {
            builder.Entity<TUser>(x => x.ToTable(name: "Users"));
            builder.Entity<TRole>(x => x.ToTable(name: "Roles"));
            
            builder.Entity<IdentityUserRole<long>>(x => x.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<long>>(x => x.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<long>>(x => x.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<long>>(x => x.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<long>>(x => x.ToTable("UserTokens"));

            return builder;
        }
    }
}