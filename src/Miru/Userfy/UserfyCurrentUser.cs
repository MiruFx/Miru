using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Miru.Userfy
{
    public class UserfyCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserfyCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long Id => 
            _httpContextAccessor.HttpContext?.User.Claims.ByType(ClaimTypes.NameIdentifier).ToLong() ?? 0;
        
        public string Display => _httpContextAccessor.HttpContext?.User.Identity?.Name;

        public bool IsLogged => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        
        public bool IsAnonymous => IsLogged == false;
    }
}