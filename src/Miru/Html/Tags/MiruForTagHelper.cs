using Baseline.Reflection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

public abstract class MiruForTagHelper : MiruTagHelper
{
    public const string ForAttributeName = "for";
    public const string ModelAttributeName = "model";
    
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression For { get; set; }

    [HtmlAttributeName(ModelAttributeName)]
    public object Model { get; set; }
    
    [HtmlAttributeName("x")]
    public Accessor ExFor { get; set; }
}