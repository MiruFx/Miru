using System.Threading.Tasks;
using Miru.Security;

namespace Miru.Queuing;

public class DefaultQueueAuthorizer : IQueueAuthorizer
{
    public Task<bool> QueueAuthorizedAsync()
    {
        return Task.FromResult(true);
    }
}