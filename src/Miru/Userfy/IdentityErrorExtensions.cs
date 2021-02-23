using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Miru.Domain;

namespace Miru.Userfy
{
    public static class IdentityErrorExtensions
    {
        public static DomainException ToDomainException(this IEnumerable<IdentityError> error)
        {
            throw new DomainException(error.Select(x => x.Description).Join(". "));
        }
    }
}