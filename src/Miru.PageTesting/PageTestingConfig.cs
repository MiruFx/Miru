using System;
using Baseline.Dates;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.PageTesting
{
    public class PageTestingConfig
    {
        public bool Headless { get; set; }
        
        public string BaseUrl { get; set; }

        public bool StartLocalServer { get; set; } = true;

        public TimeSpan TimeOut { get; set; } = 5.Seconds();
        
        public bool OnFailureScreenshot { get; set; } = true;
        
        public bool OnFailureSaveHtml { get; set; } = false;
        
        public PageTestingBrowser Browser { get; set; }
        
        public IServiceCollection Services { get; set; }
    }
}