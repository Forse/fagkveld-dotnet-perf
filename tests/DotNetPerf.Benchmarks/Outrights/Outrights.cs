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
[HardwareCounters(HardwareCounter.BranchInstructions, HardwareCounter.BranchMispredictions, HardwareCounter.CacheMisses)]
//[EtwProfiler]
//[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class Outrights
{
    private CalculateOutrights32bit _input_32bit;
    private CalculateOutrights64bit _input_64bit;

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

        _input_32bit = new CalculateOutrights32bit(
            input.Simulations,
            input.Teams.Select(t => new Team<float>(t.Name, (float)t.ExpectedGoals)).ToArray()
        );
        _input_64bit = new CalculateOutrights64bit(
            input.Simulations,
            input.Teams.Select(t => new Team<double>(t.Name, t.ExpectedGoals)).ToArray()
        );
    }

    [Benchmark]
    public Markets<float> Calculate_32bit() => Calculator<float>.Run(Simulations, _input_32bit.Teams);

    [Benchmark]
    public Markets<double> Calculate_64bit() => Calculator<double>.Run(Simulations, _input_64bit.Teams);
}