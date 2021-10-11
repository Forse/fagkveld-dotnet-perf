namespace DotNetPerf.Domain.Outrights;

public ref struct Table
{
    private readonly Span<Position> _positions;
    private readonly Span<int> _indices;

    public int Count => _positions.Length;

    public ref readonly Position this[int index] => ref _positions[index];

    public Table(Span<Position> positions, Span<int> indices, ReadOnlySpan<TeamData> teams)
    {
        _positions = positions;
        _indices = indices;

        var positionIndex = 0;
        foreach (ref readonly var team in teams)
        {
            _positions[positionIndex] = new Position(team.Id);
            _indices[team.Id] = positionIndex++;
        }
    }

    public void AddResult(in MatchData match)
    {
        var positions = _positions;

        ref var homePosition = ref positions[_indices[match.HomeTeam]];
        ref var awayPosition = ref positions[_indices[match.AwayTeam]];

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
            _indices[position.Team] = i;
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
