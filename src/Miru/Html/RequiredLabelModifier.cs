using FluentValidation;
using FluentValidation.Validators;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using Miru.Validation;

namespace Miru.Html
{
    public class RequiredLabelModifier : IElementModifier
    {
        public bool Matches(ElementRequest elementRequest)
        {
            var validator = (IValidator) elementRequest.Get(typeof(IValidator<>).MakeGenericType(elementRequest.HolderType()));

            if (validator == null)
                return false;

            if (validator.RulesFor(elementRequest.Accessor.Name).Has(typeof(NotEmptyValidator<,>)))
                return true;

            var childValidator = (IValidator) elementRequest.Get(typeof(IValidator<>).MakeGenericType(elementRequest.Accessor.OwnerType));

            if (childValidator == null)
                return false;

            if (childValidator.RulesFor(elementRequest.Accessor.InnerProperty.Name).Has(typeof(NotEmptyValidator<,>)))
                return true;
            
            return false;
        }

        public void Modify(ElementRequest request)
        {
            request.CurrentTag.Append("span", span => span.AddClass("text-danger").Text(" *"));
        }
    }
}