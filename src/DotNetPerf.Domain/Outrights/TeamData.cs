namespace DotNetPerf.Domain.Outrights;

public record struct TeamData(TeamId Id, double ExpectedGoals)
{
    public readonly bool Equals(TeamData? other) => Id == other?.Id;

    public readonly override int GetHashCode() => Id.GetHashCode();
}
