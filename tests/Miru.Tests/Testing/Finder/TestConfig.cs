using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
using Miru.Databases.EntityFramework;
using Miru.Databases.Migrations;
using Miru.Domain;
using Miru.Foundation.Hosting;
using Miru.Foundation.Logging;
// using Miru.Testing;
// using Miru.Userfy;
// using OpenQA.Selenium.Chrome;
// using OpenQA.Selenium.Remote;
// using Shoppers;
// using Shoppers.Config;
// using Shoppers.Domain;

namespace Miru.Tests.Testing.Finder
{
    // TODO: Fix
    
    // public class ShoppersTestConfig : MiruTestConfig
    // {
    //     public ShoppersTestConfig()
    //     {
    //         Services.AddSqlServerReset();
    //     
    //         Services.AddSingleton<IDatabaseReset, SqlServerDatabaseReset>();
    //         Services.AddScoped<IDataAccess, EntityFrameworkDataAccess>();
    //
    //         Services.AddSingleton<TestFixture, TestFixture>();
    //
    //         Services.AddSingleton<ISessionStore, MemorySessionStore>();
    //
    //         Services.AddSingleton<IPaymentProcessor, TestPaymentProcessor>();
    //
    //         Services.AddSingleton<IUserSession, TestingUserSession>();
    //         
    //         Services.AddSingleton<RemoteWebDriver>(
    //             new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
    //
    //         BeforeSuite(_ =>
    //         {
    //             Console.WriteLine("BeforeSuite Thread Id: " + Thread.CurrentThread.ManagedThreadId);
    //             _.App.Get<IDatabaseMigrator>().UpdateSchema();
    //         });
    //         
    //         BeforeEach<IFeatureTest>(_ =>
    //         {
    //             _.App.Get<IDatabaseReset>().ResetDatabase();
    //         });
    //
    //         BeforeAll<ICasePerFeatureTest>(_ =>
    //         {
    //             _.App.Get<IDatabaseReset>().ResetDatabase();
    //         });
    //     }
    // }
}
