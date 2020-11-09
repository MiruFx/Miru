using System;
using Miru;
using Miru.Fabrication;
using Miru.Testing;
using Mong.Database;
using NUnit.Framework;
using Shouldly;

namespace Mong.Tests
{
    public static class Extensions
    {
        public static TReturn Db<TReturn>(this ITestFixture fixture, Func<MongDbContext, TReturn> func)
        {
            return fixture.WithDb(func);
        }
        
        public static MongFabricator Fab(this ITestFixture fixture)
        {
            return fixture.Get<MongFabricator>();
        }
    }
}