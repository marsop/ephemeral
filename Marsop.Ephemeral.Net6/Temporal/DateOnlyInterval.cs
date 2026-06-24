using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Net6.Temporal;

/// <summary>
/// Represents a temporal interval defined by <see cref="DateOnly"/> boundaries and measured in days (<see cref="int"/>).
/// </summary>
public record DateOnlyInterval : FullInterval<DateOnly, int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateOnlyInterval"/> class.
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <param name="startIncluded">A flag indicating whether the starting point is included.</param>
    /// <param name="endIncluded">A flag indicating whether the ending point is included.</param>
    public DateOnlyInterval(DateOnly start, DateOnly end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    /// <inheritdoc/>
    public override ILengthOperator<DateOnly, int> Operator =>
        DateOnlyDaysLengthOperator.Instance;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => base.ToString();

    /// <summary>
    /// Creates a closed date-only interval (both start and end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="DateOnlyInterval"/> with both boundaries included.</returns>
    public new static DateOnlyInterval CreateClosed(DateOnly start, DateOnly end) => new(start, end, true, true);

    /// <summary>
    /// Creates an open date-only interval (neither start nor end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="DateOnlyInterval"/> with neither boundary included.</returns>
    public new static DateOnlyInterval CreateOpen(DateOnly start, DateOnly end) => new(start, end, false, false);

    /// <summary>
    /// Creates a date-only interval representing a single point (start and end are the same and included).
    /// </summary>
    /// <param name="boundary">The boundary value for the point.</param>
    /// <returns>A new <see cref="DateOnlyInterval"/> representing a single point.</returns>
    public new static DateOnlyInterval CreatePoint(DateOnly boundary) => CreateClosed(boundary, boundary);
}