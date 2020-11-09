using Microsoft.Extensions.DependencyInjection;

namespace Miru.Mailing
{
    public class MailingServiceBuilder
    {
        public IServiceCollection Services { get; }

        internal MailingServiceBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}