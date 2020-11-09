using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Routing.Template;
using Miru.Urls;

namespace Miru.Testing
{
    public class StubUrlMaps : IUrlMaps
    {
        private readonly TemplateBinderFactory _factory;

        public StubUrlMaps(TemplateBinderFactory factory)
        {
            _factory = factory;
        }

        public string UrlFor<TInput>(TInput request, RouteValueDictionary queryString) where TInput : class
        {
            var path = typeof(TInput).FeatureName();
            
            var binder = _factory.Create(RoutePatternFactory.Parse(path));
            
            var qs = binder.BindValues(queryString);

            return qs;
        }
    }
}