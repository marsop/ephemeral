using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Numerics;

public sealed class DoubleDefaultLengthOperator :
    ILengthOperator<double, double>
{
    private static readonly DoubleDefaultLengthOperator _instance = new();

    public static ILengthOperator<double, double> Instance => _instance;

    private DoubleDefaultLengthOperator() { }

    public double Apply(double boundary, double length) => boundary + length;

    public double Measure(IBasicInterval<double> interval) => interval.End - interval.Start;

    public double Zero() => 0.0;
}
