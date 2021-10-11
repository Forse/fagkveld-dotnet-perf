namespace DotNetPerf.Domain.Outrights;

public readonly record struct TeamData
{
    private const double HomeAdvantage = 0.25;

    public TeamData(TeamId id, double expectedGoals)
    {
        Id = id;
        PoissonLimit = Math.Exp(-expectedGoals);
        HomePoissonLimit = Math.Exp(-(expectedGoals + HomeAdvantage));
    }

    public TeamId Id { get; }

    public double PoissonLimit { get; }
    public double HomePoissonLimit { get; }

    public readonly bool Equals(TeamData? other) => Id == other?.Id;

    public readonly override int GetHashCode() => Id.GetHashCode();
}
