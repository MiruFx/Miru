using System;
using System.Collections.Generic;
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
        
    public static IEnumerable<T> MakeMany<T>(this ITestFixture fixture, int howMany, Action<T> customizations = null) where T : class
    {
        return fixture.Get<Fabricator>().MakeMany(howMany, customizations);
    }
        
    public static IEnumerable<T> MakeMany<T>(this ITestFixture fixture, Action<T> customizations) where T : class
    {
        return fixture.Get<Fabricator>().MakeMany(3, customizations);
    }
        
    public static T MakeSaving<T>(this ITestFixture fixture, Action<T> customizations = null) where T : class
    {
        var entity = fixture.Make(customizations);

        fixture.Save(entity);

        return entity;
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
        
    public static IEnumerable<T> MakeManySaving<T>(this ITestFixture fixture, int howMany = 3, Action<T> customizations = null) where T : class
    {
        MiruTest.Log.Information($"Making and saving {howMany} {typeof(T).FullName}");
            
        var entities = fixture.Get<Fabricator>().MakeMany(howMany, customizations);

        MiruTest.Log.Debug(() => $"Made:{Environment.NewLine}{entities.Inspect()}");
            
        MiruTest.Log.Debug(() => $"Saving the {howMany} entities");
            
        fixture.Save(entities);

        return entities; 
    }
        
    public static IEnumerable<T> MakeManySaving<T>(this ITestFixture fixture, Action<T> customizations) 
        where T : class
    {
        return fixture.MakeManySaving(3, customizations);
    }
        
    public static TUser MakeSavingLoginAs<TUser>(this ITestFixture fixture, Action<TUser> customizations = null) 
        where TUser : UserfyUser
    {
        MiruTest.Log.Debug(() => $"Making {typeof(TUser).FullName}");
             
        var user = fixture.Make(customizations);
            
        MiruTest.Log.Debug(() => $"Made:{Environment.NewLine}{user.Inspect()}");
            
        return fixture.MakeSavingLoginAs(user);
    }
        
    public static TUser MakeSavingLoginAs<TUser>(this ITestFixture fixture, TUser user) 
        where TUser : UserfyUser
    {
        MiruTest.Log.Debug(() => $"Saving: {user}");
            
        fixture.Save(user);
            
        fixture.LoginAs(user);

        return user;
    }
}