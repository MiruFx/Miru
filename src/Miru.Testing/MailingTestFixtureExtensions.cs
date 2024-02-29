using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core.Models;
using Miru.Core;
using Miru.Mailing;
using Shouldly;

namespace Miru.Testing;

[ShouldlyMethods]
public static class MailingTestFixtureExtensions
{
    public static Email EnqueuedEmail(this ITestFixture fixture)
    {
        var mailingQueue = fixture.Get<MailingOptions>().QueueName;
        var job = fixture.EnqueuedFor<EmailJob>(mailingQueue);

        if (job == null)
            throw new MiruException($"There is no EmailJob in the '{mailingQueue}' queue");
            
        return job.Email;
    }
        
    public static IEnumerable<Email> EnqueuedEmails(
        this ITestFixture fixture, 
        string queue = "default") =>
        fixture
            .AllEnqueuedFor<EmailJob>(queue: queue)
            .Select(x => x.Email);
    
    public static async Task SendEmailNowAsync<TMail>(this ITestFixture fixture, TMail mail) 
        where TMail : Mailable
    {
        using (var scope = fixture.App.WithScope())
        {
            var mailer = scope.Get<IMailer>();
            await mailer.SendNowAsync(mail);
        }
    }
        
    public static void ShouldContainEmail(this IEnumerable<Address> address, string email)
    {
        address.ShouldContain(
            m => m.EmailAddress == email, 
            $"Should contain {email} but there were: {address.Select(x => x.EmailAddress).Join(",")}");
    }

    public static void ShouldContain(
        this IList<Attachment> attachments,
        string name,
        string contentType) 
    {
        attachments
            .Any(x => x.Filename == name && x.ContentType == contentType)
            .ShouldBeTrue($"Should contain attachment named '{name}' with content type '{contentType}'");
    }
        
    public static ITestFixture ClearEmails(this ITestFixture fixture)
    {
        using var scope = fixture.App.WithScope();

        scope.Get<MemorySender>().Clear();

        return fixture;
    }
    
    public static void ShouldContainAddress(
        this IList<FluentEmail.Core.Models.Address> addresses, 
        FluentEmail.Core.Models.Address address) =>
        addresses.ShouldContain(m => m.Name == address.Name && m.EmailAddress == address.EmailAddress);
    
    
    public static void ShouldBe(this FluentEmail.Core.Models.Address address, string emailAddress)
    {
        address.EmailAddress.ShouldBe(emailAddress);
    }
}