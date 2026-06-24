using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Numerics;

/// <summary>
/// Represents an interval of <see cref="int"/> values.
/// </summary>
public record IntInterval :
    FullInterval<int, int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IntInterval"/> class.
    /// </summary>
    /// <param name="start">The starting boundary of the interval.</param>
    /// <param name="end">The ending boundary of the interval.</param>
    /// <param name="startIncluded">A flag indicating whether the starting point is included. Defaults to true.</param>
    /// <param name="endIncluded">A flag indicating whether the ending point is included. Defaults to true.</param>
    public IntInterval(int start, int end, bool startIncluded = true, bool endIncluded = true)
        : base(start, end, startIncluded, endIncluded)
    {
    }

    /// <inheritdoc/>
    public override ILengthOperator<int, int> Operator => IntDefaultLengthOperator.Instance;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => base.ToString();

    /// <summary>
    /// Creates a closed int interval (both start and end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="IntInterval"/> with both boundaries included.</returns>
    public new static IntInterval CreateClosed(int start, int end) => new(start, end, true, true);

    /// <summary>
    /// Creates an open int interval (neither start nor end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="IntInterval"/> with neither boundary included.</returns>
    public new static IntInterval CreateOpen(int start, int end) => new(start, end, false, false);

    /// <summary>
    /// Creates an int interval representing a single point (start and end are the same and included).
    /// </summary>
    /// <param name="boundary">The boundary value for the point.</param>
    /// <returns>A new <see cref="IntInterval"/> representing a single point.</returns>
    public new static IntInterval CreatePoint(int boundary) => CreateClosed(boundary, boundary);
}
