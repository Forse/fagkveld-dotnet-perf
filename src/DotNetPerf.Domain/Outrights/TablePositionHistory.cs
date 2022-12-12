using System.Numerics;

namespace DotNetPerf.Domain.Outrights;

public sealed record TablePositionHistory<TNumericType>
    where TNumericType : unmanaged, IBinaryFloatingPointIeee754<TNumericType>
{
    private readonly Dictionary<TeamId, int[]> _map;

    public int Count => _map.Count;

    public int[] this[TeamId team] => _map[team];

    public TablePositionHistory(ReadOnlySpan<TeamData<TNumericType>> teams)
    {
        _map = new (teams.Length);
        foreach (ref readonly var team in teams)
            _map.Add(team.Id, new int[teams.Length]);
    }

    public void Update(Table<TNumericType> table)
    {
        for (int i = 0; i < table.Count; i++)
        {
            ref readonly var position = ref table[i];
            _map[position.Team][i]++;
        }
    }
}
