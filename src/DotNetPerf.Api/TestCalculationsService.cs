using System.Diagnostics;

using DotNetPerf.Api.Dtos;
using DotNetPerf.Application.Outrights;
using DotNetPerf.Domain;

public sealed class TestCalculationsService : BackgroundService
{
    private readonly ILogger<TestCalculationsService> _logger;
    private readonly IMediator _mediator;

    public TestCalculationsService(ILogger<TestCalculationsService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var inputJson = await File.ReadAllTextAsync("../../testinput.json");
        var inputDto = JsonSerializer.Deserialize<OutrightsCalculationRequestDto>(
            inputJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );

        if (inputDto is null)
            throw new Exception("Couldnt read testinput for calculation");

        var input = new CalculateOutrights(
            inputDto.Simulations,
            inputDto.Teams.Select(t => new Team(t.Name, t.ExpectedGoals))
        );

        var timer = new Stopwatch();

        input = input with { Simulations = 10_000 };
        for (int i = 0; i < 5 && !stoppingToken.IsCancellationRequested; i++)
        {
            timer.Start();
            _ = await _mediator.Send(input, stoppingToken);
            timer.Stop();
            _logger.LogInformation(
                "Calculated outrights - simulations={simulations} time={time}ms",
                input.Simulations,
                timer.Elapsed.TotalMilliseconds.ToString("0.00")
            );
            timer.Reset();
        }
    }
}
