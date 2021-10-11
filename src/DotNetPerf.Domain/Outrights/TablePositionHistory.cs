namespace DotNetPerf.Domain.Outrights;

public sealed record TablePositionHistory
{
    private readonly Dictionary<Team, int[]> _map;

    public int Count => _map.Count;

    public int[] this[Team team] => _map[team];

    public TablePositionHistory(IEnumerable<Team> teams)
    {
        _map = new (teams.Count());
        foreach (var team in teams)
            _map.Add(team, new int[teams.Count()]);
    }

    public void Update(Table table)
    {
        for (int i = 0; i < table.Count; i++)
        {
            ref readonly var position = ref table[i];
            _map[position.Team][i]++;
        }
    }
}
