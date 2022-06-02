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

        public static bool EnqueuedOneJobFor<TJob>(this ITestFixture fixture) where TJob : IMiruJob
        {
            return fixture.Get<JobStorage>()
                .GetMonitoringApi()
                .EnqueuedJobs("default", 0, 1000)
                .Select(result => result.Value)
                .Count(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TJob)) == 1;
        }
        
        public static TJob EnqueuedJob<TJob>(this ITestFixture fixture)
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
        
        public static IEnumerable<TJob> EnqueuedJobs<TJob>(this ITestFixture fixture) where TJob : IMiruJob =>
            fixture.Get<JobStorage>()
                .GetMonitoringApi()
                .EnqueuedJobs("default", 0, 1000)
                .Select(result => result.Value)
                .Where(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TJob))
                .Select(enqueueJob => (TJob) enqueueJob.Job.Args[0])
                .ToList();
        
        public static Email EnqueuedEmail(this ITestFixture fixture)
        {
            var job = fixture.EnqueuedJob<EmailJob>();

            if (job == null)
                throw new MiruException("There is no EmailJob queued");
            
            return job.Email;
        }
        
        public static IEnumerable<Email> EnqueuedEmails(this ITestFixture fixture) =>
            fixture.EnqueuedJobs<EmailJob>().Select(x => x.Email);
    }
}