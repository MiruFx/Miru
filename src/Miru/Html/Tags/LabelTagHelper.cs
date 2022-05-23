using System;
using Baseline;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-label", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
public class LabelTagHelper : MiruForTagHelper
{
    protected override string Category => ElementConstants.Label;
}