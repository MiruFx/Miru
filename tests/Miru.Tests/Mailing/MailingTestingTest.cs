using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Mailing;
using Miru.Queuing;
using Miru.Storages;
using Miru.Testing;
using Miru.Tests.Queuing;
using Miru.Urls;
using Mong.Tests.Config;
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
                    .AddSenderSmtp();
                
                // as in the test
                services.AddSenderMemory();
            });

            // assert
            sp.ServiceProvider.GetService<FluentEmail.Core.Interfaces.ISender>().ShouldBeOfType<MemorySender>();
        }
    }
}