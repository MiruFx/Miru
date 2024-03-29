using System;
using System.Linq;
using System.Text;
using AutoFixture.Kernel;

namespace Miru.Fabrication.FixtureConventions
{
    public static class FixtureExtensions
    {
        // private static readonly string Space = " ";
        // private static readonly string Dash = "-";

        public static object CreateByType(this Fixture fixture, Type resolveType)
        {
            return new SpecimenContext(fixture).Resolve(resolveType);
        }
        
        public static Fixture OmitRecursion(this Fixture fixture)
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
        
        public static Fixture AddConvention(this Fixture fixture, Action<ConventionExpression> convention)
        {
            return fixture.AddConvention(new Faker(), convention);
        }
        
        public static Fixture AddConvention(this Fixture fixture, Faker faker, Action<ConventionExpression> convention)
        {
            var conventionExpression = new ConventionExpression(faker);
            
            convention(conventionExpression);

            fixture.AddOrInsertMiruSpecimen(conventionExpression);
            
            return fixture;
        }
        
        private static void AddOrInsertMiruSpecimen(this Fixture fixture, ConventionExpression conventionExpression)
        {
            var existentSpecimenIndex = fixture.Customizations.FindIndex(x => x is MiruConventionSpecimenBuilder);

            var specimen = new MiruConventionSpecimenBuilder(conventionExpression);
            
            if (existentSpecimenIndex < 0)
                fixture.Customizations.Add(specimen);
            else
                fixture.Customizations.Insert(existentSpecimenIndex, specimen);
        }
    }
}