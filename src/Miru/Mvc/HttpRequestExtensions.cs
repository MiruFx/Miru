using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;

namespace Miru.Mvc
{
    public static class HttpRequestExtensions
    {
        private static readonly string XRequestedWith = "X-Requested-With";
        private static readonly string XmlHttpRequest = "XMLHttpRequest";
        
        public static bool IsGet(this HttpRequest request) => request.Method == HttpMethods.Get;

        public static bool IsAjax(this HttpRequest request)
        {
            if (request.Headers != null)
            {
                return request.Headers[XRequestedWith] == XmlHttpRequest;
            }

            return false;
        }
        
        

        public static bool IsPost(this HttpRequest request) => request.Method.Equals(HttpMethods.Post);

        public static bool AcceptsHtml(this HttpRequest request) => request.CanAccept("text/html");
        
        public static bool IsInternetExplorer(this HttpRequest request)
        {
            return request.Headers["User-Agent"].Any(s => s.Contains("Trident")) ||
                   request.Headers["User-Agent"].Any(s => s.Contains("MSIE"));
        }
    }
}
