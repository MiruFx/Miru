using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

// [HtmlTargetElement(Attributes = "add-class")]
// public class AddClassTagHelper : MiruTagHelper
// {
//     [HtmlAttributeName("add-class")]
//     public string AddClass { get; set; }
//     
//     public override void Process(TagHelperContext context, TagHelperOutput output)
//     {
//         var classes = output.Attributes["class"]?.Value;
//
//         output.Attributes.SetAttribute("class", $"{classes} {AddClass}");
//     }
// }