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
using Microsoft.Extensions.Hosting;
using Miru.Databases;
using Miru.Databases.Migrations;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Queuing;
using Miru.Userfy;
using Shouldly;

namespace Miru.Testing
{
    /// <summary>
    /// API that contains services to auxiliate test's fixtures. Manipulations with Services should be Scoped
    /// </summary>
    public static class TestFixtureExtensions
    {
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
                
                db.PersistAsync(entities).Wait();
            }
        }
        
        public static TEntity Save<TEntity>(this ITestFixture fixture, TEntity entity)
        {
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<IDataAccess>();
                
                db.PersistAsync(new object[] { entity }).GetAwaiter().GetResult();
            }

            return entity;
        }

        public static async Task SaveAsync(this ITestFixture fixture, params object[] entities)
        {
            ThrowExceptionIfNotEntities(entities);
            
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<IDataAccess>();
                
                await db.PersistAsync(entities);
            }
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
                
                await db.PersistAsync(entities.Cast<object>().ToArray());
            }
        }

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

        public static TReturn WithDb<TDbContext, TReturn>(this ITestFixture fixture, Func<TDbContext, TReturn> func)
        {
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<TDbContext>();
                
                return func(db);
            }
        }
        
        public static void LoginAs(this ITestFixture fixture, IUser user)
        {
            // FIXME
            // MiruTest.Log.Information($"Login as #{user.Id}-{user.Display}");
            MiruTest.Log.Information($"Login as #___-{user.Display}");

            fixture.Get<IUserSession>().Login(user);
        }

        public static void Logout(this ITestFixture fixture)
        {
            MiruTest.Log.Information("Logging out the current user");
            
            fixture.Get<IUserSession>().Logout();
        }
        
        public static TUser CurrentUser<TUser>(this ITestFixture fixture) where TUser : IUser
        {
            using (var scope = fixture.App.WithScope())
            {
                return scope.Get<IUserSession<TUser>>().GetUserAsync().GetAwaiter().GetResult();
            }
        }
        
        public static long CurrentUserId(this ITestFixture fixture)
        {
            return fixture.Get<IUserSession>().CurrentUserId;
        }
        
        public static ITestFixture MigrateDatabase(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(MigrateDatabase)}()");
            
            fixture.Get<IDatabaseMigrator>().UpdateSchema();
            
            return fixture;
        }
        
        public static ITestFixture ClearFabricator(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(ClearFabricator)}()");
            
            fixture.Get<Fabricator>().Clear();
            
            return fixture;
        }
        
        public static ITestFixture ClearDatabase(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(ClearDatabase)}()");

            using var scope = fixture.App.WithScope();
            
            scope.Get<IDatabaseCleaner>().ClearAsync();

            return fixture;
        }
        
        public static ITestFixture ClearQueue(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(ClearQueue)}()");

            fixture.Get<IQueueCleaner>().Clear();
            
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