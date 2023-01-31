using System.Collections.Generic;
using System.Linq;
using Corpo.Skeleton.Domain;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Databases;
using Miru.Databases.Migrations;
using Miru.Fabrication;
using Miru.Testing;
using Miru.Tests;
using Miru.Userfy;
using NUnit.Framework;
using Shouldly;

namespace Miru.Postgres.Tests;

public class PostgresTest
{
    private TestFixture _;
        
    [OneTimeSetUp]
    public void SetupFixture()
    {
        _ = new ServiceCollection()
            .AddMiruApp()
            .AddFeatureTesting()
            .AddMiruCoreTesting()
            .AddFabrication()

            // postgres
            .AddEfCorePostgres<FooDbContext>()
            .AddDatabaseCleaner<PostgresDatabaseCleaner>()
            
            .Configure<DatabaseOptions>(opt => 
                opt.ConnectionString = "Server=localhost;Port=5432;Database=miru_test;User Id=postgres;Password=root;")
                
            .AddMigrator<Corpo.Skeleton.Startup>(mb => mb.AddPostgres92())
            .AddLogging()
                
            .BuildServiceProvider()
            .GetService<TestFixture>();    
        
        // drop all tables
        DropAllTables();
        
        _.MigrateDatabase();
    }

    [SetUp]
    public void Setup()
    {
        _.ClearDatabase();
    }

    [Test]
    public void Should_create_new_user()
    {
        // arrange
        var user = _.Make<User>(x => x.LockoutEnd = null);
            
        // act
        _.Save(user);
            
        // assert
        var saved = _.App.WithScope(s => s.Get<FooDbContext>().Users.First());
        saved.Email.ShouldBe(user.Email);
    }
    
    private void DropAllTables()
    {
        _.WithDb<FooDbContext>(db =>
        {
            db.Database.ExecuteSqlRaw(@"
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO public;
COMMENT ON SCHEMA public IS 'standard public schema';");
        });
    }

    public class FooDbContext : UserfyDbContext<User>
    {
        public FooDbContext(
            DbContextOptions options, 
            IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
        {
        }
    }
}