using System.Linq.Expressions;
using FluentValidation.TestHelper;

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
        Expression<Func<TRequest, TProperty>> expression, 
        TProperty validValue)
    {
        _.ShouldBeValid(Request, expression, validValue);
    }
        
    public void ShouldBeValid<TProperty>(
        Expression<Func<TRequest, TProperty>> expression)
    {
        _.ShouldBeValid(Request, expression);
    }
        
    public void ShouldBeInvalid<TProperty>(
        Expression<Func<TRequest, TProperty>> expression, 
        TProperty invalidValue)
    {
        _.ShouldBeInvalid(Request, expression, invalidValue);
    }
}