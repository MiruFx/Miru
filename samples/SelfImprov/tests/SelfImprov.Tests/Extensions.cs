using System;
using Miru.Testing;
using SelfImprov.Database;
using SelfImprov.Domain;

namespace SelfImprov.Tests
{
    public static class Extensions
    {
        public static TReturn Db<TReturn>(this ITestFixture fixture, Func<SelfImprovDbContext, TReturn> func)
        {
            return fixture.WithDb(func);
        }
        
        public static SelfImprovFabricator Fab(this ITestFixture fixture)
        {
            return fixture.Get<SelfImprovFabricator>();
        }
    }
}
