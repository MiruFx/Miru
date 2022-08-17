using BenchmarkDotNet.Attributes;
using Miru.Tests.Html.TagHelpers;

namespace Miru.Tests.Benchmarks;

[MemoryDiagnoser]
[RankColumn, AllStatisticsColumn]
public class DisplayTagBenchmark
{
    private readonly DisplayTagHelperTest _test;

    public DisplayTagBenchmark()
    {
        _test = new DisplayTagHelperTest();
        _test.SetupMiruCoreTesting();
        _test.OneTimeSetup();
    }

    [Benchmark]
    public async Task Boolean()
    {
        await _test.Should_render_class();
    }
}