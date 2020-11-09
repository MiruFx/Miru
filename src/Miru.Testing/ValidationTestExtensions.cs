using System;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.TestHelper;

namespace Miru.Testing
{
    public static class ValidationTestExtensions
    {
        public static void SetValue<TRequest, TProperty>(
            this TRequest model,
            Expression<Func<TRequest, TProperty>> expression,
            TProperty value)
        {
            // TODO: move to some appropriate class, like reflection
            new MemberAccessor<TRequest, TProperty>(expression, true).Set(model, value);
        }
        
        public static void ShouldBeValid<TModel>(this TestFixture fixture, TModel model) where TModel : class, new()
        {
            // var result = fixture.FindValidatorFor<TModel>().Validate(model);
            
            // if (result.IsValid == false)
            //     throw new ValidationException(result.Errors);
        }
        
        public static void ShouldBeValid<TModel, TProperty>(
            this TestFixture fixture, 
            TModel validModel,
            Expression<Func<TModel, TProperty>> expression,
            TProperty validValue) where TModel : class, new()
        {
            validModel.SetValue(expression, validValue);
            
            // fixture.FindValidatorFor<TModel>().ShouldNotHaveValidationErrorFor(expression, validModel);
        }
        
        public static void ShouldBeInvalid<TModel, TProperty>(
            this TestFixture fixture, 
            TModel validModel,
            Expression<Func<TModel, TProperty>> expression,
            TProperty invalidValue) where TModel : class, new()
        {
            validModel.SetValue(expression, invalidValue);
            
            // fixture.FindValidatorFor<TModel>().ShouldHaveValidationErrorFor(expression, validModel);
        }

        public static IValidator<TRequest> FindValidatorFor<TRequest>(this ScopedServices scope)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(typeof(TRequest));

            var validator = scope.Get(validatorType) as IValidator<TRequest>;

            if (validator == null)
                throw new MiruException(
                    $"Could not find a Validator of type {validatorType} for request of type {typeof(TRequest)}. Check if the Validators are being registered in the Container");

            return validator;
        }
        
        public static void ShouldBeValid<TModel>(
            this IValidator<TModel> validator, 
            TModel request) where TModel : class, new()
        {
            var result = validator.Validate(request);
            
            if (result.IsValid == false)
                throw new ValidationException(result.Errors);
        }
        
        public static void ShouldBeValid<TModel, TProperty>(
            this IValidator<TModel> validator, 
            TModel request,
            Expression<Func<TModel, TProperty>> expression,
            TProperty validValue) where TModel : class, new()
        {
            request.SetValue(expression, validValue);
            
            validator.ShouldNotHaveValidationErrorFor(expression, request);
        }
        
        public static void ShouldBeInvalid<TModel, TProperty>(
            this IValidator<TModel> validator, 
            TModel request,
            Expression<Func<TModel, TProperty>> expression,
            TProperty validValue) where TModel : class, new()
        {
            request.SetValue(expression, validValue);
            
            validator.ShouldHaveValidationErrorFor(expression, request);
        }
    }
}