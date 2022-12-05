using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.HtmlConfigs.Core;

public class Modifier
{
    public Func<TagHelperOutput, ElementRequest, bool> Condition { get; }
    public Action<TagHelperOutput, ElementRequest> Modification { get; }
    
    public Modifier(
        Func<TagHelperOutput, ElementRequest, bool> condition, 
        Action<TagHelperOutput, ElementRequest> modification)
    {
        Condition = condition;
        Modification = modification;
    }
}