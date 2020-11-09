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
            // var validatorFactory = elementRequest.Get<ValidatorFactory>();
            //
            // if (validatorFactory == null)
            //     return false;

            var validator = (IValidator) elementRequest.Get(typeof(IValidator<>).MakeGenericType(elementRequest.HolderType()));

            if (validator == null)
                return false;

            if (validator.DescriptorFor(elementRequest.Accessor.Name).Has<NotEmptyValidator>())
                return true;

            var childValidator = (IValidator) elementRequest.Get(typeof(IValidator<>).MakeGenericType(elementRequest.Accessor.OwnerType));

            if (childValidator == null)
                return false;

            if (childValidator.DescriptorFor(elementRequest.Accessor.InnerProperty.Name).Has<NotEmptyValidator>())
                return true;
            
            return false;
        }

        public void Modify(ElementRequest request)
        {
            request.CurrentTag.Append("span", span => span.AddClass("text-danger").Text(" *"));
        }
    }
}