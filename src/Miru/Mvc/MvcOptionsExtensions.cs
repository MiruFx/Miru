using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Mvc
{
    public static class MvcOptionsExtensions
    {
        public static IMvcCoreBuilder AddMiruNestedControllerUnder<TTop>(this IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.ConfigureApplicationPartManager(m =>
            {
                m.FeatureProviders.Add(new MiruControllerFeatureProvider(typeof(TTop)));
            });
            
            return mvcCoreBuilder;
        }

        public static IMvcCoreBuilder AddMiruNestedControllers(this IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.ConfigureApplicationPartManager(m =>
            {
                m.FeatureProviders.Add(new MiruControllerFeatureProvider());
            });
            
            return mvcCoreBuilder;
        }
        
        public static IMvcCoreBuilder AddMiruActionResult(this IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.Services.Configure<MvcOptions>(_ =>
            {
                _.RespectBrowserAcceptHeader = true;

                _.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            
            mvcCoreBuilder.Services.AddSingleton<IActionResultExecutor<ObjectResult>, MiruObjectResultExecutor>();
            
            return mvcCoreBuilder;
        }
    }
}