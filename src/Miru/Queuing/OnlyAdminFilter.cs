using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using Miru.Userfy;

namespace Miru.Queuing
{
    public class OnlyAdminFilter<TUser> : IDashboardAuthorizationFilter where TUser : ICanBeAdmin
    {
        public bool Authorize(DashboardContext context)
        {
            var userSession = context.GetHttpContext().RequestServices.GetService<IUserSession<TUser>>();

            if (userSession.IsAnonymous)
                return false;
                
            return userSession.User().GetAwaiter().GetResult().IsAdmin;
        }
    }
}