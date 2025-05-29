using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Numerics;

public sealed class IntDefaultLengthOperator :
    ILengthOperator<int, int>
{
    private static readonly IntDefaultLengthOperator _instance = new();

    public static ILengthOperator<int, int> Instance => _instance;

    private IntDefaultLengthOperator() { }

    public int Apply(int boundary, int length) => boundary + length;

    public int Measure(int boundary1, int boundary2) => boundary2 - boundary1;

    public int Zero() => 0;
}