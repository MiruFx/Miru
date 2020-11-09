using System;
using Alba;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Hosting;
using Miru.Urls;
using NUnit.Framework;

namespace Miru.Tests.Urls
{
    public class RouteTest : IDisposable
    {
        private readonly SystemUnderTest _system = new SystemUnderTest(new MiruTestWebHost(MiruHost.CreateMiruHost()).GetConfiguredHostBuilder());
        private UrlLookup UrlLookup => _system.Services.GetService<UrlLookup>();

        public void Dispose() => _system?.Dispose();

        [Test]
        public void Action_returns_xml()
        {
            _system.Scenario(_ =>
            {
                _.Get
                    .Url(UrlLookup.For(new OrdersCancel.Query {Id = 1}))
                    .Accepts("application/xml, text/xml");

                _.ContentTypeShouldBe("application/xml; charset=utf-8");

                _.StatusCodeShouldBeOk();

                _.ContentShouldContain("<OrderId>1</OrderId>");
                _.ContentShouldContain("<OrderName>");
            });
        }
        
        [Test]
        public void Action_returns_html()
        {
            _system.Scenario(_ =>
            {
                _.Get
                    .Url(UrlLookup.For(new OrdersCancel.Query {Id = 1}))
                    .Accepts("text/html");

                _.ContentTypeShouldBe("text/html; charset=utf-8");

                _.StatusCodeShouldBeOk();

                _.ContentShouldContain("<p>1, Order 1</p>");
            });
        }
        
        [Test]
        public void Action_returns_js_view_from_post()
        {
            _system.Scenario(_ =>
            {
                var request = new OrdersCancel.Command()
                {
                    OrderId = 1,
                    OrderName = "Order 1"
                };

                _.Post
                    .FormData(request)
                    .ToUrl(UrlLookup.For<OrdersCancel.Command>())
                    .Accepts("text/javascript");

                _.SetRequestHeader("X-Requested-With", "XMLHttpRequest");

                _.ContentTypeShouldBe("text/javascript");

                _.StatusCodeShouldBeOk();

                _.ContentShouldContain("$(\"body\").replaceWith('<p>1, True</p>');");
            });
        }
        
        [Test]
        public void Should_map_query_command_action()
        {
            _system.Scenario(_ =>
            {
                _.Get
                    .Url(UrlLookup.For<OrdersCancel.Query>())
                    .Accepts("application/xml, text/xml");
                
                _.StatusCodeShouldBeOk();
                
            });
        }
    }
}