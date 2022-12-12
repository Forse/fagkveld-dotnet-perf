using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using InlineIL;

using static InlineIL.IL.Emit;

namespace DotNetPerf.Domain.Outrights;

/// <summary>
/// Implementation of http://prng.di.unimi.it/xoroshiro128plus.c
/// One of the fastest prng's for 32bit/64bit floating points.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[SkipLocalsInit]
public struct Xoroshiro128Plus<TNumericType> 
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>
{
    private static readonly ulong MASK;
    private static readonly TNumericType NORM;

    private const int A = 24;
    private const int B = 16;
    private const int C = 37;

    private ulong state0;
    private ulong state1;

    static Xoroshiro128Plus()
    {
        if (typeof(TNumericType) == typeof(float))
        {
            MASK = (1L << 24) - 1;
            NORM = TNumericType.CreateChecked(1.0) / TNumericType.CreateChecked(1L << 24);
        }
        else if (typeof(TNumericType) == typeof(double))
        {
            MASK = (1L << 53) - 1;
            NORM = TNumericType.CreateChecked(1.0) / TNumericType.CreateChecked(1L << 53);
        }
        else
            throw new InvalidProgramException();
    }

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
    public TNumericType NextFloating()
    {
        // InlineIL to avoid CreateChecked on the generic floating type, which would add overhead..
        Ldarg_0();
        Call(new MethodRef(typeof(Xoroshiro128Plus<TNumericType>), nameof(NextInternal)));
        Ldsfld(new FieldRef(typeof(Xoroshiro128Plus<TNumericType>), nameof(MASK)));
        And();
        Conv_R_Un();
        Conv_R4();
        Ldsfld(new FieldRef(typeof(Xoroshiro128Plus<TNumericType>), nameof(NORM)));
        Mul();
        return IL.Return<TNumericType>();
    }
}