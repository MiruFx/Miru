using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    public static class TagHelperExtensions
    {
        public static void AddOrIgnore(this TagHelperAttributeList attrs, string name, object value)
        {
            if (attrs.ContainsName(name))
                return;
            
            attrs.Add(name, value);
        }
    }
}