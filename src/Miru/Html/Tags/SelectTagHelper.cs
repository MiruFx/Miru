using System;
using System.Text;
using Ardalis.SmartEnum;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;
using Miru.Mvc;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-select", Attributes = "x")]
[HtmlTargetElement("miru-select", Attributes = "for")]
[HtmlTargetElement("miru-select", Attributes = "model")]
public class SelectTagHelper : MiruTagHelper
{
    private const string SelectedAttr = $" selected=\"selected\"";
    
    [HtmlAttributeName("lookup")]
    public SelectLookups Lookups { get; set; }
    
    // [HtmlAttributeName("empty-option")]
    // public string EmptyOption { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Select;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.SelectFor(ElementRequest.Create(this), output);

        var options = new StringBuilder();

        if (output.Attributes.TryGetAttribute("empty-option", out var emptyOption))
        {
            options.Append($"<option>{emptyOption.Value}</option>");
            output.Attributes.Remove(emptyOption);
        }

        foreach (var lookup in Lookups)
        {
            var selectedAttr = GetSelectedAttr(lookup);

            options.Append($"<option value=\"{lookup.Id}\"{selectedAttr}>{lookup.Description}</option>");
        }

        output.Content.SetHtmlContent(options.ToString());
    }

    private string GetSelectedAttr(Lookup lookup)
    {
        if (For.Model != null)
        {
            if (For.Model is ISmartEnum)
                return lookup.Id.Equals(For.Model.GetPropertyValue("Value").ToString())
                    ? SelectedAttr
                    : string.Empty;

            if (For.Model is Enum @enum)
                return lookup.Id.Equals(@enum.ToInt().ToString())
                    ? SelectedAttr
                    : string.Empty;

            return lookup.Id.Equals(For.Model.ToString()) ? SelectedAttr : string.Empty;
        }

        return string.Empty;
    }
}