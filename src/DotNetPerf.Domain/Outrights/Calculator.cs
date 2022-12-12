using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DotNetPerf.Domain.Outrights;

public static class Calculator<TNumericType>
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>
{
    private static readonly string RootActivity = $"{nameof(Calculator<TNumericType>)}.{nameof(Run)}";
    private static readonly string SimulateActivity = $"{RootActivity}.{nameof(Simulate)}";
    private static readonly string ExtractMarketsActivity = $"{RootActivity}.ExtractMarkets";

    [SkipLocalsInit]
    public static Markets<TNumericType> Run(int simulations, IReadOnlyList<Team<TNumericType>> teams)
    {
        using var activity = Diagnostics.ActivitySource.StartActivity(RootActivity);
        activity?.SetTag("simulations", simulations);

        TablePositionHistory<TNumericType> tablePositionHistory;
        {
            Span<TeamData<TNumericType>> teamData = stackalloc TeamData<TNumericType>[teams.Count];

            for (int i = 0; i < teams.Count; i++)
                teamData[i] = new TeamData<TNumericType>(new TeamId(i), teams[i].ExpectedGoals);

            using (var simulateActivity = Diagnostics.ActivitySource.StartActivity(SimulateActivity))
            {
                tablePositionHistory = Simulate(simulations, teamData);
            }
        }

        Markets<TNumericType> markets;
        using (var simulateActivity = Diagnostics.ActivitySource.StartActivity(ExtractMarketsActivity))
        {
            markets = new Markets<TNumericType>(simulations, tablePositionHistory, teams);
        }

        return markets;
    }

    [SkipLocalsInit]
    private static TablePositionHistory<TNumericType> Simulate(int simulations, Span<TeamData<TNumericType>> teams)
    {
        Span<MatchData> matches = stackalloc MatchData[GetMatchCount(teams)];
        GetMatches(teams, matches);

        var tablePositionHistory = new TablePositionHistory<TNumericType>(teams);

        var table = new Table<TNumericType>(
            stackalloc Position[teams.Length],
            stackalloc int[teams.Length],
            teams
        );

        var rng = new Xoroshiro128Plus<TNumericType>(Random.Shared);

        for (int simulation = 0; simulation < simulations; simulation++)
        {
            foreach (ref var match in matches)
            {
                ref var homeTeam = ref teams[match.HomeTeam.Id];
                ref var awayTeam = ref teams[match.AwayTeam.Id];

                Simulate(ref match, ref homeTeam, ref awayTeam, ref rng);

                table.AddResult(ref match);

                match.Reset();
            }

            table.Sort();

            tablePositionHistory.Update(table);

            table.Reset();
        }

        return tablePositionHistory;
    }

    private static int GetMatchCount(ReadOnlySpan<TeamData<TNumericType>> teams) => (teams.Length - 1) * teams.Length;

    private static void GetMatches(ReadOnlySpan<TeamData<TNumericType>> teams, Span<MatchData> matches)
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
    private static void Simulate(ref MatchData match, ref TeamData<TNumericType> homeTeam, ref TeamData<TNumericType> awayTeam, ref Xoroshiro128Plus<TNumericType> rng)
    {
        // Knuth's poisson algorithm

        var homePoissonLimit = homeTeam.HomePoissonLimit;

        var product = rng.NextFloating();
        while (product >= homePoissonLimit)
        {
            match.HomeGoals++;
            product *= rng.NextFloating();
        }

        var awayPoissonLimit = awayTeam.PoissonLimit;

        product = rng.NextFloating();
        while (product >= awayPoissonLimit)
        {
            match.AwayGoals++;
            product *= rng.NextFloating();
        }
    }
}