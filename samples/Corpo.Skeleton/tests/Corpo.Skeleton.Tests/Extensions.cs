using System;
using Corpo.Skeleton.Database;

namespace Corpo.Skeleton.Tests;

public static class Extensions
{
    public static void Db(
        this ITestFixture fixture, Action<AppDbContext> func) => 
        fixture.WithDb(func);
        
    public static TReturn Db<TReturn>(
        this ITestFixture fixture, Func<AppDbContext, TReturn> func) => 
        fixture.WithDb(func);
        
    public static AppFabricator Fab(this ITestFixture fixture) => 
        fixture.Get<AppFabricator>();
}