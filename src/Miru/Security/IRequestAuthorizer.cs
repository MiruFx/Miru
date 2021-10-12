using System.Threading.Tasks;

namespace Miru.Security
{
    public interface IRequestAuthorizer
    {
        Task<bool> AuthorizedAsync<TRequest>(TRequest request);
    }
}