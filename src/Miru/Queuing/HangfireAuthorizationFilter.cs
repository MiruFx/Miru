using System.Threading.Tasks;
using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using Miru.Security;

namespace Miru.Queuing
{
    public class HangfireAuthorizationFilter : IDashboardAsyncAuthorizationFilter
    {
        public async Task<bool> AuthorizeAsync(DashboardContext context)
        {
            var services = context.GetHttpContext().RequestServices;

            // TODO: throw friendlier exception to a class implement IQueueAuthorizer
            var authorizer = services.GetRequiredService<IQueueAuthorizer>();

            return await authorizer.QueueAuthorizedAsync();
        }
    }
}