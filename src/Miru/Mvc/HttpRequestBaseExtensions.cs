using Microsoft.AspNetCore.Http;

namespace Miru.Mvc
{
    public static class HttpRequestBaseExtensions
    {
        public static bool CanAccept(this HttpRequest request, string types)
        {
            var accept = request.Headers["Accept"];
            
            return string.IsNullOrEmpty(accept) == false && accept.ToString().Contains(types);
        }
    }
}