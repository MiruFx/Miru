using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
using Miru.Databases.Migrations;
using Miru.Databases.Migrations.FluentMigrator;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Databases
{
    // TODO: fix when support Sqlite
    // public class EfCoreSqliteConfigTest
    // {
    //     private ServiceProvider _sp;
    //
    //     [SetUp]
    //     public void Setup()
    //     {
    //         _sp = new ServiceCollection()
    //             .AddSingleton(new AppSettings() { Database = new Database() { ConnectionString = "DataSource=Mong_dev.db" } })
    //             .AddEfCoreSqlite<MongDbContext>()
    //             .BuildServiceProvider();
    //     }
    //     
    //     [Test]
    //     public void Db_context_provider_should_be_sqlite()
    //     {
    //         _sp.GetService<MongDbContext>().Database.ProviderName.ShouldBe("Microsoft.EntityFrameworkCore.Sqlite");
    //     }
    //     
    //     [Test]
    //     public void Fluent_migration_database_type_should_be_sqlite()
    //     {
    //         _sp.GetService<IMigrationRunner>().Processor.DatabaseType.ShouldBe("SQLite");
    //     }
    //     
    //     [Test]
    //     public void Database_migrator_should_be_fluent_database_migrator()
    //     {
    //         _sp.GetService<IDatabaseMigrator>().ShouldBeOfType<FluentDatabaseMigrator>();
    //     }
    //     
    //     [Test]
    //     public void Db_context_should_be_scoped()
    //     {
    //         MongDbContext db;
    //         
    //         using (var scope = _sp.CreateScope())
    //         {
    //             db = scope.ServiceProvider.GetService<MongDbContext>();
    //             
    //             scope.ServiceProvider.GetService<MongDbContext>().ShouldBe(db);
    //         }
    //         
    //         using (var scope = _sp.CreateScope())
    //         {
    //             var otherDb = scope.ServiceProvider.GetService<MongDbContext>();
    //             
    //             scope.ServiceProvider.GetService<MongDbContext>().ShouldNotBe(db);
    //         }
    //     }
    // }
}