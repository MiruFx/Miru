using Microsoft.AspNetCore.Routing;

namespace Miru.Urls
{
    public class DefaultUrlMaps : IUrlMaps
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly MiruRoutingDiscoverer _routings;

        public DefaultUrlMaps(LinkGenerator linkGenerator, MiruRoutingDiscoverer routings)
        {
            _linkGenerator = linkGenerator;
            _routings = routings;
        }

        public string UrlFor<TInput>(TInput request, RouteValueDictionary queryString) where TInput : class
        {
            ModelToUrlMap map;
            
            if (_routings.Mappings.TryGetValue(request.GetType() + "+Query", out map))
                return _linkGenerator.GetPathByAction(map.ActionName, map.ControllerName, queryString);
            
            if (_routings.Mappings.TryGetValue(request.GetType() + "+Command", out map))
                return _linkGenerator.GetPathByAction(map.ActionName, map.ControllerName, queryString);
            
            var path = _linkGenerator.GetPathByName(request.GetType().ToString(), queryString);

            return path;
        }
    }
}