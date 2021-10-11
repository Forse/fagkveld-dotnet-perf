using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DotNetPerf.Domain.Outrights;

public static class Calculator
{
    private static readonly string RootActivity = $"{nameof(Calculator)}.{nameof(Run)}";
    private static readonly string SimulateActivity = $"{RootActivity}.{nameof(Simulate)}";
    private static readonly string ExtractMarketsActivity = $"{RootActivity}.ExtractMarkets";

    [SkipLocalsInit]
    public static Markets Run(int simulations, IEnumerable<Team> teams)
    {
        using var activity = Diagnostics.ActivitySource.StartActivity(RootActivity);
        activity?.SetTag("simulations", simulations);

        TablePositionHistory tablePositionHistory;
        {
            Span<TeamData> teamData = stackalloc TeamData[teams.Count()];

            var teamIndex = 0;
            foreach (var team in teams)
                teamData[teamIndex] = new TeamData(new TeamId(teamIndex++), team.ExpectedGoals);

            using (var simulateActivity = Diagnostics.ActivitySource.StartActivity(SimulateActivity))
            {
                tablePositionHistory = Simulate(simulations, teamData);
            }
        }

        Markets markets;
        using (var simulateActivity = Diagnostics.ActivitySource.StartActivity(ExtractMarketsActivity))
        {
            markets = new Markets(simulations, tablePositionHistory, teams);
        }

        return markets;
    }

    [SkipLocalsInit]
    private static TablePositionHistory Simulate(int simulations, ReadOnlySpan<TeamData> teams)
    {
        Span<MatchData> matches = stackalloc MatchData[GetMatchCount(teams)];
        GetMatches(teams, matches);

        var tablePositionHistory = new TablePositionHistory(teams);

        var table = new Table(
            stackalloc Position[teams.Length],
            stackalloc int[teams.Length],
            teams
        );

        var rng = new Xoroshiro128Plus(Random.Shared);

        for (int simulation = 0; simulation < simulations; simulation++)
        {
            foreach (ref var match in matches)
            {
                ref readonly var homeTeam = ref teams[match.HomeTeam.Id];
                ref readonly var awayTeam = ref teams[match.AwayTeam.Id];

                Simulate(ref match, in homeTeam, in awayTeam, ref rng);

                table.AddResult(in match);

                match.Reset();
            }

            table.Sort();

            tablePositionHistory.Update(table);

            table.Reset();
        }

        return tablePositionHistory;
    }

    private static int GetMatchCount(ReadOnlySpan<TeamData> teams) => (teams.Length - 1) * teams.Length;

    private static void GetMatches(ReadOnlySpan<TeamData> teams, Span<MatchData> matches)
    {
        Debug.Assert(matches.Length == GetMatchCount(teams));

        var matchIndex = 0;
        var matchups = new HashSet<(TeamId Home, TeamId Away)>();
        foreach (ref readonly var home in teams)
        {
            foreach (ref readonly var away in teams)
            {
                if (home == away)
                    continue;

                if (matchups.Add((home.Id, away.Id)))
                    matches[matchIndex++] = new MatchData(home.Id, away.Id);
            }
        }
    }

    [SkipLocalsInit]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void Simulate(ref MatchData match, in TeamData homeTeam, in TeamData awayTeam, ref Xoroshiro128Plus rng)
    {
        // Knuth's poisson algorithm

        var homePoissonLimit = homeTeam.HomePoissonLimit;

        var product = rng.NextDouble();
        while (product >= homePoissonLimit)
        {
            match.HomeGoals++;
            product *= rng.NextDouble();
        }

        var awayPoissonLimit = awayTeam.PoissonLimit;

        product = rng.NextDouble();
        while (product >= awayPoissonLimit)
        {
            match.AwayGoals++;
            product *= rng.NextDouble();
        }
    }
}