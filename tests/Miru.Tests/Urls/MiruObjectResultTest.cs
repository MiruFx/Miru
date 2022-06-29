using System;
using System.Net.Http;
using Alba;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Hosting;
using Miru.Urls;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Urls;

public class MiruObjectResultTest : IDisposable
{
    private readonly AlbaHost _system = new AlbaHost(
        new MiruTestWebHost(MiruHost.CreateMiruHost()).GetConfiguredHostBuilder());
        
    private UrlLookup UrlLookup => _system.Services.GetService<UrlLookup>();

    public void Dispose() => _system?.Dispose();

    [Test]
    public void Action_returning_xml()
    {
        _system.Scenario(_ =>
        {
            _.Get
                .Url(UrlLookup.For(new OrdersCancel.Query {Id = 1}))
                .Accepts("application/xml, text/xml");
        
            _.StatusCodeShouldBeOk();
            _.ContentTypeShouldBe("application/xml; charset=utf-8");
            _.ContentShouldContain("<OrderId>1</OrderId>");
            _.ContentShouldContain("<OrderName>");
        });
    }
        
    [Test]
    public void Action_returning_html()
    {
        _system.Scenario(_ =>
        {
            _.Get
                .Url(UrlLookup.For(new OrdersCancel.Query {Id = 1}))
                .Accepts("text/html");

            _.StatusCodeShouldBeOk();
            _.ContentTypeShouldBe("text/html; charset=utf-8");
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

            _.WithRequestHeader("X-Requested-With", "XMLHttpRequest");
                
            _.Post
                .FormData(request)
                .ToUrl(UrlLookup.For<OrdersCancel.Command>())
                .Accepts("text/javascript");

            _.StatusCodeShouldBeOk();
            _.ContentTypeShouldBe("text/javascript");
            _.ContentShouldContain("$(\"body\").replaceWith('<p>1, True</p>');");
        });
    }
        
    [Test]
    public void Action_returning_json()
    {
        _system.Scenario(_ =>
        {
            _.Get
                .Url(UrlLookup.For(new OrdersCancel.Query {Id = 1}))
                .Accepts("application/json");

            _.StatusCodeShouldBeOk();
            _.ContentTypeShouldBe("application/json; charset=utf-8");
            _.ContentShouldContain("{\"orderId\":1,\"orderName\":\"Order 1\"}");
        });
    }
}