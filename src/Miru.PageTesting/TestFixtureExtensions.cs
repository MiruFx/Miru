using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Miru.Testing;

namespace Miru.PageTesting
{
    public static class TestFixtureExtensions
    {
        /// <summary>
        /// Try to execute an Action swallowing but logging exceptions
        /// </summary>
        public static TestFixture Try(this TestFixture fixture, Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                MiruTest.Log.Error("Exception occured while trying to execute a code in a test. The Exception has been caught and handled", exception);
            }

            return fixture;
        }
    }
}