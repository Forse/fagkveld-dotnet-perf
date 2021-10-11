using System.Diagnostics;

namespace DotNetPerf.Domain;

public static class Diagnostics
{
    public static readonly ActivitySource ActivitySource = new ActivitySource("DotNetPerf");
}
