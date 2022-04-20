using System;
using System.Linq.Expressions;
using HtmlTags;
using Microsoft.Extensions.DependencyInjection;
using Miru.Html;
using Miru.Userfy;

namespace Miru.Mvc;

public abstract class MiruRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel> where TModel : class
{
    public IUserSession UserSession => ViewContext.HttpContext.RequestServices.GetService<IUserSession>();
        
    public ElementNaming Naming => ViewContext.HttpContext.RequestServices.GetService<ElementNaming>();
        
    /// <summary>
    /// Return current model if is type T or create a new T
    /// </summary>
    public object BindOrNew<T>() where T : new()
    {
        return Model is T ? (object) Model : new T();
    }
        
    public HtmlTag s(Expression<Func<TModel, object>> expression)
    {
        return GetHtmlElement().DisplayFor(ViewData.Model, expression);
    }
        
    public HtmlTag s<TOtherModel>(TOtherModel otherModel, Expression<Func<TOtherModel, object>> expression) where TOtherModel : class
    {
        return GetHtmlElement().DisplayFor(otherModel, expression);
    }

    private HtmlGenerator GetHtmlElement()
    {
        return ViewContext.HttpContext.RequestServices.GetService<HtmlGenerator>();
    }   
}