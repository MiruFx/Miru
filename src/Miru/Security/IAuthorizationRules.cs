using System.Threading.Tasks;

namespace Miru.Security;

public interface IAuthorizationRules
{
    AuthorizationResult Evaluate<TRequest>(TRequest request, FeatureInfo feature);
}