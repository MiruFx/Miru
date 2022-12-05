using FluentValidation;
using FluentValidation.Validators;
using Miru.Validation;

namespace Miru.Html.HtmlConfigs;

public static class ElementRequestExtensions
{
    public static bool IsAssignable<TProperty>(this ElementRequest req)
    {
         var type = typeof(TProperty);
         
         return type.IsAssignableFrom(req.Accessor.PropertyType);
        
        // if (!assignable && type.IsValueType)
        //     assignable = typeof(Nullable<>).MakeGenericType(type).IsAssignableFrom(req.Accessor.PropertyType);
        //
        //  return assignable;
    }
    
    public static TPropertyValidator GetValidator<TPropertyValidator>(this ElementRequest elementRequest)
        where TPropertyValidator : class, IPropertyValidator
    {
        // TODO: cache validators for the declaring type
        // TODO: extract to a class
        
        var validator = (IValidator) elementRequest
            .Get(typeof(IValidator<>).MakeGenericType(elementRequest.Accessor.DeclaringType));

        if (validator == null)
            return null;

        var propertyValidator = validator.RulesFor(elementRequest.Accessor.Name);

        if (propertyValidator is null)
        {
            // if main request validator is null, lets try the child validators
            var childValidator = (IValidator) elementRequest
                .Get(typeof(IValidator<>).MakeGenericType(elementRequest.Accessor.OwnerType));

            if (childValidator is null)
                return null;
            
            propertyValidator = childValidator.RulesFor(elementRequest.Accessor.Name);
        }
        
        return propertyValidator.Get<TPropertyValidator>();
    }
}