using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using Miru.Fabrication;
using Miru.Testing;
using Miru.Validation;
using NUnit.Framework;

namespace Miru.Tests.Validations;

public class NumericValidatorTest
{
    private ITestFixture _;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _ = new ServiceCollection()
            .AddMiruApp()
            .AddMiruTestFixture()
            .AddFabrication()
            .AddValidators<ValidationTest>()
            .BuildServiceProvider()
            .GetService<ITestFixture>();
    }
    
    [Test]
    public void Between_min_and_max()
    {
        _.ShouldBeInvalid(new Dto { Between10And20 = 9 }, x => x.Between10And20);
        _.ShouldBeInvalid(new Dto { Between10And20 = 21 }, x => x.Between10And20);

        _.ShouldBeValid(new Dto { Between10And20 = 10 }, x => x.Between10And20);
        _.ShouldBeValid(new Dto { Between10And20 = 11 }, x => x.Between10And20);
        _.ShouldBeValid(new Dto { Between10And20 = 19 }, x => x.Between10And20);
        _.ShouldBeValid(new Dto { Between10And20 = 20 }, x => x.Between10And20);
    }
        
    [Test]
    public void Minimum()
    {
        _.ShouldBeInvalid(new Dto { Min50 = 49 }, x => x.Min50);
        
        _.ShouldBeValid(new Dto { Min50 = 50 }, x => x.Min50);
        _.ShouldBeValid(new Dto { Min50 = 51 }, x => x.Min50);
        _.ShouldBeValid(new Dto { Min50 = long.MaxValue }, x => x.Min50);
    }
        
    [Test]
    public void Numeric()
    {
        _.ShouldBeValid(new Dto() { Numeric = 1 }, x => x.Numeric);
        _.ShouldBeValid(new Dto() { Numeric = long.MinValue }, x => x.Numeric);
        _.ShouldBeValid(new Dto() { Numeric = long.MaxValue }, x => x.Numeric);
    }
        
    public class Dto
    {
        public long Between10And20 { get; set; }
        public long Min50 { get; set; }
        public long Numeric { get; set; }
    }

    public class Validator : AbstractValidator<Dto>
    {
        public Validator()
        {
            RuleFor(m => m.Between10And20).Numeric(min: 10, max: 20);
                
            RuleFor(m => m.Min50).Numeric(min: 50);
                
            RuleFor(m => m.Numeric).Numeric();
        }
    }
}