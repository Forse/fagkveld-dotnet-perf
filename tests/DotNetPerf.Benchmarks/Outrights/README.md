### Outrights benchmark

Ytelsestest på outrights-kalkulering.
Outrights markedene kalkuleres med monte carlo simuleringer.
PL sesong, 20 lag med 38 kamper hver, simuleres 1000 ganger.

``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 5 5600X, 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|    Method | Simulations |     Mean |    Error |   StdDev | BranchInstructions/Op | BranchMispredictions/Op | CacheMisses/Op | Code Size | Allocated |
|---------- |------------ |---------:|---------:|---------:|----------------------:|------------------------:|---------------:|----------:|----------:|
| Calculate |        1000 | 16.95 ms | 0.101 ms | 0.089 ms |            15,518,379 |                 763,401 |         31,974 |   8,877 B |  21.17 KB |


#### Visual Studio 2022 profiler

![Visual Studio 2022 profiler](/imgs/visual-studio-profiler-05.png)

#### Other tools

##### EventPipe profiling

Speedscope view

![EventPipe profiling in Speedscope](/imgs/speedscope.png)

##### OpenTelemetry + Jaeger distributed tracing

![OpenTelemetry + Jaeger distributed tracing](/imgs/jaeger.png)
