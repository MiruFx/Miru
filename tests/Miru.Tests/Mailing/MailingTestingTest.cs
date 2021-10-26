using Microsoft.Extensions.DependencyInjection;
using Miru.Mailing;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Mailing
{
    public class MailingTestingTest
    {
        [Test]
        public void Can_replace_app_sender_to_test_sender()
        {
            // arrange
            var sp = TestServiceCollection.Build(services =>
            {
                // as in the app
                services.AddMailing()
                    .AddSmtpSender();
                
                // as in the test
                services.AddSenderMemory();
            });

            // assert
            sp.ServiceProvider.GetService<FluentEmail.Core.Interfaces.ISender>().ShouldBeOfType<MemorySender>();
        }
    }
}