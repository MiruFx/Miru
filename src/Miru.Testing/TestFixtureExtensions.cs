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
        public static async Task<TResult> Send<TResult>(this ITestFixture fixture, IRequest<TResult> message)
        {
            return await fixture.App.Send(message);
        }
        
        public static TResult SendSync<TResult>(this ITestFixture fixture, IRequest<TResult> message)
        {
            return fixture.App.SendSync(message);
        }
        
        public static void SaveSync(this ITestFixture fixture, params object[] entities)
        {
            ThrowExceptionIfNotEntities(entities);
            
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<IDataAccess>();
                
                db.PersistAsync(entities).Wait();
            }
        }
        
        public static TEntity SaveSync<TEntity>(this ITestFixture fixture, TEntity entity)
        {
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<IDataAccess>();
                
                db.PersistAsync(new object[] { entity }).GetAwaiter().GetResult();
            }

            return entity;
        }

        public static async Task Save(this ITestFixture fixture, params object[] entities)
        {
            ThrowExceptionIfNotEntities(entities);
            
            using (var scope = fixture.App.WithScope())
            {
                var db = scope.Get<IDataAccess>();
                
                await db.PersistAsync(entities);
            }
        }
        
        public static async Task<TScenario> Scenario<TScenario>(this ITestFixture fixture) where TScenario : IFixtureScenario, new()
        {
            MiruTest.Log.Information($"Loading Scenario {typeof(TScenario)}");
            
            var scenario = new TScenario();

            await scenario.Build(fixture);

            return scenario;
        }
        
        public static async Task Save<T>(this ITestFixture fixture, IEnumerable<T> entities) where T : IEntity
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
            MiruTest.Log.Information($"Login as #{user.Id}-{user.Display}");

            fixture.Get<IUserSession>().Login(user);
        }

        public static void Logout(this ITestFixture fixture)
        {
            MiruTest.Log.Information("Logging out the current user");
            
            fixture.Get<IUserSession>().Logout();
        }
        
        public static TUser CurrentUser<TUser>(this ITestFixture fixture) where TUser : IUser
        {
            return fixture.Get<IUserSession<TUser>>().User().GetAwaiter().GetResult();
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
            
            scope.Get<IDatabaseCleaner>().Clear();

            return fixture;
        }
        
        public static ITestFixture ClearQueue(this ITestFixture fixture)
        {
            MiruTest.Log.Information($"Running _.{nameof(ClearQueue)}()");
            
            var storage = fixture.Get<JobStorage>().As<MemoryStorage>();
            
            var jobs = storage.Data.GetEnumeration<JobDto>();
            
            storage.Data.Delete(jobs);

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
        
        public static int EnqueuedCount(this ITestFixture fixture)
        {
            return fixture.Get<JobStorage>()
                .As<MemoryStorage>()
                .Data
                .GetEnumeration<JobDto>()
                .Count();
        }

        public static bool EnqueuedOneJobFor<TJob>(this ITestFixture fixture) where TJob : IJob
        {
            return fixture.Get<JobStorage>()
                .As<MemoryStorage>()
                .Data
                .GetEnumeration<JobDto>()
                .Count(job => job.InvocationData.Contains(typeof(TJob).FullName!)) == 1;
        }
        
        public static string EnqueuedRawJob<TJob>(this ITestFixture fixture) where TJob : IJob
        {
            var job = fixture.Get<JobStorage>().As<MemoryStorage>()
                .Data
                .GetEnumeration<JobDto>()
                .FirstOrDefault(job => job.InvocationData.Contains(typeof(TJob).FullName!));
            
            if (job == null)
                throw new ShouldAssertException($"No job queued found of type {typeof(TJob).FullName}");

            return job.InvocationData;
        }
        
        public static IEnumerable<string> EnqueuedRawJobs<TJob>(this ITestFixture fixture) where TJob : IJob
        {
            var jobs = fixture.Get<JobStorage>().As<MemoryStorage>()
                .Data
                .GetEnumeration<JobDto>()
                .Where(job => job.InvocationData.Contains(typeof(TJob).FullName!))
                .Select(job => job.InvocationData)
                .ToList();
            
            if (jobs.Count == 0)
                throw new ShouldAssertException($"No job queued found of type {typeof(TJob).FullName}");

            return jobs;
        }
    }
}