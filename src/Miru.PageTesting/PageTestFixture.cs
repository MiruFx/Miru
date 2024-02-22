using System;
using System.IO;
using Miru.Core;
using Miru.Storages;
using Miru.Testing;
using Miru.Urls;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Miru.PageTesting
{
    /// <summary>
    /// PageTestFixture is a facade to access other helpers to implement your PageTest 
    /// </summary>
    public class PageTestFixture : PageBody, ITestFixture
    {
        private readonly UrlLookup _urlLookup;
        private readonly PageTestingConfig _config;
        private readonly WebDriver _driver;
        private readonly ITempStorage _storage;
        private readonly PageBody _browser;

        public PageTestFixture(
            IMiruApp app,
            UrlLookup urlLookup,
            PageTestingConfig config, 
            WebDriver driver,
            ITempStorage storage, 
            PageBody browser,
            MiruNavigator navigator) : base(navigator)
        {
            App = app;
            
            _urlLookup = urlLookup;
            _config = config;
            _driver = driver;
            _storage = storage;
            _browser = browser;

            _browser.BaseUrl = _config.BaseUrl;
            
            navigator.ConfigureExceptions(context =>
            {
                var exceptionMessage = $@"{context.FailureMessage.IfEmpty(context.OriginalException.Message)}

The url was: 
{Url}
  
A screenshot was taken on the moment of the failure: 
{FailureScreenshot()}

The Page's html has been saved to:
{SaveHtml()}
";

                throw new PageTestException(exceptionMessage, context.OriginalException);
            });
        }

        public IMiruApp App { get; }

        public T Get<T>()
        {
            return App.Get<T>();
        }
        
        public object Get(Type type)
        {
            return App.Get(type);
        }
        
        public PageTestFixture Visit<TModel>() where TModel : class, new()
        {
            var url = _urlLookup.For<TModel>();
            
            MiruTest.Log.Debug($"Visiting {typeof(TModel).ActionName()}: {url}");
            
            Visit(url);
            
            return this;
        }

        public PageTestFixture Visit<TModel>(TModel model) where TModel : class
        {
            var url = _urlLookup.For(model);
            
            MiruTest.Log.Debug($"Visiting {typeof(TModel).ActionName()}: {url}");
            
            Visit(url);
            
            return this;
        }

        public PageTestFixture Visit(string url)
        {
            _browser.NavigateTo(url);

            return this;
        }

        public void ShouldRedirectTo(string path)
        {
            Nav.Expect(() => Path.StartsWith(path), $"Should have navigated to {path}");
        }
        
        public void ShouldRedirectTo<T>() where T : class, new()
        {
            var path = _urlLookup.For<T>();
            
            Nav.Expect(() => Path.StartsWith(path), $"Should have navigated to {path}");
        }
        
        public string Path => _driver.Url.Replace(_config.BaseUrl, string.Empty);

        public string FailureScreenshot()
        {
            return Screenshot("Failure");
        }

        public string Screenshot(string suffix)
        {
            var file = _storage.Path / "screenshots" / $"{TestContext.CurrentContext.Test.MethodName}-{suffix}.png";
             
            file.Dir().EnsureDirExist();
            
            _driver.GetScreenshot().SaveAsFile(file);

            return file;
        }

        private string SaveHtml()
        {
            var file = _storage.Path / "htmls" / $"{TestContext.CurrentContext.Test.MethodName}-Failure.html";
             
            file.Dir().EnsureDirExist();
            
            File.WriteAllText(file, _driver.PageSource);

            return file;
        }
    }
}