namespace Miru.Urls
{
    public class UrlLookup
    {
        private readonly IUrlMaps _urlMaps;
        private readonly UrlOptions _urlOptions;

        public UrlLookup(
            UrlOptions urlOptions, 
            IUrlMaps urlMaps)
        {
            _urlOptions = urlOptions;
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
    }
}