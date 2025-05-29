// <copyright file="StandardInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Extensions;
using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Temporal;

public record StandardInterval :
    AbstractMetricInterval<DateTimeOffset, TimeSpan>
{
    public StandardInterval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardInterval" /> class
    /// </summary>
    /// <param name="start">a <see cref="ITimestamped"/> instance representing the starting point</param>
    /// <param name="end">a <see cref="ITimestamped"/> instance representing the ending point</param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public StandardInterval(ITimestamped start, ITimestamped end, bool startIncluded, bool endIncluded) :
        this(start.Timestamp, end.Timestamp, startIncluded, endIncluded)
    {
    }

    private ILengthOperator<DateTimeOffset, TimeSpan> LengthOperator =>
        DateTimeOffsetStandardLengthOperator.Instance;

    public static StandardInterval CreateFrom(IBasicInterval<DateTimeOffset> interval)
    {
        return new StandardInterval(
            interval.Start,
            interval.End,
            interval.StartIncluded,
            interval.EndIncluded);
    }

    public DisjointStandardIntervalSet Subtract(StandardInterval other) =>
        new([.. IntervalExtensions.Subtract(this, other, LengthOperator)]);

    public StandardInterval Shift(TimeSpan offset) =>
        CreateFrom(IntervalExtensions.Shift(this, offset, LengthOperator));

    public TimeSpan LengthOfIntersect(StandardInterval other) =>
        IntervalExtensions.LengthOfIntersect(this, other, LengthOperator);

    public override TimeSpan Length() => LengthOperator.Measure(Start, End);
}
