using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.HtmlConfigs.Core;

public interface ITagConventions
{
    IConventionAction Always { get; }
    
    IConventionAction If(Func<ElementRequest, bool> condition);
    
    IConventionAction If(Func<TagHelperOutput, ElementRequest, bool> condition);
}