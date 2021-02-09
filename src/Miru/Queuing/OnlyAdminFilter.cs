using Hangfire.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Miru.Userfy;

namespace Miru.Queuing
{
    public class OnlyAdminFilter<TUser> : IDashboardAuthorizationFilter where TUser : UserfyUser
    {
        public bool Authorize(DashboardContext context)
        {
            var userSession = context.GetHttpContext().RequestServices.GetService<IUserSession<TUser>>();

            if (userSession.IsAnonymous)
                return false;
                
            // return userSession.GetUserAsync().GetAwaiter().GetResult().IsAdmin;
            return true;
        }
    }
}