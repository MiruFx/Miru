using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-input", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("mi", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class InputTagHelper : HtmlTagTagHelper
    {
        protected override string Category { get; } = ElementConstants.Editor;
    }
}