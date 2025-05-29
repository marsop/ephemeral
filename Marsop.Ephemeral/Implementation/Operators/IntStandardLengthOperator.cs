using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

public sealed class IntStandardLengthOperator :
    ILengthOperator<int, int>
{
    private static readonly IntStandardLengthOperator _instance = new();

    public static ILengthOperator<int, int> Instance => _instance;

    private IntStandardLengthOperator() { }

    public int Apply(int boundary, int length) => boundary + length;

    public int Measure(int boundary1, int boundary2) => boundary2 - boundary1;

    public int Zero() => 0;
}