using System;
using Corpo.Skeleton.Database;
using Miru.Testing;

namespace Corpo.Skeleton.Tests
{
    public static class Extensions
    {
        public static TReturn Db<TReturn>(this ITestFixture fixture, Func<SkeletonDbContext, TReturn> func)
        {
            return fixture.WithDb(func);
        }
        
        public static SkeletonFabricator Fab(this ITestFixture fixture)
        {
            return fixture.Get<SkeletonFabricator>();
        }
    }
}