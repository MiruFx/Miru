using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Miru.Domain;

namespace Miru.Userfy;

public static class IdentityErrorExtensions
{
    public static DomainException ThrowDomainException(this IEnumerable<IdentityError> error)
    {
        throw new DomainException(error.Select(x => x.Description).Join(". "));
    }
    
    public static void ThrowDomainExceptionIfFailed(this IdentityResult result)
    {
        if (result.Succeeded == false)
            result.Errors.ThrowDomainException();
    }
    
    public static bool IsEmailTaken(this IdentityResult result) => 
        result.Errors.Any(x => x.Code == "DuplicateUserName");
}