using System;
using Pantanal.Database;
using Miru.Testing;

namespace Pantanal.Tests
{
    public static class Extensions
    {
        public static TReturn Db<TReturn>(this ITestFixture fixture, Func<PantanalDbContext, TReturn> func)
        {
            return fixture.WithDb(func);
        }
        
        public static PantanalFabricator Fab(this ITestFixture fixture)
        {
            return fixture.Get<PantanalFabricator>();
        }
    }
}
