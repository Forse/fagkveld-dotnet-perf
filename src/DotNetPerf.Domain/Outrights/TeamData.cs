using System.Numerics;

namespace DotNetPerf.Domain.Outrights;

public readonly record struct TeamData<TNumericType>
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>
{
    private static readonly TNumericType HomeAdvantage = TNumericType.CreateChecked(0.25);

    public TeamData(TeamId id, TNumericType expectedGoals)
    {
        Id = id;
        PoissonLimit = TNumericType.Exp(-expectedGoals);
        HomePoissonLimit = TNumericType.Exp(-(expectedGoals + HomeAdvantage));
    }

    public TeamId Id { get; }

    public TNumericType PoissonLimit { get; }
    public TNumericType HomePoissonLimit { get; }

    public readonly bool Equals(TeamData<TNumericType>? other) => Id == other?.Id;

    public readonly override int GetHashCode() => Id.GetHashCode();
}
