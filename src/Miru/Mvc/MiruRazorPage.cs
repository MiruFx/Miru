using Microsoft.Extensions.DependencyInjection;
using Miru.Html;
using Miru.Scopables;
using Miru.Userfy;

namespace Miru.Mvc;

public abstract class MiruRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel> 
    where TModel : class
{
    public ICurrentUser CurrentUser => ViewContext.HttpContext.RequestServices.GetService<ICurrentUser>();
        
    public ElementNaming Naming => ViewContext.HttpContext.RequestServices.GetService<ElementNaming>();
}

public abstract class MiruRazorPage<TModel, TCurrent> : MiruRazorPage<TModel> 
    where TModel : class
    where TCurrent : class
{
    public static readonly StubFeature Instance = new();
    
    public TCurrent Current 
    {
        get 
        {
            var currentScope = ViewContext.HttpContext.RequestServices.GetRequiredService<ICurrentAttributes>();

            currentScope.BeforeAsync(Instance, default).GetAwaiter().GetResult();
            
            return ViewContext.HttpContext.RequestServices.GetService<TCurrent>();
        }    
    }
    
    public class StubFeature
    {
    }
}
