using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Miru.Html
{
    public class AntiForgeryAccessor : IAntiforgeryAccessor
    {
        public string RequestToken { get; }
        
        public string FormFieldName { get; }
        
        public bool HasToken { get; }
        
        public AntiForgeryAccessor(IHttpContextAccessor httpContextAccessor, IAntiforgery antiForgery)
        {
            if (httpContextAccessor.HttpContext != null)
            {
                // antiForgery.SetCookieTokenAndHeader();
                
                var token = antiForgery.GetAndStoreTokens(httpContextAccessor.HttpContext);

                RequestToken = token.RequestToken;
                
                FormFieldName = token.FormFieldName;
                
                HasToken = true;
            }
        }
    }
}