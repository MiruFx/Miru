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
            
            var form = new FormTag().NoClosingTag();
            
            if (request.Model != null)
                form.Id(naming.Form(request.Model));

            return form;
        }
    }
}