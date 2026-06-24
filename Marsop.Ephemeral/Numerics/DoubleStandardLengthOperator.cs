using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Numerics;

/// <summary>
/// Provides a default length operator for intervals with <see cref="double"/> boundaries and lengths.
/// </summary>
public sealed class DoubleDefaultLengthOperator :
    ILengthOperator<double, double>
{
    private static readonly DoubleDefaultLengthOperator _instance = new();

    /// <summary>
    /// Gets the singleton instance of the <see cref="DoubleDefaultLengthOperator"/>.
    /// </summary>
    public static ILengthOperator<double, double> Instance => _instance;

    private DoubleDefaultLengthOperator()
    {
        // Private constructor to prevent external instantiation and enforce the singleton pattern.
    }

    /// <inheritdoc/>
    public double Apply(double boundary, double length) => boundary + length;

    /// <inheritdoc/>
    public double Measure(IBasicInterval<double> interval) => interval.End - interval.Start;

    /// <inheritdoc/>
    public double Zero() => 0.0;
}
