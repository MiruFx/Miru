using System;
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
        
        public static IRuleBuilderOptions<T, string> Numeric<T>(this IRuleBuilder<T, string> ruleBuilder) 
        {
            return ruleBuilder.SetValidator(new NumericValidator());
        }
        
        public static IRuleBuilderOptions<T, string> Numeric<T>(this IRuleBuilder<T, string> ruleBuilder, long min) 
        {
            return ruleBuilder.SetValidator(new NumericValidator(min));
        }
        
        public static IRuleBuilderOptions<T, string> Numeric<T>(this IRuleBuilder<T, string> ruleBuilder, long min, long max) 
        {
            return ruleBuilder.SetValidator(new NumericValidator(min, max));
        }
        
        public static DescriptorBuilder DescriptorFor(this IValidator validator, string propertyName)
        {
            return GetDescriptorBuilder(validator, propertyName);
        }
        
        public static DescriptorBuilder DescriptorFor<T, TProperty>(this IValidator<T> validator, Expression<Func<T, TProperty>> expression)
        {
            var property = ReflectionHelper.GetProperty(expression);
            return GetDescriptorBuilder(validator, property.Name);
        }

        private static DescriptorBuilder GetDescriptorBuilder(IValidator validator, string propertyName)
        {
            var descriptor = validator.CreateDescriptor();
            var validators = descriptor.GetValidatorsForMember(propertyName);

            return new DescriptorBuilder(validators);
        }
    }
}