using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Miru.Tests.Databases
{
    public class FluntDatabaseMigratorTest
    {
        // TODO: Fix
        
        // [Test]
        // public void Can_run_migration()
        // {
        //     var sp = new ServiceCollection()
        //         .AddFluentMigratorCore()
        //         .ConfigureRunner(rb => rb
        //             .AddSqlServer2016()
        //             .WithGlobalConnectionString("Data Source=.\\sqlexpress;Initial Catalog=shoppers_test;Integrated Security=SSPI;")
        //             .ScanIn(typeof(ShoppersDbContext).Assembly).For.Migrations())
        //         .AddLogging(lb => lb.AddFluentMigratorConsole())
        //         .BuildServiceProvider(false);
        //     
        //     sp.GetService<IMigrationRunner>().MigrateUp();
        // }
    }
}