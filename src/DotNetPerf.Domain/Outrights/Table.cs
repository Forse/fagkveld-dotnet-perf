namespace DotNetPerf.Domain.Outrights;

public sealed record Table
{
    private readonly List<Position> _positions;

    public int Count => _positions.Count;

    public Position this[int index] => _positions[index];

    public Table(IEnumerable<Team> teams)
    {
        _positions = new (teams.Count());
        foreach (var team in teams)
            _positions.Add(new Position(team));
    }

    public void AddResult(Match match)
    {
        var homePosition = _positions.Single(p => p?.Team == match.HomeTeam);
        var awayPosition = _positions.Single(p => p?.Team == match.AwayTeam);

        homePosition.AddMatch(match);
        awayPosition.AddMatch(match);

        _positions.Sort();
    }

    public List<Position>.Enumerator GetEnumerator() => _positions.GetEnumerator();

    public void Reset()
    {
        foreach (var position in _positions)
            position.Reset();
    }
}
