using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Miru.Mailing;

namespace Miru.Testing
{
    public static class MailingExtensions
    {
        public static MailingServiceBuilder AddSendToMemory(this MailingServiceBuilder options)
        {
            options.Services.AddSingleton<MemorySender>();
            options.Services.AddSingleton<ISender>(sp => sp.GetService<MemorySender>());
            
            return options;
        }
    }
}