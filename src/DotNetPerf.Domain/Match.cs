using System.Numerics;

namespace DotNetPerf.Domain;

public sealed record Match<TNumericType>(Team<TNumericType> HomeTeam, Team<TNumericType> AwayTeam)
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>
{
    public int HomeGoals { get; set; }

    public int AwayGoals { get; set; }

    public void Reset()
    {
        HomeGoals = 0;
        AwayGoals = 0;
    }

    public bool IsHomeWin => HomeGoals > AwayGoals;

    public bool IsDraw => HomeGoals == AwayGoals;

    public bool IsAwayWin => AwayGoals > HomeGoals;

    public bool Equals(Match<TNumericType>? other) => (HomeTeam, AwayTeam) == (other?.HomeTeam, other?.AwayTeam);

    public override int GetHashCode() => (HomeTeam, AwayTeam).GetHashCode();
}
