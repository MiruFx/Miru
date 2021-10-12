using System.Threading.Tasks;

namespace Miru.Security
{
    public interface IQueueAuthorizer
    {
        Task<bool> QueueAuthorizedAsync();
    }
}