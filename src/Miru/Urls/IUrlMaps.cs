using Microsoft.AspNetCore.Routing;

namespace Miru.Urls
{
    public interface IUrlMaps
    {
        string UrlFor<TInput>(TInput request, RouteValueDictionary queryString) where TInput : class;
    }
}