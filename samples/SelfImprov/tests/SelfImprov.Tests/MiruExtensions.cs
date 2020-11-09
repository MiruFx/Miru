using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Miru;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Testing;
using SelfImprov.Database;
using SelfImprov.Domain;
using Shouldly;

namespace SelfImprov.Tests
{
    public static class MiruExtensions
    {
        // public static void ShouldBeAprox(this decimal current, decimal value)
        // {
        //     current.ShouldBe(value, 0.01m);
        // }
        
        // public static T Make<T>(this ITestFixture fixture, params Action<T>[] customizations) where T : class
        // {
        //     var made = fixture.Get<Fabricator>().Make<T>();
        //
        //     foreach (var customization in customizations)
        //     {
        //         customization(made);
        //     }
        //
        //     return made;
        // }
    }
}
