using Bogus;
using Miru.Fabrication;
using Miru.Foundation.Logging;
using Miru.Userfy;

namespace Miru.Testing;

public static class TestFixtureFabricationExtensions
{
    public static Faker Faker(this ITestFixture fixture)
    {
        return fixture.Get<Faker>();
    }

    public static T Make<T>(this ITestFixture fixture, Action<T> customizations = null) where T : class
    {
        return fixture.Get<Fabricator>().Make(customizations);
    }
        
    public static IEnumerable<T> MakeMany<T>(this ITestFixture fixture, int make = 3, Action<T> customizations = null) where T : class
    {
        return fixture.Get<Fabricator>().MakeMany(make, customizations);
    }
        
    public static IEnumerable<T> MakeMany<T>(this ITestFixture fixture, Action<T> customizations) where T : class
    {
        return fixture.Get<Fabricator>().MakeMany(3, customizations);
    }
        
    public static T Make<T>(this ITestFixture fixture, params Action<T>[] customizations) where T : class
    {
        var made = fixture.Get<Fabricator>().Make<T>();

        foreach (var customization in customizations)
        {
            customization(made);
        }

        return made;
    }
}