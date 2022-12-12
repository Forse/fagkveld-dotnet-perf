using DotNetPerf.Domain;
using DotNetPerf.Domain.Outrights;

namespace DotNetPerf.Application.Outrights;

public sealed record CalculateOutrights32bit(
    int Simulations,
    IReadOnlyList<Team<float>> Teams
) : IRequest<Markets<float>>;

public sealed record CalculateOutrights64bit(
    int Simulations,
    IReadOnlyList<Team<double>> Teams
) : IRequest<Markets<double>>;

public sealed class CalculateOutrightsHandler :
    IRequestHandler<CalculateOutrights32bit, Markets<float>>,
    IRequestHandler<CalculateOutrights64bit, Markets<double>>
{
    public ValueTask<Markets<float>> Handle(CalculateOutrights32bit request, CancellationToken cancellationToken)
    {
        var markets = Calculator<float>.Run(request.Simulations, request.Teams);
        return new ValueTask<Markets<float>>(markets);
    }

    public ValueTask<Markets<double>> Handle(CalculateOutrights64bit request, CancellationToken cancellationToken)
    {
        var markets = Calculator<double>.Run(request.Simulations, request.Teams);
        return new ValueTask<Markets<double>>(markets);
    }
}
