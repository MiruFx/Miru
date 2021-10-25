using System;
using Corpo.Skeleton.Database;

namespace Corpo.Skeleton.Tests;

public static class Extensions
{
    public static void Db(
        this ITestFixture fixture, Action<SkeletonDbContext> func) => 
        fixture.WithDb(func);
        
    public static TReturn Db<TReturn>(
        this ITestFixture fixture, Func<SkeletonDbContext, TReturn> func) => 
        fixture.WithDb(func);
        
    public static SkeletonFabricator Fab(this ITestFixture fixture) => 
        fixture.Get<SkeletonFabricator>();
}