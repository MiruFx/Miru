using System;
using System.Linq.Expressions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Miru.Testing
{
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
        
        public void ShouldBeValid<TProperty>(Expression<Func<TRequest, TProperty>> expression, TProperty validValue)
        {
            using var scope = _.App.WithScope();
            scope.FindValidatorFor<TRequest>().ShouldNotHaveValidationErrorFor(expression, validValue);
        }
        
        public void ShouldBeValid<TProperty>(Expression<Func<TRequest, TProperty>> expression, TRequest model)
        {
            using var scope = _.App.WithScope();
            scope.FindValidatorFor<TRequest>().ShouldNotHaveValidationErrorFor(expression, model);
        }
        
        public void ShouldBeInvalid<TProperty>(Expression<Func<TRequest, TProperty>> expression, TProperty validValue)
        {
            using var scope = _.App.WithScope();
            scope.FindValidatorFor<TRequest>().ShouldHaveValidationErrorFor(expression, validValue);
        }
    }
}