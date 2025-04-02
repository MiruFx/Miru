using System.Linq.Expressions;
using Baseline.Reflection;
using Miru.Currentable;
using Miru.Html;
using Miru.Urls;
using Miru.Userfy;

namespace Miru.Mvc;

public abstract class MiruRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel> 
    where TModel : class
{
    // TODO: move to miru?
    public string UrlFor<T>(T request) where T : class
    {
        var url = ViewContext.HttpContext.RequestServices.GetRequiredService<UrlLookup>();
                
        return url.For(request);
    }
    
    public ICurrentUser CurrentUser => 
        ViewContext.HttpContext.RequestServices.GetService<ICurrentUser>();
        
    public ElementNaming Naming => 
        ViewContext.HttpContext.RequestServices.GetService<ElementNaming>();
    
    public Accessor For(Expression<Func<TModel, object>> expression) => 
        ReflectionHelper.GetAccessor(expression);
    
    public string CurrentUrl => $"{Context.Request.Path}{Context.Request.QueryString}";
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
            var currentScope = ViewContext.HttpContext.RequestServices.GetRequiredService<ICurrentHandler>();

            currentScope.Handle(Instance, default).GetAwaiter().GetResult();
            
            return ViewContext.HttpContext.RequestServices.GetService<TCurrent>();
        }    
    }
    
    public class StubFeature
    {
    }
}
