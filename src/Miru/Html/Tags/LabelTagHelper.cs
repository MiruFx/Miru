using System;
using Baseline;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-label", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class LabelTagHelper : MiruHtmlTagHelper
    {
        protected override string Category { get; } = ElementConstants.Label;

        [HtmlAttributeName("set-class")]
        protected string SetClass { get; set; }
    }
}