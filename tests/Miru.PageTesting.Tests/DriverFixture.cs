using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Miru.Core;
using Miru.Foundation;
using Miru.Foundation.Bootstrap;
using Miru.Html;
using Miru.PageTesting;
using Miru.PageTesting.Chrome;
using Miru.PageTesting.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Serilog;
using Serilog.Events;
using LogLevel = OpenQA.Selenium.LogLevel;

namespace Miru.PageTesting.Tests;

public class DriverFixture
{
    public static readonly Lazy<DriverFixture> Get = new(new DriverFixture());
        
    public WebDriver FirefoxDriver { get; private set; }
    public WebDriver ChromeDriver { get; private set; }
    public PageBody Page { get; set; }
        
    private readonly IWebHost _host;
    private string _indexHtml;
    // private string _otherHtml;
        
    private ElementNaming _elementNaming;
    private PageTestingConfig _config;
    private string _baseUrl;
    private ILogger<MiruNavigator> _logger;

    public DriverFixture()
    {
        _host = new WebHostBuilder()
            .UseKestrelAnyLocalPort()
            .ConfigureServices(services =>
            {
                services.AddRouting();
            })
            .Configure(ConfigureApp)
            .Build();

        Parallel.Invoke(
            () => _host.Start(),
            () => FirefoxDriver = new FirefoxDriver(new FirefoxOptions().Headless()),
            () => ChromeDriver = new ChromeDriver(new ChromeOptions().Headless()));

        _elementNaming = new ElementNaming();
        _config = new PageTestingConfig();
        _baseUrl = _host.Services!
            .GetRequiredService<IServer>()!
            .Features.Get<IServerAddressesFeature>()!
            .Addresses.First();

        var loggerFactory = LoggerConfigurations.CreateLoggerFactory(cfg =>
        {
            cfg.MinimumLevel.Override("Miru.PageTesting", LogEventLevel.Debug);
        });
            
        _logger = loggerFactory.CreateLogger<MiruNavigator>();
    }

    private void ConfigureApp(IApplicationBuilder app)
    {
        app.UseRouting();
            
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", context =>
            {
                context.Response.ContentType = "text/html";
                return context.Response.WriteAsync(_indexHtml);
            });

            // endpoints.MapGet("/other", context =>
            // {
            //     context.Response.ContentType = "text/html";
            //     return context.Response.WriteAsync(_otherHtml);
            // });
        });
    }

    public void HtmlIs(string html)
    {
        _indexHtml = html;
            
        Page.NavigateTo("/");
    }
        
    public void Dispose()
    {
        Parallel.Invoke(
            () => _host.StopAsync().GetAwaiter().GetResult(),
            () => FirefoxDriver?.Dispose(),
            () => ChromeDriver?.Dispose());
    }

    public DriverFixture ForChrome()
    {
        var wait = new WebDriverWait(ChromeDriver, _config.TimeOut);

        var navigator = new ChromeNavigator(
            wait, 
            _elementNaming, 
            () => ChromeDriver.FindElement(By.TagName("body")),
            ChromeDriver,
            _logger);
            
        Page = new PageBody(navigator) { BaseUrl = _baseUrl };

        return this;
    }
        
    public DriverFixture ForFirefox()
    {
        var wait = new WebDriverWait(FirefoxDriver, _config.TimeOut);

        var navigator = new FirefoxNavigator(
            wait, 
            _elementNaming, 
            () => FirefoxDriver.FindElement(By.TagName("body")),
            FirefoxDriver,
            _logger);
            
        Page = new PageBody(navigator) { BaseUrl = _baseUrl };

        return this;
    }
}