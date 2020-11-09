using System.Threading.Tasks;

namespace Miru.Security
{
    public interface IAuthorizer<TRequest>
    {
        Task<bool> HasAuthorization(TRequest message);
    }
}