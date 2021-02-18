using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Miru.Fabrication;
using Miru.Foundation.Logging;
using Miru.Userfy;

namespace Miru.Testing
{
    public static class UserfyTestFixtureExtensions
    {
        public static TUser MakeUser<TUser>(
            this ITestFixture fixture, 
            string password, 
            Action<TUser> customizations = null) 
                where TUser : UserfyUser
        {
            using var scope = fixture.Get<IMiruApp>().WithScope();
            
            var userManager = scope.Get<UserManager<TUser>>();
            
            var user = scope.Get<Fabricator>().Make(customizations);

            userManager.CreateAsync(user, password);

            return user;
        }
    }
}