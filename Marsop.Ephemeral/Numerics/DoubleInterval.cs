using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Numerics;

/// <summary>
/// Represents an interval of <see cref="double"/> values.
/// </summary>
public record DoubleInterval :
    FullInterval<double, double>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleInterval"/> class.
    /// </summary>
    /// <param name="start">The starting boundary of the interval.</param>
    /// <param name="end">The ending boundary of the interval.</param>
    /// <param name="startIncluded">A flag indicating whether the starting point is included. Defaults to true.</param>
    /// <param name="endIncluded">A flag indicating whether the ending point is included. Defaults to true.</param>
    public DoubleInterval(double start, double end, bool startIncluded = true, bool endIncluded = true)
        : base(start, end, startIncluded, endIncluded)
    {
    }

    /// <inheritdoc/>
    public override ILengthOperator<double, double> Operator => DoubleDefaultLengthOperator.Instance;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => base.ToString();

    /// <summary>
    /// Creates a closed double interval (both start and end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="DoubleInterval"/> with both boundaries included.</returns>
    public new static DoubleInterval CreateClosed(double start, double end) => new(start, end, true, true);

    /// <summary>
    /// Creates an open double interval (neither start nor end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="DoubleInterval"/> with neither boundary included.</returns>
    public new static DoubleInterval CreateOpen(double start, double end) => new(start, end, false, false);

    /// <summary>
    /// Creates a double interval representing a single point (start and end are the same and included).
    /// </summary>
    /// <param name="boundary">The boundary value for the point.</param>
    /// <returns>A new <see cref="DoubleInterval"/> representing a single point.</returns>
    public new static DoubleInterval CreatePoint(double boundary) => CreateClosed(boundary, boundary);
}