using Microsoft.AspNetCore.Http;

namespace Miru.Mvc
{
    public class HttpSessionStore : ISessionStore
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpSessionStore(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetString(string key)
        {
            return _accessor.HttpContext.Session.GetString(key);
        }

        public void SetString(string key, string value)
        {
            _accessor.HttpContext.Session.SetString(key, value);
        }
    }
}