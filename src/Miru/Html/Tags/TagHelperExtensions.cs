using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

public static class TagHelperExtensions
{
    public const string AttributeSeparator = " ";
    
    public static void AddOrIgnore(this TagHelperAttributeList attrs, string attrName, object value)
    {
        if (attrs.ContainsName(attrName))
            return;
            
        attrs.Add(attrName, value);
    }
    
    public static void Append(this TagHelperAttributeList attrs, string attrName, object value)
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
}