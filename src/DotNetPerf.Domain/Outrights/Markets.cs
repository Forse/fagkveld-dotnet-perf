using System.Collections;
using System.Numerics;

namespace DotNetPerf.Domain.Outrights;

public sealed record Markets<TNumericType> : IEnumerable<Market<TNumericType>>
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>
{
    private readonly List<Market<TNumericType>> _markets;

    public Markets(int simulations, TablePositionHistory<TNumericType> tablePositionHistory, IEnumerable<Team<TNumericType>> teams)
    {
        _markets = new List<Market<TNumericType>>();
        AddWinnerMarket(simulations, tablePositionHistory, teams);
        AddTop4Market(simulations, tablePositionHistory, teams);
    }

    private void AddWinnerMarket(int simulations, TablePositionHistory<TNumericType> tablePositionHistory, IEnumerable<Team<TNumericType>> teams)
    {
        var outcomes = new Outcome<TNumericType>[teams.Count()];
        var outcomesIndex = 0;
        var teamIndex = 0;
        foreach (var team in teams)
        {
            var history = tablePositionHistory[new TeamId(teamIndex)];
            var timesInFirstPlace = history[0];

            var winProbability = timesInFirstPlace / (double)simulations;
            outcomes[outcomesIndex++] = new Outcome<TNumericType>(team.Name, winProbability);

            teamIndex++;
        }

        Array.Sort(outcomes);
        var market = new Market<TNumericType>(MarketType.Winner, outcomes);
        _markets.Add(market);
    }

    private void AddTop4Market(int simulations, TablePositionHistory<TNumericType> tablePositionHistory, IEnumerable<Team<TNumericType>> teams)
    {
        var outcomes = new Outcome<TNumericType>[teams.Count()];
        var outcomesIndex = 0;
        var teamIndex = 0;
        foreach (var team in teams)
        {
            var history = tablePositionHistory[new TeamId(teamIndex)];
            var timesInTop4 = history[0..4].Sum();

            var winProbability = timesInTop4 / (double)simulations;
            outcomes[outcomesIndex++] = new Outcome<TNumericType>(team.Name, winProbability);

            teamIndex++;
        }

        Array.Sort(outcomes);
        var market = new Market<TNumericType>(MarketType.Winner, outcomes);
        _markets.Add(market);
    }

    IEnumerator IEnumerable.GetEnumerator() => _markets.GetEnumerator();

    public IEnumerator<Market<TNumericType>> GetEnumerator() => _markets.GetEnumerator();
}
