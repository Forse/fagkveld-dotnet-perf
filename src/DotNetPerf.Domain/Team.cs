namespace DotNetPerf.Domain;

public sealed record Team(string Name, double ExpectedGoals)
{
    public bool Equals(Team? other) => Name == other?.Name;

    public override int GetHashCode() => Name.GetHashCode();
}
