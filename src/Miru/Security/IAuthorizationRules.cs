using System.Threading.Tasks;

namespace Miru.Security
{
    public interface IAuthorizationRules
    {
        Task<AuthorizationResult> Evaluate<TRequest>(TRequest request, FeatureInfo feature);
    }
}