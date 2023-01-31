using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace MiruNext.Tests;

public class Program
{
    public static void Main(string[] args) => 
        BenchmarkRunner.Run<ViewsBenchmark>(
            DefaultConfig.Instance
                .WithOptions(ConfigOptions.DisableOptimizationsValidator));
}