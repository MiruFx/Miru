using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Miru.Userfy
{
    public static class ClaimExtensions
    {
        public static string ByType(this IEnumerable<Claim> claims, string claimType)
        {
            return claims.FirstOrDefault(x => x.Type == claimType)?.Value;
        }
    }
}