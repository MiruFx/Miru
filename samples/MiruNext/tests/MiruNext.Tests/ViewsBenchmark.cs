using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using Baseline.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Miru.Testing.Html;

namespace MiruNext.Tests;

[MemoryDiagnoser]
[RankColumn, AllStatisticsColumn]
public class ViewsBenchmark : MiruTagTesting
{
    public ViewsBenchmark()
    {
    }

    [Benchmark]
    public void OrderListString()
    {
    }
    
    [Benchmark]
    public void OrderListBuffer()
    {
    }
}