using System.ComponentModel.DataAnnotations;
using System.Globalization;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Miru.Fabrication;
using Miru.Testing;
using Miru.Validation;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Validations;

public class ValidationTest
{
    private CultureInfo _currentCulture;
    private ServiceProvider _sp;
    private ITestFixture _;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _sp = new ServiceCollection()
            .AddMiruApp()
            .AddMiruTestFixture()
            .AddFabrication()
            .AddValidators<ValidationTest>()
            .BuildServiceProvider();

        _ = _sp.GetService<ITestFixture>();
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

    [Test]
    public void Should_check_valid_properties()
    {
        var request = _.Make<Customer>();
        
        _.ShouldBeValid(request, m => m.Name, request.Name);
    }

    [Test]
    public void Should_check_invalid_properties()
    {
        var request = _.Make<Customer>();
        
        _.ShouldBeInvalid(request, m => m.Name, string.Empty);
    }
    
    // public class Validator : ValidationTest<Customer>
    // {
    //     [Test]
    //     public void Should_check_valid_properties()
    //     {
    //         ShouldBeValid(Request, m => m.Name, Request.Name);
    //     }
    //
    //     [Test]
    //     public void Should_check_invalid_properties()
    //     {
    //         ShouldBeInvalid(Request, m => m.Name, string.Empty);
    //     }
    // }
    
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