``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1766 (21H1/May2021Update)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.203
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT


```
| Method |     Mean |    Error |   StdDev |   StdErr |      Min |       Q1 |   Median |       Q3 |      Max |     Op/s | Rank |  Gen 0 | Allocated |
|------- |---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|---------:|-----:|-------:|----------:|
|   Url2 | 59.74 μs | 1.142 μs | 1.269 μs | 0.291 μs | 58.02 μs | 58.54 μs | 60.11 μs | 60.50 μs | 62.07 μs | 16,740.2 |    2 | 0.2441 |      7 KB |
|   Url1 | 49.76 μs | 0.929 μs | 1.106 μs | 0.241 μs | 47.66 μs | 49.07 μs | 49.81 μs | 50.40 μs | 51.92 μs | 20,097.8 |    1 | 0.4883 |     13 KB |
