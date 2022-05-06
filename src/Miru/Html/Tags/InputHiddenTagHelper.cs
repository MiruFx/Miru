using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-hidden", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
public class InputHiddenTagHelper : MiruHtmlTagHelper
{
    protected override string Category => "InputHidden";
}