using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotNetPerf.Domain.Outrights;

/// <summary>
/// Implementation of http://prng.di.unimi.it/xoroshiro128plus.c
/// One of the fastest prng's for 32bit/64bit floating points.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[SkipLocalsInit]
public struct Xoroshiro128Plus
{
    private const ulong DOUBLE_MASK = (1L << 53) - 1;
    private const double NORM_53 = 1.0d / (1L << 53);
    private const ulong FLOAT_MASK = (1L << 24) - 1;
    private const float NORM_24 = 1.0f / (1L << 24);

    private const int A = 24;
    private const int B = 16;
    private const int C = 37;

    private ulong state0;
    private ulong state1;

    public Xoroshiro128Plus(Random? random = null)
    {
        if (random is not null)
        {
            const int min = int.MinValue;
            const int max = int.MaxValue;
            state0 = (ulong)random.Next(min, max) << 32 | (uint)random.Next(min, max);
            state1 = (ulong)random.Next(min, max) << 32 | (uint)random.Next(min, max);
        }
        else
        {
            Unsafe.SkipInit(out ulong rng1);
            Unsafe.SkipInit(out uint rng2);
            Unsafe.SkipInit(out ulong rng3);
            Unsafe.SkipInit(out uint rng4);

            state0 = rng1 << 32 | rng2;
            state1 = rng3 << 32 | rng4;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static ulong Rotl(ulong x, int k)
    {
        return (x << k) | (x >> (64 - k));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private ulong NextInternal()
    {
        var s0 = state0;
        var s1 = state1 ^ s0;

        state0 = Rotl(s0, A) ^ s1 ^ s1 << B;
        state1 = Rotl(s1, C);

        return state0 + state1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int Next()
    {
        return (int)(NextInternal() >> 32);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public double NextDouble()
    {
        return (NextInternal() & DOUBLE_MASK) * NORM_53;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public float NextFloat()
    {
        return (NextInternal() & FLOAT_MASK) * NORM_24;
    }
}