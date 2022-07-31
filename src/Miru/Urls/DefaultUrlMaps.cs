using Microsoft.AspNetCore.Routing;

namespace Miru.Urls;

public class DefaultUrlMaps : IUrlMaps
{
    private readonly LinkGenerator _linkGenerator;
    private readonly MiruRoutingDiscoverer _routings;
    private readonly UrlMapsScanner _urlMapsScanner;

    public DefaultUrlMaps(LinkGenerator linkGenerator, MiruRoutingDiscoverer routings, UrlMapsScanner urlMapsScanner)
    {
        _linkGenerator = linkGenerator;
        _routings = routings;
        _urlMapsScanner = urlMapsScanner;
    }

    public string UrlFor<TInput>(TInput request, RouteValueDictionary queryString) where TInput : class
    {
        if (_routings.Mappings.Count == 0)
            _urlMapsScanner.Scan();
            
        if (_routings.Mappings.TryGetValue($"{request.GetType()}+Query", out var map))
            return _linkGenerator.GetPathByAction(map.ActionName, map.ControllerName, queryString);
            
        if (_routings.Mappings.TryGetValue($"{request.GetType()}+Command", out map))
            return _linkGenerator.GetPathByAction(map.ActionName, map.ControllerName, queryString);
            
        var path = _linkGenerator.GetPathByName(request.GetType().ToString(), queryString);

        return path;
    }
}