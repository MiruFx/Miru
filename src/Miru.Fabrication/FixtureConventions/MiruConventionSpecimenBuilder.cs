using System.Reflection;
using AutoFixture.Kernel;

namespace Miru.Fabrication.FixtureConventions
{
    public class MiruConventionSpecimenBuilder : ISpecimenBuilder
    {
        public ConventionExpression Convention { get; }

        public MiruConventionSpecimenBuilder(ConventionExpression convention)
        {
            Convention = convention;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propertyInfo = request as PropertyInfo;
            
            if (propertyInfo != null)
            {
                foreach (var expression in Convention.Expressions)
                {
                    if (expression.IfProperty != null && expression.IfProperty.Compile().Invoke(propertyInfo))
                    {
                        if (expression.ShouldIgnore)
                            return new OmitSpecimen();
                    
                        return expression.UseValue();
                    }
                }
            }

            var typeInfo = request as TypeInfo;
            
            if (typeInfo != null)
            {
                foreach (var expression in Convention.Expressions)
                {
                    if (expression.IfClass != null && expression.IfClass.Compile().Invoke(typeInfo))
                    {
                        if (expression.ShouldIgnore)
                            return null;
                    
                        return expression.UseValue();
                    }
                }
            }
            
            return new NoSpecimen();
        }
    }
}