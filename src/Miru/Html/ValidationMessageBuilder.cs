using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using HtmlTags.Conventions.Elements.Builders;

namespace Miru.Html
{
    public class ValidationMessageBuilder : IElementBuilder
    {
        public HtmlTag Build(ElementRequest request)
        {
            // HtmlTags is not using configured element naming convention for Selects
            // That's way we are doing manually here
            request.ElementId = HtmlConfiguration.ElementNamingConvention
                .GetName(request.Accessor.OwnerType, request.Accessor);

            var id = DefaultIdBuilder.Build(request);
            
            return new HtmlTag("div")
                .Attr("hidden", "hidden")
                .Data("for", id)
                .Id($"{id}-validation")
                .Attr("name", request.ElementId);
        }
    }
}