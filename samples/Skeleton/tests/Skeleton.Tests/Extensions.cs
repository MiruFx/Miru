using System;
using Miru.Testing;
using Skeleton.Database;
using Skeleton.Domain;

namespace Skeleton.Tests
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