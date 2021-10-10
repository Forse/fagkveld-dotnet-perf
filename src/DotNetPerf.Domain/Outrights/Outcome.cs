namespace DotNetPerf.Domain.Outrights;

public sealed record Outcome(string Ref, double Probability) :
    IComparable<Outcome>
{
    public int CompareTo(Outcome? other)
    {
        return other switch
        {
            null => -1,
            var o => o.Probability.CompareTo(Probability),
        };
    }
}
