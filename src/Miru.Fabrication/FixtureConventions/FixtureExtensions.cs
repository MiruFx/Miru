using System;
using System.Linq;
using System.Text;
using AutoFixture;
using AutoFixture.Kernel;
using Bogus;

namespace Miru.Fabrication.FixtureConventions
{
    public static class FixtureExtensions
    {
        private static readonly string Space = " ";
        private static readonly string Dash = "-";

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

        public static string WhatConventionsDoIHave(this Fixture fixture)
        {
            var conventionsText = new StringBuilder();

            conventionsText.Append(
                "List of Conventions. Ordered descending by time added. Create() will use the first Convention found in this order:");

            conventionsText.Append(Environment.NewLine);
            conventionsText.Append(Environment.NewLine);
            
            var allConventionsSpecimen = fixture.Customizations
                .Where(c => c is MiruConventionSpecimenBuilder);

            foreach (MiruConventionSpecimenBuilder conventionSpecimen in allConventionsSpecimen)
            {
                foreach (var expression in conventionSpecimen.Convention.Expressions)
                {
                    conventionsText.Append(Dash);
                    conventionsText.Append(Space);

                    if (expression.IsForClass)
                    {
                        conventionsText.Append(nameof(IfFilterExpression.IfClass));
                        conventionsText.Append(Space);
                        conventionsText.Append(expression.IfClass);
                    }

                    if (expression.IsForProperty)
                    {
                        conventionsText.Append(nameof(IfFilterExpression.IfProperty));
                        conventionsText.Append(Space);
                        conventionsText.Append(expression.IfProperty);
                    }

                    conventionsText.Append(" => ");

                    if (expression.ShouldIgnore)
                        conventionsText.Append(nameof(IfFilterExpression.Ignore));
                    else if (expression.UseValue != null)
                        conventionsText.Append(nameof(IfFilterExpression.Use));
                    else
                        conventionsText.Append("NO ACTION DEFINED!");
                    
                    conventionsText.Append(Environment.NewLine);
                    conventionsText.Append(Environment.NewLine);
                }
            }

            return conventionsText.ToString();
        }
    }
}