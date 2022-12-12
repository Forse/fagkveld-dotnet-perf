using System.Numerics;

namespace DotNetPerf.Domain.Outrights;

public sealed record Outcome<TNumericType>(string Ref, double Probability) : IComparable<Outcome<TNumericType>>
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>
{
    public int CompareTo(Outcome<TNumericType>? other)
    {
        return other switch
        {
            null => -1,
            var o => o.Probability.CompareTo(Probability),
        };
    }
}
