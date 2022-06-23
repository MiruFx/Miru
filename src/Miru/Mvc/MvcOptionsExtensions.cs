using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Mvc;

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
        
    // public static void UseEnumerationModelBinding(this MvcOptions opts)
    // {
    //     var binderToFind = opts.ModelBinderProviders
    //         .FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));
    //
    //     if (binderToFind == null) 
    //         return;
    //
    //     var index = opts.ModelBinderProviders.IndexOf(binderToFind);
    //         
    //     opts.ModelBinderProviders.Insert(index, new EnumerationModelBinderProvider());
    // }
}