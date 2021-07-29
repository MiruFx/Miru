using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Reflection;
using FluentValidation;

namespace Miru.Validation
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> UniqueAsync<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            Func<T, CancellationToken, Task<bool>> predicate)
        {
            var rule = ruleBuilder
                .MustAsync(async (cmd, str, ct) => await predicate(cmd, ct));
            
            return rule.WithMessage("{PropertyName} has already been taken");
        }
        
        public static IRuleBuilderOptions<T, long> Numeric<T>(this IRuleBuilder<T, long> ruleBuilder) 
        {
            return ruleBuilder.SetValidator(new NumericValidator<T>());
        }
        
        public static IRuleBuilderOptions<T, long> Numeric<T>(this IRuleBuilder<T, long> ruleBuilder, long min) 
        {
            return ruleBuilder.SetValidator(new NumericValidator<T>(min));
        }
        
        public static IRuleBuilderOptions<T, long> Numeric<T>(this IRuleBuilder<T, long> ruleBuilder, long min, long max) 
        {
            return ruleBuilder.SetValidator(new NumericValidator<T>(min, max));
        }
        
        public static ValidatorRules RulesFor(this IValidator validator, string propertyName)
        {
            return GetDescriptorBuilder(validator, propertyName);
        }
        
        public static ValidatorRules RulesFor<T, TProperty>(this IValidator<T> validator, Expression<Func<T, TProperty>> expression)
        {
            var property = ReflectionHelper.GetProperty(expression);
            return GetDescriptorBuilder(validator, property.Name);
        }

        private static ValidatorRules GetDescriptorBuilder(IValidator validator, string propertyName)
        {
            var descriptor = validator.CreateDescriptor();
            var validators = descriptor.GetValidatorsForMember(propertyName);

            return new ValidatorRules(validators.Select(x => x.Validator));
        }
    }
}