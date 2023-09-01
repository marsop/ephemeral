// <copyright file="Interval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;
using Optional;
using System;

namespace Marsop.Ephemeral.Implementation;

/// <summary>
/// Immutable Interval Base class
/// </summary>
public class Interval : GenericInterval<DateTimeOffset>, IInterval
{
    public TimeSpan Duration => End - Start;

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval" /> class
    /// </summary>
    /// <param name="start">starting <see cref="DateTimeOffset"/></param>
    /// <param name="duration">the interval duration</param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public Interval(DateTimeOffset start, TimeSpan duration, bool startIncluded, bool endIncluded)
        : base(start, start.Add(duration), startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval" /> class
    /// </summary>
    /// <param name="start">a <see cref="ITimestamped"/> instance representing the starting point</param>
    /// <param name="end">a <see cref="ITimestamped"/> instance representing the ending point</param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public Interval(ITimestamped start, ITimestamped end, bool startIncluded, bool endIncluded)
        : base(start.Timestamp, end.Timestamp, startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval" /> class
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public Interval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded)
        : base(start, end, startIncluded, endIncluded)
    { }

    /// <summary>
    /// Creates an interval with both start and end included
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="Interval"/> with both start and end included</returns>
    public static Interval CreateClosed(DateTimeOffset start, DateTimeOffset end) => new(start, end, true, true);

    /// <summary>
    /// Creates an interval with neither start or end included
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="Interval"/> with neither start or end included</returns>
    public static Interval CreateOpen(DateTimeOffset start, DateTimeOffset end) => new(start, end, false, false);

    /// <summary>
    /// Creates an interval with duration 0
    /// </summary>
    /// <param name="timestamp">the <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="Interval"/> with start and end point set with the given <see cref="DateTimeOffset"/></returns>
    public static Interval CreatePoint(DateTimeOffset timestamp) => CreateClosed(timestamp, timestamp);

    /// <summary>
    /// Intersect two intervals
    /// </summary>
    /// <param name="first">the first <see cref="IInterval"/> instance</param>
    /// <param name="second">the second <see cref="IInterval"/> instance</param>
    /// <returns>a new <see cref="Interval"/> if an intersection exists</returns>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static Option<Interval> Intersect(IInterval first, IInterval second) =>
        GenericInterval<DateTimeOffset>.Intersect(first, second).Map(x => x.ToInterval());

    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="first">the first <see cref="IInterval"/> instance</param>
    /// <param name="second">the second <see cref="IInterval"/> instance</param>
    /// <returns>a new <see cref="Interval"/> with joined intervals</returns>
    /// <exception cref="ArgumentException">an exception is thrown if the two intervals are not contiguous or overlapping</exception>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static Interval Join(IGenericInterval<DateTimeOffset> first, IGenericInterval<DateTimeOffset> second)
    {
        if (first is null)
        {
            throw new ArgumentNullException(nameof(first));
        }

        if (second is null)
        {
            throw new ArgumentNullException(nameof(second));
        }

        if (second.StartsBefore(first))
        {
            return Join(second, first);
        }

        if (first.Covers(second))
        {
            return first.ToGenericInterval().ToInterval();
        }

        if (first.Intersects(second) || first.IsContiguouslyFollowedBy(second))
        {
            return new Interval(first.Start, second.End, first.StartIncluded, second.EndIncluded);
        }

        throw new ArgumentException("the intervals are not overlapping or contiguous");
    }

    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="source">the source <see cref="IInterval"/> instance</param>
    /// <param name="subtraction">the subtraction <see cref="IInterval"/> instance</param>
    /// <returns>a list of <see cref="Interval"/> after subtraction</returns>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static IDisjointIntervalSet Subtract(IInterval source, IInterval subtraction)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (subtraction is null)
        {
            throw new ArgumentNullException(nameof(subtraction));
        }

        if (!source.Intersects(subtraction))
        {
            return new DisjointIntervalSet
            {
                source.ToInterval()
            };
        }

        if (subtraction.Covers(source))
        {
            return new DisjointIntervalSet();
        }

        var result = new DisjointIntervalSet();

        if (source.Start < subtraction.Start ||
            (source.Start == subtraction.Start && source.StartIncluded && !subtraction.StartIncluded))
            result.Add(new Interval(source.Start, subtraction.Start, source.StartIncluded, !subtraction.StartIncluded));
        if (source.End > subtraction.End ||
            (source.End == subtraction.End && source.EndIncluded && !subtraction.EndIncluded))
            result.Add(new Interval(subtraction.End, source.End, !subtraction.EndIncluded, source.EndIncluded));

        return result;
    }
}