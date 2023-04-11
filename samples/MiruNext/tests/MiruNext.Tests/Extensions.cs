using System;
using Miru.Testing;
using MiruNext.Database;

namespace MiruNext.Tests;

public static class Extensions
{
    public static TReturn Db<TReturn>(this ITestFixture fixture, Func<AppDbContext, TReturn> func) => 
        fixture.WithDb(func);

    public static AppFabricator Fab(this ITestFixture fixture) => 
        fixture.Get<AppFabricator>();
}