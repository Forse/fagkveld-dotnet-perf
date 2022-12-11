using System.Text.Json;

using DotNetPerf.Api.Dtos;
using DotNetPerf.Application;
using DotNetPerf.Application.Outrights;
using DotNetPerf.Domain;

using Mediator;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace DotNetPerf.UnitTests;

public class CalculatorTests
{
    [Fact]
    public async Task Test_Calculation()
    {
        var sp = new ServiceCollection().AddApplication().BuildServiceProvider();

        var mediator = sp.GetRequiredService<IMediator>();

        var inputJson = await File.ReadAllTextAsync("testinput.json");
        var input = JsonSerializer.Deserialize<OutrightsCalculationRequestDto>(
            inputJson, 
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );

        Assert.NotNull(input);

        var markets = await mediator.Send(new CalculateOutrights(
            input!.Simulations,
            input.Teams.Select(t => new Team(t.Name, t.ExpectedGoals))
        ));

        Assert.NotNull(markets);
    }
}