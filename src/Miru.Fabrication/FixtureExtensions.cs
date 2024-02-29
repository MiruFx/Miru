using Ardalis.SmartEnum;
using Baseline.Dates;
using Microsoft.AspNetCore.Http;
using Miru.Domain;
using Miru.Fabrication.FixtureConventions;
using Miru.Mvc;
using Miru.Userfy;

namespace Miru.Fabrication;

public static class FixtureExtensions
{
    private static readonly string HashedPassword = nameof(HashedPassword);

    public static Fixture AddDefaultMiruConvention(this Fixture fixture, Faker faker)
    {
        fixture.OmitRecursion();
            
        fixture.AddConvention(faker, x =>
        {
            x.AddEntityFramework();

            // Ignore types
            x.IfPropertyImplementsEnumerableOf<IEntity>().Ignore();

            x.IfPropertyImplements(typeof(ISmartEnum)).Ignore();
            // _.IfPropertyImplements(typeof(Enumeration<>)).Ignore();
            x.IfPropertyImplements<IEnumerable<ILookupable>>().Ignore();
            x.IfPropertyTypeIs<SelectLookups>().Ignore();
            x.IfPropertyTypeIs<IFormFile>().Ignore();
            
            x.AddAutoFaker();
                
            // default values for types
            x.IfPropertyTypeIs<bool>().Use(_ => false);
            
            // Potential additions to AutoFaker
            x.IfPropertyNameIs("Name").Use(f => f.Person.FirstName);
                
            // Userfy
            x.IfPropertyNameStarts("Password").Use("123456");
            x.IfPropertyNameIs(HashedPassword).Use(() => Hash.Create("123456"));

            x.IfPropertyNameIs(nameof(IRecoverable.ResetPasswordToken)).Ignore();
            x.IfPropertyNameIs(nameof(IRecoverable.ResetPasswordSentAt)).Ignore();
                
            x.IfPropertyNameIs(nameof(IConfirmable.ConfirmationToken)).Use(() => null);
            x.IfPropertyNameIs(nameof(IConfirmable.ConfirmationSentAt)).Use(() => null);
            x.IfPropertyNameIs(nameof(IConfirmable.ConfirmedAt)).Use(() => 5.Minutes().Ago());
                
            x.IfPropertyNameIs(nameof(ICanBeAdmin.IsAdmin)).Use(() => false);
                
            // CreditCard
            x.IfPropertyNameIs("CreditCard").Use(f => f.Finance.CreditCardNumber());
                
            // Dates
            // _.IfPropertyTypeIs<DateTimeOffset>().Use(f => f.Date.FutureOffset().Time);
        });
            
        return fixture;
    }
}