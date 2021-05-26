using System;
using System.Net;
using System.Net.Mail;
using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Miru.Mailing
{
    public static class MailingServiceCollectionExtensions
    {
        public static IServiceCollection AddMailing(
            this IServiceCollection services,
            Action<MailingOptions> setupAction = null)
        {
            if (setupAction != null)
                services.Configure(setupAction);
            
            services.AddTransient<IMailer, FluentEmailMailer>();

            FluentEmail.Core.Email.DefaultSender = null;

            services.AddSingleton(sp => sp.GetService<IOptions<MailingOptions>>().Value);

            services.AddTransient<RazorViewToStringRenderer>();

            return services;
        }

        public static IServiceCollection AddSenderStorage(this IServiceCollection services)
        {
            services.AddScoped<ISender, SaveStorageSender>();

            return services;
        }
        
        public static IServiceCollection AddSenderSmtp(this IServiceCollection services)
        {
            services.AddSingleton<ISender>(sp =>
            {
                var smtpOptions = sp.GetService<MailingOptions>().Smtp;

                return new SmtpSender(new SmtpClient(smtpOptions.Host, smtpOptions.Port)
                {
                    EnableSsl = smtpOptions.Ssl,
                    Credentials = new NetworkCredential(smtpOptions.UserName, smtpOptions.Password)
                });
            });

            return services;
        }
    }
}