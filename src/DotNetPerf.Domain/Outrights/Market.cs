namespace DotNetPerf.Domain.Outrights;

public sealed record Market(MarketType Type, IEnumerable<Outcome> Outcomes);
