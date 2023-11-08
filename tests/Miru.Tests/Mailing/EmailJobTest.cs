using FluentEmail.Core.Models;
using FluentValidation;
using Miru.Mailing;
using Miru.Storages;

namespace Miru.Tests.Mailing;

[TestFixture]
public class EmailJobTest
{
    public class Getting_job_title
    {
        [Test]
        public void Should_return_subject_and_recipient()
        {
            new FeatureInfo(
                new EmailJob(
                    new Email()
                    {
                        Subject = "Welcome",
                        ToAddresses = new[] { new Address("bill@gates.com") }
                    }))
                .GetTitle()
                .ShouldBe("EmailJob?Subject=Welcome&To=bill@gates.com");
        }
    }
}