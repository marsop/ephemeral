// <copyright file="StandardInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Temporal;

/// <summary>
/// Represents a temporal interval defined by <see cref="DateTimeOffset"/> boundaries and measured with <see cref="TimeSpan"/>.
/// </summary>
public record DateTimeOffsetInterval :
    FullInterval<DateTimeOffset, TimeSpan>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetInterval" /> class
    /// </summary>
    /// <param name="start">a <see cref="ITimestamped"/> instance representing the starting point</param>
    /// <param name="end">a <see cref="ITimestamped"/> instance representing the ending point</param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public DateTimeOffsetInterval(ITimestamped start, ITimestamped end, bool startIncluded, bool endIncluded) :
        this(start.Timestamp, end.Timestamp, startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetInterval" /> class.
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <param name="startIncluded">A flag indicating whether the starting point is included.</param>
    /// <param name="endIncluded">A flag indicating whether the ending point is included.</param>
    public DateTimeOffsetInterval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    /// <inheritdoc/>
    public override ILengthOperator<DateTimeOffset, TimeSpan> Operator =>
        DateTimeOffsetTimeSpanLengthOperator.Instance;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => base.ToString();

    /// <summary>
    /// Creates a closed date-time offset interval (both start and end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="DateTimeOffsetInterval"/> with both boundaries included.</returns>
    public new static DateTimeOffsetInterval CreateClosed(DateTimeOffset start, DateTimeOffset end) => new(start, end, true, true);

    /// <summary>
    /// Creates an open date-time offset interval (neither start nor end are included).
    /// </summary>
    /// <param name="start">The starting boundary.</param>
    /// <param name="end">The ending boundary.</param>
    /// <returns>A new <see cref="DateTimeOffsetInterval"/> with neither boundary included.</returns>
    public new static DateTimeOffsetInterval CreateOpen(DateTimeOffset start, DateTimeOffset end) => new(start, end, false, false);

    /// <summary>
    /// Creates a date-time offset interval representing a single point (start and end are the same and included).
    /// </summary>
    /// <param name="boundary">The boundary value for the point.</param>
    /// <returns>A new <see cref="DateTimeOffsetInterval"/> representing a single point.</returns>
    public new static DateTimeOffsetInterval CreatePoint(DateTimeOffset boundary) => CreateClosed(boundary, boundary);
}
