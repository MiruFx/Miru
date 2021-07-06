using System;
using System.Threading.Tasks;
using FluentEmail.Core.Models;
using FluentValidation;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Foundation.Hosting;
using Miru.Foundation.Logging;
using Miru.Mailing;
using Miru.Queuing;
using Miru.Storages;
using Miru.Testing;
using Miru.Urls;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Mailing
{
    public class MailingTest
    {
        private readonly MiruTestWebHost _host;
        
        private readonly IMailer _mailer;
        private readonly MemorySender _emailsSent;
        private readonly IServiceProvider _sp;
        private readonly ITestFixture _;

        public MailingTest()
        {
            _host = new MiruTestWebHost(MiruHost.CreateMiruHost(), 
                services =>
                {
                    services
                        .AddStorage()
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
                        .AddMiruTestFixture()
                        .AddSenderMemory()
                        .AddSingleton<MiruSolution, MiruTestSolution>()
                        .AddQueuing((sp, cfg) => cfg.UseMemoryStorage())
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
            
            emailSent.Data.FromAddress.EmailAddress.ShouldBe("mailing@test.com");
            emailSent.Data.ReplyToAddresses[0].EmailAddress.ShouldBe("noreply@test.com");
        }
        
        [Test]
        public void Email_with_custom_parameters()
        {
            // arrange
            var user = new User
            {
                Email = "bill@gates.com", 
                Name = "Bill Gates"
            };
            
            // act
            _mailer.SendNowAsync(new ConfirmMail(user));
            
            // assert
            var emailSent = _emailsSent.Last();
            
            emailSent.Data.ToAddresses[0].EmailAddress.ShouldBe("bill@gates.com");
            emailSent.Data.Subject.ShouldBe("Welcome Bill Gates");
            emailSent.Data.Body.ShouldContain("Hi Bill Gates");
            emailSent.Data.Body.ShouldContain("Confirm your email clicking");
        }
        
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
            
            emailSent.Data.ToAddresses[0].EmailAddress.ShouldBe("bill@gates.com");
            emailSent.Data.Subject.ShouldBe("Welcome Bill Gates");
            emailSent.Data.Body.ShouldContain("<h1>Welcome, Bill Gates!</h1>");
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
        
        public class User
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }
    }
}