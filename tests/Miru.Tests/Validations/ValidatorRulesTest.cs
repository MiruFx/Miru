using FluentValidation;
using FluentValidation.Validators;
using Miru.Validation;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Validations;

public class ValidatorRulesTest
{
    [Test]
    public void Should_return_if_a_rule_has_a_validator()
    {
        // arrange
        var validator = new Validator();
        var rules = validator.RulesFor(nameof(Command.Name));

        // act & assert
        rules.Has<INotEmptyValidator>().ShouldBeTrue();
        rules.Has(typeof(INotEmptyValidator)).ShouldBeTrue();
        rules.Has(typeof(NotEmptyValidator<,>)).ShouldBeTrue();
            
        rules.Has<IMaximumLengthValidator>().ShouldBeTrue();
        rules.Has(typeof(IMaximumLengthValidator)).ShouldBeTrue();
        rules.Has(typeof(MaximumLengthValidator<>)).ShouldBeTrue();
            
        rules.Has<ICreditCardValidator>().ShouldBeFalse();
        rules.Has(typeof(ICreditCardValidator)).ShouldBeFalse();
        rules.Has(typeof(CreditCardValidator<>)).ShouldBeFalse();
    }

    [Test]
    public void Should_return_the_validator_of_a_rule()
    {
        // arrange
        var validator = new Validator();
        var rules = validator.RulesFor(nameof(Command.Name));

        // act & assert
        rules.Get<INotEmptyValidator>().ShouldNotBeNull();
        rules.Get(typeof(INotEmptyValidator)).ShouldNotBeNull();
        rules.Get(typeof(NotEmptyValidator<,>)).ShouldNotBeNull();
            
        rules.Get<IMaximumLengthValidator>().Max.ShouldBe(50);
        rules.Get(typeof(IMaximumLengthValidator)).GetType().Implements(typeof(MaximumLengthValidator<>));
        rules.Get(typeof(MaximumLengthValidator<>)).GetType().Implements(typeof(MaximumLengthValidator<>));
            
        rules.Get<ICreditCardValidator>().ShouldBeNull();
        rules.Get(typeof(ICreditCardValidator)).ShouldBeNull();
        rules.Get(typeof(CreditCardValidator<>)).ShouldBeNull();
    }

    public class Command
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Alive { get; set; }
        public int YearsWorked { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Age);

            When(x => x.Alive, () =>
            {
                RuleFor(x => x.YearsWorked).NotEmpty();
            });
        }
    }
}