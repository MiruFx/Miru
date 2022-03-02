using System;
using System.Linq.Expressions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Miru.Testing;

public class ValidationTest<TRequest> : OneCaseFeatureTest where TRequest : class, new()
{
    protected TRequest Request;
        
    [OneTimeSetUp]
    public void ValidationOneTimeSetup()
    {
        Request = _.Make<TRequest>();
    }

    [TearDown]
    public void Teardown()
    {
    }
        
    public void ShouldBeValid<TProperty>(
        TRequest model,
        Expression<Func<TRequest, TProperty>> expression, 
        TProperty validValue)
    {
        using var scope = _.App.WithScope();
            
        var validator = scope.FindValidatorFor<TRequest>();

        var testResult = validator.TestValidate(model);
        
        testResult.ShouldNotHaveValidationErrorFor(expression);
    }
        
    public void ShouldBeValid<TProperty>(
        TRequest model,
        Expression<Func<TRequest, TProperty>> expression)
    {
        using var scope = _.App.WithScope();
            
        var validator = scope.FindValidatorFor<TRequest>();

        var testResult = validator.TestValidate(model);
        
        testResult.ShouldNotHaveValidationErrorFor(expression);
    }
        
    public void ShouldBeInvalid<TModel, TProperty>(
        TModel model,
        Expression<Func<TModel, TProperty>> expression, 
        TProperty validValue)
    {
        using var scope = _.App.WithScope();
            
        var validator = scope.FindValidatorFor<TModel>();

        var testResult = validator.TestValidate(model);
        
        testResult.ShouldHaveValidationErrorFor(expression);
    }
}