using DotNetPerf.Domain.Outrights;

namespace DotNetPerf.Api.Dtos
{
    public sealed record OutrightsCalculationRequestDto(
        int Simulations,
        IEnumerable<TeamDto> Teams
    );

    public sealed record TeamDto(string Name, double ExpectedGoals);

    public sealed record OutrightMarketDto(MarketType Type, IEnumerable<OutrightOutcomeDto> Outcomes);

    public sealed record OutrightOutcomeDto(string Ref, double Probability);
}
