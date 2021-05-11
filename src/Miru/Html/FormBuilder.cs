using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;

namespace Miru.Html
{
    public class FormBuilder : IElementBuilder
    {
        public HtmlTag Build(ElementRequest request)
        {
            var naming = request.Get<ElementNaming>();

            var formTag = new FormTag();

            if (request.Model != null)
            {
                formTag
                    .Id(naming.Form(request.Model))
                    .Attr("data-form-summary", naming.FormSummaryId(request.Model));
            }

            return formTag                
                .NoClosingTag()
                .Attr("data-controller", "form");
        }
    }
}