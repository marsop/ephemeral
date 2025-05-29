using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Numerics;

public sealed class DoubleDefaultLengthOperator :
    ILengthOperator<double, double>
{
    private static readonly DoubleDefaultLengthOperator _instance = new();

    public static ILengthOperator<double, double> Instance => _instance;

    private DoubleDefaultLengthOperator() { }

    public double Apply(double boundary, double length) => boundary + length;

    public double Measure(double boundary1, double boundary2) => boundary2 - boundary1;

    public double Zero() => 0.0;
}
