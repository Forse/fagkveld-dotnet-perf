namespace DotNetPerf.Domain.Outrights;

public static class Calculator
{
    private static readonly string RootActivity = $"{nameof(Calculator)}.{nameof(Run)}";
    private static readonly string SimulateActivity = $"{RootActivity}.{nameof(Simulate)}";
    private static readonly string ExtractMarketsActivity = $"{RootActivity}.ExtractMarkets";

    public static Markets Run(int simulations, IEnumerable<Team> teams)
    {
        using var activity = Diagnostics.ActivitySource.StartActivity(RootActivity);
        activity?.SetTag("simulations", simulations);

        TablePositionHistory tablePositionHistory;
        using (var simulateActivity = Diagnostics.ActivitySource.StartActivity(SimulateActivity))
        {
            tablePositionHistory = Simulate(simulations, teams);
        }

        Markets markets;
        using (var simulateActivity = Diagnostics.ActivitySource.StartActivity(ExtractMarketsActivity))
        {
            markets = new Markets(simulations, tablePositionHistory, teams);
        }

        return markets;
    }

    private static TablePositionHistory Simulate(int simulations, IEnumerable<Team> teams)
    {
        var matches = GetMatches(teams);

        var tablePositionHistory = new TablePositionHistory(teams);

        var table = new Table(teams);

        for (int simulation = 0; simulation < simulations; simulation++)
        {
            foreach (var match in matches)
            {
                Simulate(match);

                table.AddResult(match);

                match.Reset();
            }

            tablePositionHistory.Update(table);

            table.Reset();
        }

        return tablePositionHistory;
    }

    private static Match[] GetMatches(IEnumerable<Team> teams)
    {
        var matches = new Match[(teams.Count() - 1) * teams.Count()];
        var matchIndex = 0;
        var matchups = new HashSet<(Team Home, Team Away)>();
        foreach (var home in teams)
        {
            foreach (var away in teams)
            {
                if (home == away)
                    continue;

                if (matchups.Add((home, away)))
                    matches[matchIndex++] = new Match(home, away);
            }
        }
        return matches;
    }

    private static void Simulate(Match match)
    {
        var rng = Random.Shared;

        // Knuth's poisson algorithm

        const double homeAdvantage = 0.25;
        var homePoissonLimit = Math.Exp(-(match.HomeTeam.ExpectedGoals + homeAdvantage));

        var product = rng.NextDouble();
        while (product >= homePoissonLimit)
        {
            match.HomeGoals++;
            product *= rng.NextDouble();
        }

        var awayPoissonLimit = Math.Exp(-match.AwayTeam.ExpectedGoals);

        product = rng.NextDouble();
        while (product >= awayPoissonLimit)
        {
            match.AwayGoals++;
            product *= rng.NextDouble();
        }
    }
}