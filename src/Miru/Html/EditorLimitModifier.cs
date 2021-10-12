using System;
using Baseline.Reflection;
using FluentValidation.Validators;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;

namespace Miru.Html
{
    public class EditorLimitModifier : IElementModifier
    {
        public bool Matches(ElementRequest elementRequest) => 
            elementRequest.FindModel().GetType().Implements<INoEditorEnricher>() == false;

        public void Modify(ElementRequest request)
        {
            var rules = request.ValidatorRules();

            var maximumLengthValidator = rules.Get<IMaximumLengthValidator>();

            if (maximumLengthValidator != null)
            {
                request.CurrentTag.MaxLength(maximumLengthValidator.Max);
            }

            if ((request.Accessor.PropertyType.IsAnyOf<DateTime, DateTime?>())
                && request.Accessor.InnerProperty.HasAttribute<DateOnlyAttribute>())
            {
                request.CurrentTag.MaxLength(10);
            }
        }
    }
}