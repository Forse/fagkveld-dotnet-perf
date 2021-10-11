### Outrights benchmark

Ytelsestest på outrights-kalkulering.
Outrights markedene kalkuleres med monte carlo simuleringer.
PL sesong, 20 lag med 38 kamper hver, simuleres 1000 ganger.

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1237 (21H1/May2021Update)
AMD Ryzen 5 5600X, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.100-rc.1.21463.6
  [Host]     : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT


```
|    Method | Simulations |     Mean |   Error |  StdDev |     Gen 0 | Code Size | Allocated |
|---------- |------------ |---------:|--------:|--------:|----------:|----------:|----------:|
| Calculate |        1000 | 299.4 ms | 5.85 ms | 5.75 ms | 5000.0000 |      0 MB |     84 MB |


#### EventPipe profiling

Speedscope view

![EventPipe profiling in Speedscope](/imgs/speedscope.png)


#### Visual Studio 2022 profiler

![Visual Studio 2022 profiler](/imgs/visual-studio-profiler.png)
