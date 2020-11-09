using HtmlTags.Conventions;

namespace Miru.Html
{
    public static class HtmlConventionLibraryExtensions
    {
        public static HtmlConventionLibrary AddConvention(this HtmlConventionLibrary library, HtmlConventionRegistry convention)
        {
            convention.Apply(library);
            return library;
        }
    }
}