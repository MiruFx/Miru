using System;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Miru.Tests.Mvc;

public class MvcServiceRegistryTest
{
    private IServiceProvider _sp;

    [SetUp]
    public void Setup()
    {
        _sp = new ServiceCollection()
            .AddMiruApp()
            .AddMiruMvc()
            .BuildServiceProvider();
    }

    [Test]
    public void Should_register_antiforgery_default_options()
    {
        var options = _sp.Get<IOptions<AntiforgeryOptions>>().Value;
        options.Cookie.Name.ShouldBe("csrf-token");
        options.HeaderName.ShouldBe("x-csrf-token");
    }
    
    [Test]
    public void Should_register_cookie_default_options()
    {
        var options = _sp.Get<IOptions<SessionOptions>>().Value;
        options.Cookie.Name.ShouldBe("app-session");
        options.Cookie.SecurePolicy.ShouldBe(CookieSecurePolicy.Always);
        options.Cookie.IsEssential.ShouldBeTrue();

        var tempOptions = _sp.Get<IOptions<CookieTempDataProviderOptions>>().Value;
        tempOptions.Cookie.Name.ShouldBe("app-temp");
    }
}