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
using Miru.Mailing;
using Miru.Queuing;
using Miru.Userfy;
using Shouldly;

namespace Miru.Testing
{
    public static class QueueingTestFixtureExtensions
    {
        public static int EnqueuedCount(this ITestFixture fixture)
        {
            return fixture.Get<JobStorage>()
                .GetMonitoringApi()
                .EnqueuedJobs("default", 0, 1000)
                .Count;
        }

        public static bool AnyEnqueuedFor<TQueueable>(this ITestFixture fixture) 
            where TQueueable : IQueueable
        {
            return fixture.Get<JobStorage>()
                .GetMonitoringApi()
                .EnqueuedJobs("default", 0, 1000)
                .Select(result => result.Value)
                .Count(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TQueueable)) == 1;
        }
        
        public static TJob EnqueuedFor<TJob>(this ITestFixture fixture)
        {
            var entry = fixture.Get<JobStorage>()
                .GetMonitoringApi()
                .EnqueuedJobs("default", 0, 1000)
                .Select(result => result.Value)
                .FirstOrDefault(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TJob));
            
            if (entry == null)
                throw new ShouldAssertException($"No job queued found of type {typeof(TJob).FullName}");

            return (TJob) entry.Job.Args[0];
        }
        
        public static IEnumerable<TJob> AllEnqueuedFor<TJob>(
            this ITestFixture fixture,
            string queue = "default") where TJob : IQueueable =>
                fixture.Get<JobStorage>()
                    .GetMonitoringApi()
                    .EnqueuedJobs(queue, 0, 1000)
                    .Select(result => result.Value)
                    .Where(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TJob))
                    .Select(enqueueJob => (TJob) enqueueJob.Job.Args[0])
                    .ToList();
        
        public static Email EnqueuedEmail(this ITestFixture fixture)
        {
            var job = fixture.EnqueuedFor<EmailJob>();

            if (job == null)
                throw new MiruException("There is no EmailJob queued");
            
            return job.Email;
        }
        
        public static IEnumerable<Email> EnqueuedEmails(
            this ITestFixture fixture, 
            string queue = "default") =>
                fixture
                    .AllEnqueuedFor<EmailJob>(queue: queue)
                    .Select(x => x.Email);
    }
}