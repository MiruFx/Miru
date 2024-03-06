using System.Linq.Expressions;
using Baseline.Reflection;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Miru.Testing;

public static class ValidationTestExtensions
{
    public static void SetValue<TRequest, TProperty>(
        this TRequest model,
        Expression<Func<TRequest, TProperty>> expression,
        TProperty value)
    {
        // TODO: move to some appropriate class, like reflection
        ReflectionHelper.GetProperty(expression).SetValue(model, value);
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

    public static void ShouldBeValid<TRequest, TProperty>(
        this ITestFixture fixture,
        TRequest model,
        Expression<Func<TRequest, TProperty>> expression, 
        TProperty validValue)
    {
        using var scope = fixture.App.WithScope();
            
        var validator = scope.FindValidatorFor<TRequest>();

        model.SetValue(expression, validValue);
        
        var testResult = validator.TestValidate(model);
        
        testResult.ShouldNotHaveValidationErrorFor(expression);
    }
        
    public static void ShouldBeValid<TRequest, TProperty>(
        this ITestFixture fixture,
        TRequest model,
        Expression<Func<TRequest, TProperty>> expression)
    {
        using var scope = fixture.App.WithScope();
            
        var validator = scope.FindValidatorFor<TRequest>();

        var testResult = validator.TestValidate(model);
        
        testResult.ShouldNotHaveValidationErrorFor(expression);
    }
        
    public static void ShouldBeInvalid<TRequest, TProperty>(
        this ITestFixture fixture,
        TRequest model,
        Expression<Func<TRequest, TProperty>> expression, 
        TProperty invalidValue)
    {
        using var scope = fixture.App.WithScope();
            
        var validator = scope.FindValidatorFor<TRequest>();

        model.SetValue(expression, invalidValue);
        
        var testResult = validator.TestValidate(model);
        
        testResult.ShouldHaveValidationErrorFor(expression);
    }
    
    public static void ShouldBeInvalid<TRequest, TProperty>(
        this ITestFixture fixture,
        TRequest model,
        Expression<Func<TRequest, TProperty>> expression)
    {
        using var scope = fixture.App.WithScope();
            
        var validator = scope.FindValidatorFor<TRequest>();

        var testResult = validator.TestValidate(model);
        
        testResult.ShouldHaveValidationErrorFor(expression);
    }
}