using DotNetPerf.Domain;
using DotNetPerf.Domain.Outrights;

namespace DotNetPerf.Application.Outrights;

public sealed record CalculateOutrights(
    int Simulations,
    IEnumerable<Team> Teams
) : IRequest<Markets>;

public sealed class CalculateOutrightsHandler :
    IRequestHandler<CalculateOutrights, Markets>
{
    public ValueTask<Markets> Handle(CalculateOutrights request, CancellationToken cancellationToken)
    {
        var markets = Calculator.Run(request.Simulations, request.Teams);
        return new ValueTask<Markets>(markets);
    }
}
