using DotNetPerf.Api.Dtos;
using DotNetPerf.Application.Outrights;
using DotNetPerf.Domain;

namespace DotNetPerf.Api.Controllers;

[ApiController]
public class OutrightsController : ApiControllerBase
{
    private readonly ILogger<OutrightsController> _logger;

    public OutrightsController(ILogger<OutrightsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<OutrightMarketDto>>> Calculate([FromBody] OutrightsCalculationRequestDto input)
    {
        var markets = await Mediator.Send(new CalculateOutrights32bit(
            input.Simulations,
            input.Teams.Select(t => new Team<float>(t.Name, (float)t.ExpectedGoals)).ToArray()
        ));

        var dto = markets
            .Select(m => new OutrightMarketDto(m.Type, m.Outcomes
            .Select(o => new OutrightOutcomeDto(o.Ref, o.Probability))))
            .ToArray();

        return Ok(dto);
    }
}
