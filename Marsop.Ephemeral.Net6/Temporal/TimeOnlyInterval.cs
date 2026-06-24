using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Net6.Temporal;

/// <summary>
/// Represents a temporal interval defined by <see cref="TimeOnly"/> boundaries and measured in <see cref="TimeSpan"/>.
/// </summary>
public record TimeOnlyInterval : FullInterval<TimeOnly, TimeSpan>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOnlyInterval"/> class.
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <param name="startIncluded">A flag indicating whether the starting point is included.</param>
    /// <param name="endIncluded">A flag indicating whether the ending point is included.</param>
    public TimeOnlyInterval(TimeOnly start, TimeOnly end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    /// <inheritdoc/>
    public override ILengthOperator<TimeOnly, TimeSpan> Operator =>
        TimeOnlyTimeSpanLengthOperator.Instance;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => base.ToString();

    /// <summary>
    /// Creates a closed time-only interval (both start and end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="TimeOnlyInterval"/> with both boundaries included.</returns>
    public new static TimeOnlyInterval CreateClosed(TimeOnly start, TimeOnly end) => new(start, end, true, true);

    /// <summary>
    /// Creates an open time-only interval (neither start nor end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="TimeOnlyInterval"/> with neither boundary included.</returns>
    public new static TimeOnlyInterval CreateOpen(TimeOnly start, TimeOnly end) => new(start, end, false, false);

    /// <summary>
    /// Creates a time-only interval representing a single point (start and end are the same and included).
    /// </summary>
    /// <param name="boundary">The boundary value for the point.</param>
    /// <returns>A new <see cref="TimeOnlyInterval"/> representing a single point.</returns>
    public new static TimeOnlyInterval CreatePoint(TimeOnly boundary) => CreateClosed(boundary, boundary);
}