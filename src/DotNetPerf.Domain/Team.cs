using System.Numerics;

namespace DotNetPerf.Domain;

public sealed record Team<TNumericType>(string Name, TNumericType ExpectedGoals)
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>
{
    public bool Equals(Team<TNumericType>? other) => Name == other?.Name;

    public override int GetHashCode() => Name.GetHashCode();
}
