using HtmlTags;

namespace Miru.Mvc
{
    public class ValidationMessageTag : HtmlTag
    {
        public ValidationMessageTag(string id, string inputId, string errorMessage) : base("div")
        {
            Attr("data-for", inputId);
            Attr("data-controller", "validation-message");
            AddClass("invalid-feedback");
            Id(id);
            Text(errorMessage);
        }
    }
}