using Microsoft.AspNetCore.Mvc.Rendering;

namespace Miru.Html.HtmlConfigs;

public static class IdAndNameExtensions
{
    public static string InputIdFromName(this ElementNaming naming, string nameAttr) => 
        TagBuilder.CreateSanitizedId(nameAttr, HtmlAttr.InputIdInvalidChar);

    public static string InputId(this ElementNaming naming, ElementRequest elementRequest)
    {
        if (elementRequest.Accessor is not null)
            return naming.InputIdFromName(elementRequest.Name);

        if (elementRequest.Value is not null)
            return naming.Id(elementRequest.Value);

        return string.Empty;
    }
    
    public static string InputName(this ElementNaming naming, ElementRequest elementRequest) => 
        elementRequest.Name;
}