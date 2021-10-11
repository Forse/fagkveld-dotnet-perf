namespace DotNetPerf.Domain.Outrights;

public record struct TeamId(int Id)
{
    public static implicit operator int(TeamId id) => id.Id;
}
