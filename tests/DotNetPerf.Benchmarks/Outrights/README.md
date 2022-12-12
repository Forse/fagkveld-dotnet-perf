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
|          Method | Simulations |     Mean |    Error |   StdDev | Code Size | CacheMisses/Op | BranchMispredictions/Op | BranchInstructions/Op | Allocated |
|---------------- |------------ |---------:|---------:|---------:|----------:|---------------:|------------------------:|----------------------:|----------:|
| Calculate_32bit |        1000 | 20.06 ms | 0.226 ms | 0.211 ms |  10,535 B |         44,126 |                 769,929 |            13,767,066 |  21.14 KB |
| Calculate_64bit |        1000 | 20.29 ms | 0.306 ms | 0.286 ms |  10,507 B |         47,010 |                 771,106 |            13,783,586 |  21.14 KB |


#### Visual Studio 2022 profiler

![Visual Studio 2022 profiler](/imgs/visual-studio-profiler-05.png)

#### Other tools

##### EventPipe profiling

Speedscope view

![EventPipe profiling in Speedscope](/imgs/speedscope.png)

##### OpenTelemetry + Jaeger distributed tracing

![OpenTelemetry + Jaeger distributed tracing](/imgs/jaeger.png)
