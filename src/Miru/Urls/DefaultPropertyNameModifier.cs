using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Urls;

public class DefaultPropertyNameModifier
{
    public virtual string GetModifiedPropertyName(PropertyInfo propertyInfo, Attribute[] attributes)
    {
        var fromQueryAttributes = attributes.OfType<FromQueryAttribute>().ToArray();
            
        if (fromQueryAttributes.Length > 0)
        {
            return fromQueryAttributes[0].Name;
        }

        return propertyInfo.Name;
    }
}