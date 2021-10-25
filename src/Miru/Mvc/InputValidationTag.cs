using HtmlTags;

namespace Miru.Mvc
{
    public class InputValidationTag : HtmlTag
    {
        public InputValidationTag(string id, string inputId, string errorMessage) : base("div")
        {
            Attr("data-for", inputId);
            Attr("data-controller", "form-input-validation");
            AddClass("invalid-feedback");
            Id(id);
            Text(errorMessage);
        }
    }
}