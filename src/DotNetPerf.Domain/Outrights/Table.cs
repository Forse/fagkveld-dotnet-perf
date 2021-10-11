namespace DotNetPerf.Domain.Outrights;

public sealed record Table
{
    private readonly Position[] _positions;
    private readonly Dictionary<TeamId, int> _map;

    public int Count => _positions.Length;

    public ref readonly Position this[int index] => ref _positions[index];

    public Table(ReadOnlySpan<TeamData> teams)
    {
        _positions = new Position[teams.Length];
        _map = new(teams.Length);
        var positionIndex = 0;
        foreach (ref readonly var team in teams)
        {
            _positions[positionIndex] = new Position(team.Id);
            _map[team.Id] = positionIndex++;
        }
    }

    public void AddResult(in MatchData match)
    {
        var positions = _positions;

        ref var homePosition = ref positions[_map[match.HomeTeam]];
        ref var awayPosition = ref positions[_map[match.AwayTeam]];

        homePosition.AddMatch(in match);
        awayPosition.AddMatch(in match);
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
