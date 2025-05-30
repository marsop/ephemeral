// <copyright file="StandardInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Temporal;

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

    public DateTimeOffsetInterval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    public override ILengthOperator<DateTimeOffset, TimeSpan> Operator =>
        DateTimeOffsetTimeSpanLengthOperator.Instance;

    public override string ToString() => base.ToString();

    public new static DateTimeOffsetInterval CreateClosed(DateTimeOffset start, DateTimeOffset end) => new(start, end, true, true);

    public new static DateTimeOffsetInterval CreateOpen(DateTimeOffset start, DateTimeOffset end) => new(start, end, false, false);

    public new static DateTimeOffsetInterval CreatePoint(DateTimeOffset boundary) => CreateClosed(boundary, boundary);
}
