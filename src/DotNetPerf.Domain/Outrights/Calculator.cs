using System.Collections;

namespace DotNetPerf.Domain.Outrights;

public sealed record Markets : IEnumerable<Market>
{
    private readonly List<Market> _markets;

    public Markets(int simulations, TablePositionHistory tablePositionHistory, IEnumerable<Team> teams)
    {
        _markets = new List<Market>();
        AddWinnerMarket(simulations, tablePositionHistory, teams);
        AddTop4Market(simulations, tablePositionHistory, teams);
    }

    private void AddWinnerMarket(int simulations, TablePositionHistory tablePositionHistory, IEnumerable<Team> teams)
    {
        var outcomes = new Outcome[teams.Count()];
        var outcomesIndex = 0;
        foreach (var team in teams)
        {
            var history = tablePositionHistory[team];
            var timesInFirstPlace = history[0];

            var winProbability = timesInFirstPlace / (double)simulations;
            outcomes[outcomesIndex++] = new Outcome(team.Name, winProbability);
        }

        Array.Sort(outcomes);
        var market = new Market(MarketType.Winner, outcomes);
        _markets.Add(market);
    }

    private void AddTop4Market(int simulations, TablePositionHistory tablePositionHistory, IEnumerable<Team> teams)
    {
        var outcomes = new Outcome[teams.Count()];
        var outcomesIndex = 0;
        foreach (var team in teams)
        {
            var history = tablePositionHistory[team];
            var timesInTop4 = history[0..4].Sum();

            var winProbability = timesInTop4 / (double)simulations;
            outcomes[outcomesIndex++] = new Outcome(team.Name, winProbability);
        }

        Array.Sort(outcomes);
        var market = new Market(MarketType.Winner, outcomes);
        _markets.Add(market);
    }

    IEnumerator IEnumerable.GetEnumerator() => _markets.GetEnumerator();

    public IEnumerator<Market> GetEnumerator() => _markets.GetEnumerator();
}

public static class Calculator
{
    public static Markets Run(int simulations, IEnumerable<Team> teams)
    {
        var tablePositionHistory = Simulate(simulations, teams);
        return new Markets(simulations, tablePositionHistory, teams);
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