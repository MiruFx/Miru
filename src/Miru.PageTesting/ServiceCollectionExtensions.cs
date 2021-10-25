using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation;
using Miru.Storages;
using Miru.Testing;
using Miru.Urls;
using Miru.Userfy;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Miru.PageTesting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPageTesting<TUser>(
            this IServiceCollection services, Action<PageTestingConfig> setupAction = null)
                where TUser : UserfyUser
        {
            var config = new PageTestingConfig() { Services = services };
            
            if (setupAction != null)
            {
                setupAction(config);
                
                services.AddSingleton(config);
            }

            services.AddSingleton<PageTestFixture>();
            
            services.ReplaceSingleton<IUrlMaps, DefaultUrlMaps>();
            
            services.AddSingleton<IStartupFilter, PageTestingStartupFilter>();

            services.AddTransient<IUserSession, TestingUserSession<TUser>>();

            services.AddSingleton<PageBody>();
            
            services.AddSingleton(sp => new WebDriverWait(
                sp.GetService<RemoteWebDriver>(), 
                sp.GetService<PageTestingConfig>().TimeOut));

            return services;
        }
    }
}