using Bogus;
using Microsoft.AspNetCore.Identity;
using Miru.Fabrication;
using Miru.Foundation.Logging;
using Miru.Userfy;

namespace Miru.Testing
{
    public static class UserfyTestFixtureExtensions
    {
        public static async Task<TUser> MakeUserAsync<TUser>(
            this ITestFixture fixture, 
            string password = "123456", 
            Action<TUser> customizations = null) 
                where TUser : UserfyUser
        {
            using var scope = fixture.Get<IMiruApp>().WithScope();
            
            var userManager = scope.Get<UserManager<TUser>>();
            
            var user = scope.Get<Fabricator>().Make(customizations);

            user.UserName = user.Email;
            user.NormalizedUserName = user.NormalizedEmail;
            
            await userManager.CreateAsync(user, password);

            return user;
        }
    }
}