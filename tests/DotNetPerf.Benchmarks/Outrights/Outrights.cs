using DotNetPerf.Api.Dtos;
using DotNetPerf.Application;
using DotNetPerf.Application.Outrights;
using DotNetPerf.Domain;
using DotNetPerf.Domain.Outrights;

using Mediator;

using Microsoft.Extensions.DependencyInjection;

namespace DotNetPerf.Benchmarks.Outrights;

[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 3)]
//[EtwProfiler]
//[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class Outrights
{
    private CalculateOutrights _input;

    [Params(1_000)]
    public int Simulations { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var sp = new ServiceCollection().AddApplication().BuildServiceProvider();

        var mediator = sp.GetRequiredService<IMediator>();

        var inputJson = await File.ReadAllTextAsync("testinput.json");

        var input = JsonSerializer.Deserialize<OutrightsCalculationRequestDto>(
            inputJson,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );

        _input = new CalculateOutrights(
            input.Simulations,
            input.Teams.Select(t => new Team(t.Name, t.ExpectedGoals))
        );
    }

    [Benchmark]
    public Markets Calculate() => Calculator.Run(Simulations, _input.Teams);
}