namespace Miru.Urls;

public class UrlLookup
{
    private readonly IUrlMaps _urlMaps;
    private readonly UrlOptions _urlOptions;

    public UrlLookup(
        IOptions<UrlOptions> urlOptions, 
        IUrlMaps urlMaps)
    {
        _urlOptions = urlOptions.Value;
        _urlMaps = urlMaps;
    }

    public string For<TRequest>() where TRequest : class, new()
    {
        return new UrlBuilder<TRequest>(new TRequest(), _urlOptions, _urlMaps);
    }
        
    public string For<TRequest>(TRequest request) where TRequest : class
    {
        return new UrlBuilder<TRequest>(request, _urlOptions, _urlMaps);
    }
        
    public UrlBuilder<TRequest> Build<TRequest>() where TRequest : class, new()
    {
        return new UrlBuilder<TRequest>(new TRequest(), _urlOptions, _urlMaps);
    }
        
    public UrlBuilder<TRequest> Build<TRequest>(TRequest request) where TRequest : class
    {
        return new UrlBuilder<TRequest>(request, _urlOptions, _urlMaps);
    }
        
    public string FullFor<TRequest>() where TRequest : class, new()
    {
        var baseUrl = _urlOptions.Base;
            
        if (baseUrl.EndsWith('/'))
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            
        return $"{baseUrl}{new UrlBuilder<TRequest>(new TRequest(), _urlOptions, _urlMaps)}";
    }
        
    public string FullFor<TRequest>(TRequest request) where TRequest : class
    {
        var baseUrl = _urlOptions.Base;

        if (baseUrl.NotEmpty() && baseUrl.EndsWith('/'))
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            
        return $"{baseUrl}{new UrlBuilder<TRequest>(request, _urlOptions, _urlMaps)}";
    }
}