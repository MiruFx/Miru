using System;
using Baseline.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Html.HtmlConfigs.Core;

namespace Miru.Html.Tags;

public abstract class MiruTagHelper : TagHelper
{
    private IServiceProvider _requestServices;
        
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    [HtmlAttributeNotBound]
    public ElementNaming ElementNaming => RequestServices.GetService<ElementNaming>();
    
    [HtmlAttributeNotBound]
    public TagModifier TagModifier => RequestServices.GetService<TagModifier>();
        
    [HtmlAttributeNotBound]
    public IServiceProvider RequestServices
    {
        get => _requestServices ?? ViewContext.HttpContext.RequestServices;
        set => _requestServices = value;
    }
}