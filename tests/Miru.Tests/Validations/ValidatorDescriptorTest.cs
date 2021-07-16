using FluentValidation;
using FluentValidation.Validators;
using Miru.Validation;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Validations
{
    public class ValidatorDescriptorTest
    {
        [Test]
        public void Should_return_if_a_rule_has_a_validator()
        {
            // arrange
            var validator = new Validator();
            var descriptor = validator.DescriptorFor(nameof(Command.Name));

            // act & assert
            descriptor.Has<INotEmptyValidator>().ShouldBeTrue();
            descriptor.Has(typeof(INotEmptyValidator)).ShouldBeTrue();
            descriptor.Has(typeof(NotEmptyValidator<,>)).ShouldBeTrue();
            
            descriptor.Has<IMaximumLengthValidator>().ShouldBeTrue();
            descriptor.Has(typeof(IMaximumLengthValidator)).ShouldBeTrue();
            descriptor.Has(typeof(MaximumLengthValidator<>)).ShouldBeTrue();
            
            descriptor.Has<ICreditCardValidator>().ShouldBeFalse();
            descriptor.Has(typeof(ICreditCardValidator)).ShouldBeFalse();
            descriptor.Has(typeof(CreditCardValidator<>)).ShouldBeFalse();
        }

        [Test]
        public void Should_return_validator_of_a_rule()
        {
            // arrange
            var validator = new Validator();
            var descriptor = validator.DescriptorFor(nameof(Command.Name));

            // act
            var maxLengthValidator = descriptor.Get<IMaximumLengthValidator>();
            
            // assert
            maxLengthValidator.Max.ShouldBe(50);
        }

        public class Command
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Age);
            }
        }
    }
}