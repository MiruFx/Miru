using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Miru.Hosting;
using Miru.Mvc;
using Miru.Tests.Urls;
using Miru.Urls;

namespace Miru.Tests;

public class Program
{
    public static void Main(string[] args)
    {
        // await new BenchmarkTagHelper().Boolean();
        
        // BenchmarkRunner.Run<DisplayTagBenchmark>(
        //     DefaultConfig.Instance
        //         .WithOptions(ConfigOptions.DisableOptimizationsValidator));

        // new BenchmarkTagHelper().Url2();
    }

    private static void UrlGeneration()
    {
        var generator = new RouteValueDictionaryGenerator(new UrlOptions());
        
        for (int i = 0; i < 100; i++)
        {
            var modifiers = new UrlBuilderModifiers();
    
            modifiers.WithoutValues.Add("Size", RouteValueDictionaryGeneratorTest.Size.Medium);
            modifiers.WithoutValues.Add("Size", RouteValueDictionaryGeneratorTest.Size.Large);
    
            var dic = generator.Generate(new RouteValueDictionaryGeneratorTest.ProductsList.Query
            {
                Size = new List<RouteValueDictionaryGeneratorTest.Size>
                {
                    RouteValueDictionaryGeneratorTest.Size.Small,
                    RouteValueDictionaryGeneratorTest.Size.Medium,
                    RouteValueDictionaryGeneratorTest.Size.Large
                }
            }, modifiers);
        }
        
    
        // dic.ShouldCount(1);
        // dic["Size[0]"].ShouldBe(RouteValueDictionaryGeneratorTest.ProductsList.Size.Small);
        
        // BenchmarkRunner.Run<BenchmarkUrl>(
        //     DefaultConfig.Instance
        //         .WithOptions(ConfigOptions.DisableOptimizationsValidator));

        // new BenchmarkUrl().Url2();
        
        // await Task.CompletedTask;
        // var rootCommand = new RootCommand
        // {
        //     new TestNewSolutionCommand()
        // };
        //
        // await rootCommand.InvokeAsync(args);
    }
}

[MemoryDiagnoser]
[RankColumn, AllStatisticsColumn]
public class BenchmarkUrl
{
    private readonly MiruTestWebHost _host;
    // private UrlLookup UrlLookup;
    private readonly IUrlMaps _urlMaps;
    private readonly UrlOptions _urlOptions;
    private readonly UrlTest.ProductsList.Query _request;

    public BenchmarkUrl()
    {
        _host = new(MiruHost.CreateMiruHost(),
            services =>
            {
                services
                    .AddMvcCore()
                    .AddMiruActionResult()
                    .AddMiruNestedControllers();

                services.AddMiruUrls(x =>
                {
                    x.Base = "https://mirufx.github.io";
                });

                services.AddControllersWithViews();
            });
        
        _urlMaps = _host.Services.GetService<IUrlMaps>();
        _urlOptions = _host.Services.GetService<UrlOptions>();
        
        _request = new UrlTest.ProductsList.Query
        {
            Category = "Cars",
            Size = new List<UrlTest.ProductsList.Size>
            {
                UrlTest.ProductsList.Size.Small,
                UrlTest.ProductsList.Size.Medium,
                UrlTest.ProductsList.Size.Large
            }
        };
    }
    
    [Benchmark]
    public void Url2()
    {
        var url = new UrlBuilder<UrlTest.ProductsList.Query>(_request, _urlOptions, _urlMaps);
            
        url
            .With(m => m.Page, 3)
            .Without(m => m.Category)
            .Without(m => m.Size, UrlTest.ProductsList.Size.Medium)
            .Without(m => m.Size, UrlTest.ProductsList.Size.Large)
            .ToString()
            .ShouldBe("/Products/List?Size%5B0%5D=Small&Page=3");
    }
}
//
// [MemoryDiagnoser]
// [RankColumn, AllStatisticsColumn]
// public class BenchmarkTagHelper : MiruTagTesting
// {
//     private readonly RadioButtonTest.Boolean _test;
//
//     public BenchmarkTagHelper()
//     {
//         _test = new RadioButtonTest.Boolean();
//         _test.SetupMiruCoreTesting();
//         _test.OneTimeSetup();
//     }
//
//     [Benchmark]
//     public async Task Boolean()
//     {
//         await _test.If_input_is_for_true_and_property_is_true_then_input_should_be_checked();
//     }
// }