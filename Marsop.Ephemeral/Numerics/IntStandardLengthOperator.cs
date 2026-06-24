using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Numerics;

/// <summary>
/// Provides a default length operator for intervals with <see cref="int"/> boundaries and lengths.
/// </summary>
public sealed class IntDefaultLengthOperator :
    ILengthOperator<int, int>
{
    private static readonly IntDefaultLengthOperator _instance = new();

    /// <summary>
    /// Gets the singleton instance of the <see cref="IntDefaultLengthOperator"/>.
    /// </summary>
    public static ILengthOperator<int, int> Instance => _instance;

    private IntDefaultLengthOperator()
    {
        // Private constructor to prevent external instantiation and enforce the singleton pattern.
    }

    /// <inheritdoc/>
    public int Apply(int boundary, int length) => boundary + length;

    /// <inheritdoc/>
    public int Measure(IBasicInterval<int> interval) => interval.End - interval.Start;

    /// <inheritdoc/>
    public int Zero() => 0;
}