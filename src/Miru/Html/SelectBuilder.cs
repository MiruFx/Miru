using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;

namespace Miru.Html;

public class SelectBuilder : IElementBuilder
{
    public HtmlTag Build(ElementRequest request)
    {
        // HtmlTags is not using configured element naming convention for Selects
        // That's way we are doing manually here
        request.ElementId = HtmlConfiguration.ElementNamingConvention
            .GetName(request.Accessor.OwnerType, request.Accessor);
            
        return new SelectTag();
    }
}