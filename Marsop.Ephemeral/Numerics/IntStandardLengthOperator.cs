using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Numerics;

public sealed class IntDefaultLengthOperator :
    ILengthOperator<int, int>
{
    private static readonly IntDefaultLengthOperator _instance = new();

    public static ILengthOperator<int, int> Instance => _instance;

    // Required to enforce the singleton pattern by preventing external instantiation.
    private IntDefaultLengthOperator() { }

    public int Apply(int boundary, int length) => boundary + length;

    public int Measure(IBasicInterval<int> interval) => interval.End - interval.Start;

    public int Zero() => 0;
}