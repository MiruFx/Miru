using Miru.Html.HtmlConfigs.Core;
using Miru.Urls;

namespace Miru.Html.Tags;

public class TagServices
{
    public TagModifier TagModifier { get; }
    public UrlLookup UrlLookup { get; }
    public ElementNaming ElementNaming { get; }
    public IAntiforgeryAccessor AntiforgeryAccessor { get; }
    
    public TagServices(
        TagModifier tagModifier, 
        UrlLookup urlLookup, 
        ElementNaming elementNaming,
        IAntiforgeryAccessor antiforgeryAccessor)
    {
        TagModifier = tagModifier;
        UrlLookup = urlLookup;
        ElementNaming = elementNaming;
        AntiforgeryAccessor = antiforgeryAccessor;
    }
}