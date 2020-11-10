using System.Collections.Generic;
using AutoFixture;
using Baseline.Dates;
using Bogus;
using Miru.Domain;
using Miru.Fabrication.FixtureConventions;
using Miru.Userfy;

namespace Miru.Fabrication
{
    public static class FixtureExtensions
    {
        private static readonly string HashedPassword = nameof(HashedPassword);

        public static Fixture AddDefaultMiruConvention(this Fixture fixture, Faker faker)
        {
            fixture.OmitRecursion();
            
            fixture.AddConvention(faker, _ =>
            {
                _.AddEntityFramework();

                // Ignore types
                _.IfPropertyImplementsEnumerableOf<IEntity>().Ignore();

                _.IfPropertyImplements(typeof(Enumeration<,>)).Ignore();
                _.IfPropertyImplements(typeof(Enumeration<>)).Ignore();
                _.IfPropertyImplements<IEnumerable<ILookupable>>().Ignore();
                
                _.AddAutoFaker();
                
                // Potential additions to AutoFaker
                
                _.IfPropertyNameIs("Name").Use(f => f.Name.FirstName());
                
                // Userfy
                _.IfPropertyNameStarts("Password").Use("123456");
                _.IfPropertyNameIs(HashedPassword).Use(() => Hash.Create("123456"));

                _.IfPropertyNameIs(nameof(IRecoverable.ResetPasswordToken)).Ignore();
                _.IfPropertyNameIs(nameof(IRecoverable.ResetPasswordSentAt)).Ignore();
                
                _.IfPropertyNameIs(nameof(IConfirmable.ConfirmationToken)).Use(() => null);
                _.IfPropertyNameIs(nameof(IConfirmable.ConfirmationSentAt)).Use(() => null);
                _.IfPropertyNameIs(nameof(IConfirmable.ConfirmedAt)).Use(() => 5.Minutes().Ago());
                
                _.IfPropertyNameIs(nameof(ICanBeAdmin.IsAdmin)).Use(() => false);
                
                // CreditCard
                _.IfPropertyNameIs("CreditCard").Use(f => f.Finance.CreditCardNumber());
            });
            
            return fixture;
        }
    }
}