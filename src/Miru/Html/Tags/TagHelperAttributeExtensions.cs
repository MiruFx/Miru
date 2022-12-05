using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

public static class TagHelperAttributeExtensions
{
    public const string AttributeSeparator = " ";
    
    public static bool ValueIsEqual(this TagHelperAttribute attr, string compareTo)
    {
        if (attr.Value is HtmlString htmlString)
            return htmlString.ToString().Equals(compareTo);

        return attr.Value.Equals(compareTo);
    }
    
    public static void AppendAttr(this TagHelperAttributeList attrs, string attrName, object value)
    {
        var attr = attrs[attrName];
        
        if (attr == null)
        {
            attrs.SetAttribute(attrName, value);
            return;
        }
        
        var currentValue = attr.Value;
        attrs.SetAttribute(attrName, currentValue + AttributeSeparator + value);
    }
    
    public static void PrependAttr(this TagHelperAttributeList attrs, string attrName, object value)
    {
        var attr = attrs[attrName];
        
        if (attr == null)
        {
            attrs.SetAttribute(attrName, value);
            return;
        }
        
        var currentValue = attr.Value;
        attrs.SetAttribute(attrName, value + AttributeSeparator + currentValue);
    }
    
    public static string GetValue(this TagHelperAttributeList list, string attributeName)
    {
        if (list.TryGetAttribute(attributeName, out var value) && value.Value is not null)
            return value.Value.ToString();

        return null;
    }

    public static bool IsTypeRadio(this TagHelperAttributeList attrs) =>
        attrs.GetValue(HtmlAttr.Type)?.Equals(HtmlAttr.Radio) ?? false;

    public static bool IsTypeCheckbox(this TagHelperAttributeList attrs) =>
        attrs.GetValue(HtmlAttr.Type)?.Equals(HtmlAttr.Checkbox) ?? false;
}