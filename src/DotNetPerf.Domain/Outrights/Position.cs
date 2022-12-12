using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DotNetPerf.Domain.Outrights;

public record struct Position : IComparable<Position>
{
    public Position(TeamId team)
    {
        Team = team;
        Played = 0;
        Won = 0;
        Drawn = 0;
        Lost = 0;
        GoalsFor = 0;
        GoalsAgainst = 0;
        GoalDifference = 0;
        Points = 0;
    }

    public readonly TeamId Team { get; }

    public int Played { readonly get; private set; }
    public int Won { readonly get; private set; }
    public int Drawn { readonly get; private set; }
    public int Lost { readonly get; private set; }
    public int GoalsFor { readonly get; private set; }
    public int GoalsAgainst { readonly get; private set; }
    public int GoalDifference { readonly get; private set; }
    public int Points { readonly get; private set; }

    public void AddMatch(ref MatchData match)
    {
        Debug.Assert(match.HomeTeam == Team || match.AwayTeam == Team);
        var isHomeTeam = match.HomeTeam == Team;
        var isAwayTeam = !isHomeTeam;

        var isWin = (isHomeTeam && match.IsHomeWin) || (!isHomeTeam && match.IsAwayWin);
        var isDraw = match.IsDraw;
        var isLost = !isWin && !isDraw;

        Played++;

        Won += Unsafe.As<bool, int>(ref isWin);
        Drawn += Unsafe.As<bool, int>(ref isDraw);
        Lost += Unsafe.As<bool, int>(ref isLost);
        Points += (Unsafe.As<bool, int>(ref isWin) * 3) + (Unsafe.As<bool, int>(ref isDraw));

        var goalsFor = (Unsafe.As<bool, int>(ref isHomeTeam) * match.HomeGoals) +
            (Unsafe.As<bool, int>(ref isAwayTeam) * match.AwayGoals);
        var goalsAgainst = (Unsafe.As<bool, int>(ref isHomeTeam) * match.AwayGoals) +
            (Unsafe.As<bool, int>(ref isAwayTeam) * match.HomeGoals);

        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;
        GoalDifference += goalsFor - goalsAgainst;

        //if (isWin)
        //{
        //    Won++;
        //    Points += 3;
        //}
        //else if (match.IsDraw)
        //{
        //    Drawn++;
        //    Points += 1;
        //}
        //else
        //{
        //    Lost++;
        //}

        //var goalsFor = isHomeTeam ? match.HomeGoals : match.AwayGoals;
        //var goalsAgainst = isHomeTeam ? match.AwayGoals : match.HomeGoals;

        //GoalsFor += goalsFor;
        //GoalsAgainst += goalsAgainst;
        //GoalDifference += goalsFor - goalsAgainst;
    }

    public void Reset()
    {
        Played = 0;
        Won = 0;
        Drawn = 0;
        Lost = 0;
        GoalsFor = 0;
        GoalsAgainst = 0;
        GoalDifference = 0;
        Points = 0;
    }

    public readonly int CompareTo(Position other)
    {
        return other switch
        {
            var p when Points == p.Points => (GoalDifference - p.GoalDifference) switch
            {
                0 => 0,
                < 0 => 1,
                > 0 => -1,
            },
            var p => (Points - p.Points) switch
            {
                < 0 => 1,
                > 0 => -1,
                _ => throw new Exception("Invalid state"),
            },
        };
    }
}
