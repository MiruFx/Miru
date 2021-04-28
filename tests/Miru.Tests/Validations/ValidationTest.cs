using System.ComponentModel.DataAnnotations;
using System.Globalization;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using Miru.Testing;
using Miru.Validation;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Validations
{
    public class ValidationTest
    {
        private CultureInfo _currentCulture;
        private ServiceProvider _sp;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _sp = new ServiceCollection()
                .AddValidators<ValidationTest>()
                .BuildServiceProvider();
        }
        [SetUp]
        public void Setup()
        {
            _currentCulture = CultureInfo.CurrentUICulture;
        }

        [TearDown]
        public void Teardown()
        {
            CultureInfo.CurrentUICulture = _currentCulture;
        }
        
        [Test]
        public void Should_return_errors_in_current_culture_language()
        {
            // arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("pt-BR");
            var validator = _sp.GetRequiredService<IValidator<Customer>>();

            // act
            var result = validator.Validate(new Customer());
            
            // assert
            result.Errors[0].ErrorMessage.ShouldBe("'Name' deve ser informado.");
        }

        [Test]
        public void If_property_has_display_attr_then_should_return_errors_with_display_name_()
        {
            // arrange
            var validator = _sp.GetRequiredService<IValidator<Product>>();

            // act
            var result = validator.Validate(new Product());
            
            // assert
            result.Errors[0].ErrorMessage.ShouldBe("'Product Name' must not be empty.");
        }

        public class CustomerValidator : AbstractValidator<Customer>
        {
            public CustomerValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
        
        public class ProductValidator : AbstractValidator<Product>
        {
            public ProductValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
        
        public class Customer
        {
            public string Name { get; set; }
        }
        
        public class Product
        {
            [Display(Name = "Product Name")]
            public string Name { get; set; }
        }
    }
}