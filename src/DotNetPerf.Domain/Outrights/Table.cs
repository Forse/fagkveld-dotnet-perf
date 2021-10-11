namespace DotNetPerf.Domain.Outrights;

public sealed record Table
{
    private readonly Position[] _positions;
    private readonly Dictionary<Team, int> _map;

    public int Count => _positions.Length;

    public ref readonly Position this[int index] => ref _positions[index];

    public Table(IEnumerable<Team> teams)
    {
        _positions = new Position[teams.Count()];
        _map = new(teams.Count());
        var positionIndex = 0;
        foreach (var team in teams)
        {
            _positions[positionIndex] = new Position(team);
            _map[team] = positionIndex++;
        }
    }

    public void AddResult(Match match)
    {
        var positions = _positions;

        ref var homePosition = ref positions[_map[match.HomeTeam]];
        ref var awayPosition = ref positions[_map[match.AwayTeam]];

        homePosition.AddMatch(match);
        awayPosition.AddMatch(match);
    }

    public void Sort()
    {
        Span<Position> positions = _positions;

        positions.Sort();
        for (int i = 0; i < positions.Length; i++)
        {
            ref readonly var position = ref positions[i];
            _map[position.Team] = i;
        }
    }

    public ReadOnlySpan<Position>.Enumerator GetEnumerator()
    {
        ReadOnlySpan<Position> positions = _positions;
        return positions.GetEnumerator();
    }

    public void Reset()
    {
        Span<Position> positions = _positions;

        foreach (ref var position in positions)
            position.Reset();
    }
}
