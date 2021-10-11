namespace DotNetPerf.Domain.Outrights;

public record struct MatchData
{
    public MatchData(TeamId homeTeam, TeamId awayTeam)
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        HomeGoals = 0;
        AwayGoals = 0;
    }

    public readonly TeamId HomeTeam { get; }
    public readonly TeamId AwayTeam { get; }

    public int HomeGoals { readonly get; set; }

    public int AwayGoals { readonly get; set; }

    public void Reset()
    {
        HomeGoals = 0;
        AwayGoals = 0;
    }

    public readonly bool IsHomeWin => HomeGoals > AwayGoals;

    public readonly bool IsDraw => HomeGoals == AwayGoals;

    public readonly bool IsAwayWin => AwayGoals > HomeGoals;

    public readonly bool Equals(MatchData other) => (HomeTeam, AwayTeam) == (other.HomeTeam, other.AwayTeam);

    public readonly override int GetHashCode() => (HomeTeam, AwayTeam).GetHashCode();
}
