using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using Bogus;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.MemoryStorage.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Databases;
using Miru.Databases.Migrations;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Queuing;
using Miru.Storages;
using Miru.Userfy;
using Shouldly;

namespace Miru.Testing
{
    /// <summary>
    /// API that contains services to auxiliate test's fixtures. Manipulations with Services should be Scoped
    /// </summary>
    public static class TestFixtureExtensions
    {
        public static ScopedServices WithScope(this ITestFixture fixture)
        {
            return fixture.Get<IMiruApp>().WithScope();
        }
        
        public static async Task<TResult> SendAsync<TResult>(this ITestFixture fixture, IRequest<TResult> message)
        {
            return await fixture.App.SendAsync(message);
        }
        
        public static TResult SendSync<TResult>(this ITestFixture fixture, IRequest<TResult> message)
        {
            return fixture.App.SendSync(message);
        }
        
        public static void Save(this ITestFixture fixture, params object[] entities)
        {
            ThrowExceptionIfNotEntities(entities);
            
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<IDataAccess>();
                
                db.SaveAsync(entities).Wait();
            }
        }
        
        public static TEntity Save<TEntity>(this ITestFixture fixture, TEntity entity)
        {
            using var scope = fixture.App.WithScope();
            
            var db = scope.Get<IDataAccess>();
                
            db.SaveAsync(new object[] {entity}).GetAwaiter().GetResult();

            return entity;
        }

        public static async Task SaveAsync(this ITestFixture fixture, params object[] entities)
        {
            ThrowExceptionIfNotEntities(entities);
            
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<IDataAccess>();
                
                await db.SaveAsync(entities);
            }
        }
        
        public static async Task SaveAddingAsync(this ITestFixture fixture, params object[] entities)
        {
            ThrowExceptionIfNotEntities(entities);

            using var scope = fixture.App.WithScope();
            
            var db = scope.Get<DbContext>();

            await using var tx = await db.Database.BeginTransactionAsync();
            
            foreach (var entity in entities)
            {
                db.Add(entity);
            }

            await db.SaveChangesAsync();
            await tx.CommitAsync();
        }
        
        public static void Update<TEntity>(this ITestFixture fixture, TEntity entity)
            where TEntity : Entity
        {
            var scope = fixture.App.WithScope();
            
            var db = scope.Get<DbContext>();
                
            if (entity.IsNotNew())
            {
                var entry = db.Entry(entity);
            
                if (entry.State == EntityState.Detached)
                    db.Attach(entity);
            
                entry.State = EntityState.Modified;
            }

            db.SaveChanges();
        }
        
        public static async Task<TScenario> ScenarioAsync<TScenario>(this ITestFixture fixture) where TScenario : IFixtureScenario, new()
        {
            MiruTest.Log.Information($"Loading Scenario {typeof(TScenario)}");
            
            var scenario = new TScenario();

            await scenario.BuildAsync(fixture);

            return scenario;
        }
        
        public static async Task SaveAsync<T>(this ITestFixture fixture, IEnumerable<T> entities) where T : IEntity
        {
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<IDataAccess>();
                
                await db.SaveAsync(entities.Cast<object>().ToArray());
            }
        }

        public static IStorage Storage(this ITestFixture fixture) => fixture.Get<IStorage>();
        
        private static void ThrowExceptionIfNotEntities(object[] entities)
        {
            // TODO: Make configurable if should throw exception
            foreach (var entity in entities)
            {
                if (entity is IEnumerable collectionOfEntities)
                {
                    foreach (var castEntity in collectionOfEntities)
                        if (castEntity is IEntity == false)
                            throw new MiruException(
                                $"Trying to persist objects but {entity} does not implement IEntity");
                }
                else
                {
                    if (entity is IEntity == false)
                        throw new MiruException($"Trying to persist objects but {entity} does not implement IEntity");
                }
            }
        }

        public static TReturn WithDb<TDbContext, TReturn>(
            this ITestFixture fixture, 
            Func<TDbContext, TReturn> func) where TDbContext : DbContext
        {
            using var scope = fixture.App.WithScope();
            
            var db = scope.Get<TDbContext>();
                
            return func(db);
        }
        
        public static void ExecDb<TDbContext>(
            this ITestFixture fixture, 
            Action<TDbContext> action)
        {
            using var scope = fixture.App.WithScope();
            
            var db = scope.Get<TDbContext>();
                
            action(db);
        }
        
        public static void LoginAs<TUser>(this ITestFixture fixture, TUser user) where TUser : UserfyUser
        {
            MiruTest.Log.Information($"Login as #{user.Id}-{user.Display}");

            using var scope = fixture.App.WithScope();

            TestingCurrentUser.User = user;
        }

        public static void Logout(this ITestFixture fixture)
        {
            MiruTest.Log.Information("Logging out the current user");

            using var scope = fixture.App.WithScope();
            
            scope.Get<IUserSession>().LogoutAsync().GetAwaiter().GetResult();
        }
        
        public static IServiceCollection AddTestingUserSession<TUser>(this IServiceCollection services) 
            where TUser : UserfyUser
        {
            return services
                .ReplaceTransient<IUserSession, TestingUserSession<TUser>>()
                .ReplaceTransient<IUserSession<TUser>, TestingUserSession<TUser>>()
                .ReplaceTransient<ICurrentUser, TestingCurrentUser>();
        }

        public static long CurrentUserId(this ITestFixture fixture)
        {
            using var scope = fixture.App.WithScope();
            
            return scope.Get<IUserSession>().CurrentUserId;
        }
        
        public static ITestFixture MigrateDatabase(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(MigrateDatabase)}()");
            
            using var scope = fixture.App.WithScope();
            
            scope.Get<IDatabaseMigrator>().UpdateSchema();
            
            return fixture;
        }
        
        public static ITestFixture ClearFabricator(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(ClearFabricator)}()");
            
            using var scope = fixture.App.WithScope();
            
            scope.Get<Fabricator>().Clear();
            
            return fixture;
        }
        
        public static ITestFixture ClearDatabase(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(ClearDatabase)}()");

            using var scope = fixture.App.WithScope();
            
            scope.Get<IDatabaseCleaner>().ClearAsync().GetAwaiter().GetResult();

            return fixture;
        }
        
        public static ITestFixture ClearQueue(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(ClearQueue)}()");

            using var scope = fixture.App.WithScope();
            
            scope.Get<IQueueCleaner>().Clear();
            
            return fixture;
        }
        
        public static EmailSent LastEmailSent(this ITestFixture fixture)
        {
            return fixture.Get<MemorySender>().Last();
        }
        
        public static IEnumerable<EmailSent> AllEmailsSent(this ITestFixture fixture)
        {
            return fixture.Get<MemorySender>().All();
        }
        
        public static ITestFixture StopServer(this ITestFixture fixture)
        {
            MiruTest.Log.Debug("Stopping App's Server");
            
            fixture.Get<IHost>().StopAsync().GetAwaiter().GetResult();
            fixture.Get<IHost>().Dispose();
            
            MiruTest.Log.Debug("App's Server stopped");
            
            return fixture;
        }

        public static ITestFixture StartServer2(this ITestFixture fixture)
        {
            var host2 = fixture.Get<IHost>();

            try
            {
                host2.Start();
            }
            catch (Exception exception)
            {
                throw new MiruTestConfigException(
                    "Could not start host for the App. Check the Inner Exception and your Program.cs/Startup.cs configurations",
                    exception.InnerException ?? exception);
            }

            // var server = fixture.Get<IServer>();
            //     
            // var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            //
            // if (addresses.Count == 0)
            //     throw new MiruTestConfigException(
            //         "The App's Server has no http addresses associated to it. Maybe the App is already running in another process?");
            
            return fixture;
        }
    }
}