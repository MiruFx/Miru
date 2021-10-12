using System.Threading.Tasks;

namespace Miru.Security
{
    public interface IPermissionAuthorizer<in TPermissions>
    {
        Task<bool> AuthorizedAsync(TPermissions permission);
    }
}