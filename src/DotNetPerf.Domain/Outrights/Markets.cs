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
        var teamIndex = 0;
        foreach (var team in teams)
        {
            var history = tablePositionHistory[new TeamId(teamIndex)];
            var timesInFirstPlace = history[0];

            var winProbability = timesInFirstPlace / (double)simulations;
            outcomes[outcomesIndex++] = new Outcome(team.Name, winProbability);

            teamIndex++;
        }

        Array.Sort(outcomes);
        var market = new Market(MarketType.Winner, outcomes);
        _markets.Add(market);
    }

    private void AddTop4Market(int simulations, TablePositionHistory tablePositionHistory, IEnumerable<Team> teams)
    {
        var outcomes = new Outcome[teams.Count()];
        var outcomesIndex = 0;
        var teamIndex = 0;
        foreach (var team in teams)
        {
            var history = tablePositionHistory[new TeamId(teamIndex)];
            var timesInTop4 = history[0..4].Sum();

            var winProbability = timesInTop4 / (double)simulations;
            outcomes[outcomesIndex++] = new Outcome(team.Name, winProbability);

            teamIndex++;
        }

        Array.Sort(outcomes);
        var market = new Market(MarketType.Winner, outcomes);
        _markets.Add(market);
    }

    IEnumerator IEnumerable.GetEnumerator() => _markets.GetEnumerator();

    public IEnumerator<Market> GetEnumerator() => _markets.GetEnumerator();
}
