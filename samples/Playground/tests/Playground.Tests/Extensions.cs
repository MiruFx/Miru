using System;
using Miru.Testing;
using Playground.Database;

namespace Playground.Tests
{
    public static class Extensions
    {
        public static TReturn Db<TReturn>(this ITestFixture fixture, Func<PlaygroundDbContext, TReturn> func)
        {
            return fixture.WithDb(func);
        }
        
        public static PlaygroundFabricator Fab(this ITestFixture fixture)
        {
            return fixture.Get<PlaygroundFabricator>();
        }
    }
}
