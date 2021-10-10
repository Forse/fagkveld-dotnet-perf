namespace DotNetPerf.Benchmarks.Misc;


//public sealed record PassByRefSize4C(int D1);
//public record struct PassByRefSize4S(int D1);
public sealed record PassByRefSize8C(long D1);
public record struct PassByRefSize8S(long D1);
//public sealed record PassByRefSize12C(long D1, int D2);
//public record struct PassByRefSize12S(long D1, int D2);
public sealed record PassByRefSize16C(long D1, long D2);
public record struct PassByRefSize16S(long D1, long D2);
//public sealed record PassByRefSize20C(long D1, long D2, int D3);
//public record struct PassByRefSize20S(long D1, long D2, int D3);
public sealed record PassByRefSize24C(long D1, long D2, long D3);
public record struct PassByRefSize24S(long D1, long D2, long D3);
//public sealed record PassByRefSize28C(long D1, long D2, long D3, int D4);
//public record struct PassByRefSize28S(long D1, long D2, long D3, int D4);
public sealed record PassByRefSize32C(long D1, long D2, long D3, long D4);
public record struct PassByRefSize32S(long D1, long D2, long D3, long D4);

internal static class Methods
{
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(PassByRefSize8C o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(PassByRefSize8S o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(ref PassByRefSize8S o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(PassByRefSize16C o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(PassByRefSize16S o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(ref PassByRefSize16S o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(PassByRefSize24C o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(PassByRefSize24S o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(ref PassByRefSize24S o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(PassByRefSize32C o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(PassByRefSize32S o) => _ = o.D1;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void Do(ref PassByRefSize32S o) => _ = o.D1;
}

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory), CategoriesColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[MemoryDiagnoser]
public class PassByRef
{
    private PassByRefSize8C _passByRefSize8C;
    private PassByRefSize8S _passByRefSize8S;
    private PassByRefSize16C _passByRefSize16C;
    private PassByRefSize16S _passByRefSize16S;
    private PassByRefSize24C _passByRefSize24C;
    private PassByRefSize24S _passByRefSize24S;
    private PassByRefSize32C _passByRefSize32C;
    private PassByRefSize32S _passByRefSize32S;

    [GlobalSetup]
    public void Setup()
    {
        _passByRefSize8C = new PassByRefSize8C(1);
        _passByRefSize8S = new PassByRefSize8S(1);
        _passByRefSize16C = new PassByRefSize16C(1, 1);
        _passByRefSize16S = new PassByRefSize16S(1, 1);
        _passByRefSize24C = new PassByRefSize24C(1, 1, 1);
        _passByRefSize24S = new PassByRefSize24S(1, 1, 1);
        _passByRefSize32C = new PassByRefSize32C(1, 1, 1, 1);
        _passByRefSize32S = new PassByRefSize32S(1, 1, 1, 1);
    }

    [Benchmark, BenchmarkCategory("8")] public void Class_8() => Methods.Do(_passByRefSize8C);
    [Benchmark, BenchmarkCategory("8")] public void Struct_8() => Methods.Do(_passByRefSize8S);
    [Benchmark, BenchmarkCategory("8")] public void Struct_Ref_8() => Methods.Do(ref _passByRefSize8S);

    [Benchmark, BenchmarkCategory("16")] public void Class_16() => Methods.Do(_passByRefSize16C);
    [Benchmark, BenchmarkCategory("16")] public void Struct_16() => Methods.Do(_passByRefSize16S);
    [Benchmark, BenchmarkCategory("16")] public void Struct_Ref_16() => Methods.Do(ref _passByRefSize16S);

    [Benchmark, BenchmarkCategory("24")] public void Class_24() => Methods.Do(_passByRefSize24C);
    [Benchmark, BenchmarkCategory("24")] public void Struct_24() => Methods.Do(_passByRefSize24S);
    [Benchmark, BenchmarkCategory("24")] public void Struct_Ref_24() => Methods.Do(ref _passByRefSize24S);

    [Benchmark, BenchmarkCategory("32")] public void Class_32() => Methods.Do(_passByRefSize32C);
    [Benchmark, BenchmarkCategory("32")] public void Struct_32() => Methods.Do(_passByRefSize32S);
    [Benchmark, BenchmarkCategory("32")] public void Struct_Ref_32() => Methods.Do(ref _passByRefSize32S);
}
