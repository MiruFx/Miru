using System;
using System.Threading.Tasks;
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
        private readonly MiruTestWebHost _host = new MiruTestWebHost(MiruHost.CreateMiruHost(), 
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
                    .AddSenderMemory()
                    .AddSingleton<MiruSolution, MiruTestSolution>()
                    .AddQueuing((sp, cfg) => cfg.UseMemoryStorage())
                    .AddMediatR(typeof(MailingTest).Assembly)
                    .BuildServiceProvider();
            });
        
        private readonly IMailer _mailer;
        private readonly MemorySender _emailsSent;
        private readonly IServiceProvider _sp;

        public MailingTest()
        {
            _sp = _host.Services;
            
            _mailer = _sp.GetService<IMailer>();
            
            _emailsSent = _sp.GetService<MemorySender>();
        }

        [Test]
        public void Email_with_default_parameters()
        {
            // act
            _mailer.SendNow(new NullMail());
            
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
            _mailer.SendNow(new ConfirmMail(user));
            
            // assert
            var emailSent = _emailsSent.Last();
            
            emailSent.Data.ToAddresses[0].EmailAddress.ShouldBe("bill@gates.com");
            emailSent.Data.Subject.ShouldBe("Welcome Bill Gates");
            emailSent.Data.Body.ShouldContain("Hi Bill Gates");
            emailSent.Data.Body.ShouldContain("Confirm your email clicking");
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
            await _mailer.SendNow(new WelcomeMail(user));
            
            // assert
            var emailSent = _emailsSent.Last();
            
            emailSent.Data.ToAddresses[0].EmailAddress.ShouldBe("bill@gates.com");
            emailSent.Data.Subject.ShouldBe("Welcome Bill Gates");
            emailSent.Data.Body.ShouldContain("<h1>Welcome, Bill Gates!</h1>");
        }
        
        [Test]
        public void Registered_services()
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
        
        public class NullMail : Mailable
        {
            public override void Build(Email mail)
            {
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
                    .Template(_user);
            }
        }
        
        public class User
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }
    }
}