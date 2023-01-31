using System;
using Miru.Testing;
using MiruNext.Database;

namespace MiruNext.Tests;

public static class Extensions
{
    public static TReturn Db<TReturn>(this ITestFixture fixture, Func<MiruNextDbContext, TReturn> func) => 
        fixture.WithDb(func);

    public static MiruNextFabricator Fab(this ITestFixture fixture) => 
        fixture.Get<MiruNextFabricator>();
}