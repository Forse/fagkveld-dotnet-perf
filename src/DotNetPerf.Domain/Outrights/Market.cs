using System.Numerics;

namespace DotNetPerf.Domain.Outrights;

public sealed record Market<TNumericType>(MarketType Type, IEnumerable<Outcome<TNumericType>> Outcomes)
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>;
