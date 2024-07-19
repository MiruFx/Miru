using System;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Logging;
using Miru.Hosting;
using Miru.Mailing;
using Miru.Storages;
using Miru.Urls;

namespace Miru.Tests.Mailing;

public class MailingLater
{
    private readonly MiruTestWebHost _host;
        
    private readonly IMailer _mailer;
    private readonly MemorySender _emailsSent;
    private readonly IServiceProvider _sp;
    private readonly ITestFixture _;

    public MailingLater()
    {
        _host = new MiruTestWebHost(MiruHost.CreateMiruHost(), services =>
            {
                services
                    .AddAppTestStorage()
                    .AddMiruMvc()
                    .AddMiruUrls()
                    .AddLogging()
                    .AddSerilogConfig()
                    .AddSingleton<IUrlMaps, StubUrlMaps>()
                    .AddMailing(options =>
                    {
                        options.EnqueueIn("email");
                        
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
                    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<MailingTest>());
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
    [Ignore("FIX")]
    public void Should_enqueue_email_with_attachment()
    {
    }
    
    [Test]
    public async Task Should_enqueue_to_configured_queue_name()
    {
        // act
        await _mailer.SendLaterAsync(new ConfirmMail(new User()));
            
        // assert
        var enqueuedEmails = _.EnqueuedEmails(queue: "email");
            
        enqueuedEmails.ShouldCount(1);
        enqueuedEmails.First().FromAddress.EmailAddress.ShouldBe("mailing@test.com");
        enqueuedEmails.First().ReplyToAddresses[0].EmailAddress.ShouldBe("noreply@test.com");
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