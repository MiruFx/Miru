using System;
using FluentValidation;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Logging;
using Miru.Hosting;
using Miru.Mailing;
using Miru.Queuing;
using Miru.Storages;
using Miru.Urls;

namespace Miru.Tests.Mailing;

public class MailingTest
{
    private readonly MiruTestWebHost _host;
        
    private readonly IMailer _mailer;
    private readonly MemorySender _emailsSent;
    private readonly IServiceProvider _sp;
    private readonly ITestFixture _;

    public MailingTest()
    {
        _host = new MiruTestWebHost(MiruHost.CreateMiruHost(), services =>
            {
                services
                    .AddTestStorage()
                    .AddMiruMvc()
                    .AddMiruUrls()
                    .AddLogging()
                    .AddSerilogConfig()
                    .AddSingleton<IUrlMaps, StubUrlMaps>()
                    .AddMailing(options =>
                    {
                        options.EmailDefaults(email =>
                        {
                            email.From("mailing@test.com", "Mailing Test");
                            email.ReplyTo("noreply@test.com");
                        });
                            
                        options.AppUrl = "http://www.contoso.com";
                        options.TemplatePath = new SolutionFinder().FromCurrentDir().Solution.AppTestsDir;
                    })
                    .AddMiruCoreTesting()
                    .AddSenderMemory()
                    .AddSingleton<MiruSolution, MiruTestSolution>()
                    .AddQueuing(x => x.Configuration.UseMemoryStorage())
                    .AddMediatR(typeof(MailingTest).Assembly);
            });
            
        _sp = _host.Services;
            
        _mailer = _sp.GetService<IMailer>();
            
        _emailsSent = _sp.GetService<MemorySender>();

        _ = _sp.GetService<ITestFixture>();
    }

    [SetUp]
    public void Setup()
    {
        _emailsSent.Clear();
    }
        
    [Test]
    public async Task Should_set_default_email_options()
    {
        // act
        await _mailer.SendNowAsync(new ConfirmMail(new User()));
            
        // assert
        var emailSent = _emailsSent.Last();
            
        emailSent.FromAddress.EmailAddress.ShouldBe("mailing@test.com");
        emailSent.ReplyToAddresses[0].EmailAddress.ShouldBe("noreply@test.com");
    }
        
    [Test]
    public async Task Email_with_custom_parameters()
    {
        // arrange
        var user = new User
        {
            Email = "bill@gates.com", 
            Name = "Bill Gates"
        };
            
        // act
        await _mailer.SendNowAsync(new ConfirmMail(user));
            
        // assert
        var emailSent = _emailsSent.Last();
            
        emailSent.ToAddresses[0].EmailAddress.ShouldBe("bill@gates.com");
        emailSent.Subject.ShouldBe("Welcome Bill Gates");
        emailSent.Body.ShouldContain("Hi Bill Gates");
        emailSent.Body.ShouldContain("Confirm your email clicking");
    }
       
    [Test]
    public async Task Email_with_attachment()
    {
        // arrange
        var user = new User { Email = "bill@gates.com", Name = "Bill Gates" };
        
        (_.Storage().App / "attachment.txt").MakeFake();
            
        // act
        await _mailer.SendNowAsync(new EmailWithAttachment(user, _.Storage()));
            
        // assert
        var emailSent = _emailsSent.Last();
            
        emailSent.ToAddresses.ShouldContainEmail("bill@gates.com");
        emailSent.Subject.ShouldBe("Welcome Bill Gates");
        emailSent.Body.ShouldContain("Check your attachment");
        emailSent.Attachments.ShouldContain("attachment.txt", "text/plain");
    }
    
    // [Test]
    // public async Task Email_directly_without_mailable()
    // {
    //     // arrange
    //     var email = new Email()
    //         .To("paul@beatles.com")
    //         .Subject("Abbey Road")
    //         .Body("Lets go?");
    //         
    //     // act
    //     await _mailer.SendNowAsync(new Mailable());
    //         
    //     // assert
    //     var lastEmail = _emailsSent.Last();
    //         
    //     lastEmail.FromAddress.EmailAddress.ShouldBe("mailing@test.com");
    //     lastEmail.ToAddresses.ShouldContainEmail("paul@beatles.com");
    //     lastEmail.Subject.ShouldBe("Abbey Road");
    //     lastEmail.Body.ShouldContain("Lets go?");
    // }
        
    [Test]
    public async Task When_sending_email_should_throw_exception_if_email_from_is_not_set()
    {
        // arrange
        // act
        await Should.ThrowAsync<ValidationException>(() => _mailer.SendNowAsync(new LackFromMail()));
            
        // assert
        _emailsSent.All().ShouldBeEmpty();
    }
        
    [Test]
    public async Task When_queueing_email_should_throw_exception_if_email_from_is_not_set()
    {
        // arrange
        // act
        await Should.ThrowAsync<ValidationException>(() => _mailer.SendLaterAsync(new LackFromMail()));
            
        // assert
        _.EnqueuedEmails().ShouldBeEmpty();
    }
        
    [Test]
    public async Task When_queueing_email_should_throw_exception_if_to_from_is_not_set()
    {
        // arrange
        // act
        await Should.ThrowAsync<ValidationException>(() => _mailer.SendLaterAsync(new LackToMail()));
            
        // assert
        _.EnqueuedEmails().ShouldBeEmpty();
    }
        
    [Test]
    public async Task Should_throw_exception_if_email_to_is_not_set()
    {
        // arrange
        // act
        await Should.ThrowAsync<ValidationException>(() => _mailer.SendNowAsync(new LackToMail()));
            
        // assert
        _emailsSent.All().ShouldBeEmpty();
    }
        
    [Test]
    [Ignore("Can't find WelcomeMail.cshtml")]
    public async Task Email_with_razor_template()
    {
        // arrange
        var user = new User
        {
            Email = "bill@gates.com", 
            Name = "Bill Gates"
        };
            
        // act
        await _mailer.SendNowAsync(new WelcomeMail(user));
            
        // assert
        var emailSent = _emailsSent.Last();
            
        emailSent.ToAddresses[0].EmailAddress.ShouldBe("bill@gates.com");
        emailSent.Subject.ShouldBe("Welcome Bill Gates");
        emailSent.Body.ShouldContain("<h1>Welcome, Bill Gates!</h1>");
    }
        
    [Test]
    public void Sender_registry_should_be_singleton()
    {
        var sender1 = _sp.GetService<FluentEmail.Core.Interfaces.ISender>();
        var sender2 = _sp.GetService<FluentEmail.Core.Interfaces.ISender>();
            
        sender1.ShouldBeOfType<MemorySender>();
        sender1.ShouldBe(sender2);
    }
        
    public class ConfirmMail : Mailable
    {
        private readonly User _user;

        public ConfirmMail(User user)
        {
            _user = user;
        }

        public override void Build(Email mail)
        {
            mail.To(_user.Email)
                .Subject($"Welcome {_user.Name}")
                .Body($@"
Hi {_user.Name},

Confirm your email clicking on the link below:

");
        }
    }
        
    public class EmptyMail : Mailable
    {
        public override void Build(Email mail)
        {
        }
    }
        
    public class LackToMail : Mailable
    {
        public override void Build(Email mail)
        {
        }
    }
        
    public class LackFromMail : Mailable
    {
        public override void Build(Email mail)
        {
            mail.From(string.Empty);
        }
    }
        
    public class WelcomeMail : Mailable
    {
        private readonly User _user;

        public WelcomeMail(User user)
        {
            _user = user;
        }

        public override void Build(Email mail)
        {
            mail.To(_user.Email)
                .Subject($"Welcome {_user.Name}")
                .Template("_Welcome", _user);
        }
    }
    
    public class EmailWithAttachment : Mailable
    {
        private readonly User _user;
        private readonly IStorage _storage;

        public EmailWithAttachment(User user, IStorage storage)
        {
            _user = user;
            _storage = storage;
        }

        public override async Task BuildAsync(Email mail)
        {
            mail.To(_user.Email)
                .Subject($"Welcome {_user.Name}")
                .Body("Check your attachment")
                .Attach("attachment.txt", await _storage.GetAsync("attachment.txt"));
        }
    }
        
    public class User
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}