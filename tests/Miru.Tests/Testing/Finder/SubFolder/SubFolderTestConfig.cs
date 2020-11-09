using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
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

namespace Miru.Tests.Testing.Finder.SubFolder
{
    // TODO: Fix
    
    // public class SubFolderTestConfig : MiruTestConfig
    // {
    //     public SubFolderTestConfig()
    //     {
    //         BeforeSuite(_ =>
    //         {
    //             Console.WriteLine("BeforeSuite Thread Id: " + Thread.CurrentThread.ManagedThreadId);
    //             _.App.Get<IDatabaseMigrator>().UpdateSchema();
    //         });
    //         
    //         BeforeEach<IFeatureTest>(_ =>
    //         {
    //             // FakeKeeper.Clear();
    //             _.App.Get<IDatabaseReset>().ResetDatabase();
    //         });
    //
    //         BeforeAll<ICasePerFeatureTest>(_ =>
    //         {
    //             // FakeKeeper.Clear();
    //             _.App.Get<IDatabaseReset>().ResetDatabase();
    //         });
    //     }
    // }
}
