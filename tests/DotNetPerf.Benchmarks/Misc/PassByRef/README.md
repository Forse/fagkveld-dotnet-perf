### PassByRef benchmark

Ytelsestest på når det lønner seg å bruke pass by ref for struct. Klasse-variant brukt som en slags baseline.

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1237 (21H1/May2021Update)
AMD Ryzen 5 5600X, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.100-rc.1.21463.6
  [Host]     : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.45113), X64 RyuJIT


```
|        Method | Categories |      Mean |     Error |    StdDev | Allocated |
|-------------- |----------- |----------:|----------:|----------:|----------:|
|       Class_8 |          8 | 0.4310 ns | 0.0036 ns | 0.0030 ns |         - |
|      Struct_8 |          8 | 0.4375 ns | 0.0099 ns | 0.0087 ns |         - |
|  Struct_Ref_8 |          8 | 0.4381 ns | 0.0138 ns | 0.0129 ns |         - |
|               |            |           |           |           |           |
| Struct_Ref_16 |         16 | 0.4347 ns | 0.0040 ns | 0.0031 ns |         - |
|      Class_16 |         16 | 0.4365 ns | 0.0088 ns | 0.0073 ns |         - |
|     Struct_16 |         16 | 1.0876 ns | 0.0088 ns | 0.0078 ns |         - |
|               |            |           |           |           |           |
| Struct_Ref_24 |         24 | 0.4355 ns | 0.0078 ns | 0.0069 ns |         - |
|      Class_24 |         24 | 0.4416 ns | 0.0097 ns | 0.0086 ns |         - |
|     Struct_24 |         24 | 1.0914 ns | 0.0091 ns | 0.0086 ns |         - |
|               |            |           |           |           |           |
| Struct_Ref_32 |         32 | 0.4355 ns | 0.0098 ns | 0.0087 ns |         - |
|      Class_32 |         32 | 0.4389 ns | 0.0085 ns | 0.0075 ns |         - |
|     Struct_32 |         32 | 1.3088 ns | 0.0105 ns | 0.0087 ns |         - |

