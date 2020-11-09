using HtmlTags;

namespace Miru.Html
{
    public static class SelectTagExtensions
    {
        public static SelectTag EmptyOption(this SelectTag htmlTag, string text = "")
        {
            htmlTag.InsertFirst(new HtmlTag("option").Value(string.Empty).Text(text));
            return htmlTag;
        }
    }
}
