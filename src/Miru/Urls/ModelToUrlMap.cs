using System.Net.Http;

namespace Miru.Urls
{
    public class ModelToUrlMap
    {
        public HttpMethod Method { get; set; }
        
        public string ActionName { get; set; }
        
        public string ControllerName { get; set; }
    }
}