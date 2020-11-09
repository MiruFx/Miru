using System.Threading.Tasks;

namespace Miru.Security
{
    public class Authorizer
    {
        private readonly IAuthorizationRules _rules;

        public Authorizer(IAuthorizationRules rules)
        {
            _rules = rules;
        }

        public async Task<bool> Can<TRequest>() where TRequest : new()
        {
            var request = new TRequest();
            
            var result = await _rules.Evaluate(request, FeatureInfo.For(request));
            
            return result.IsAuthorized;
        }
    }
}