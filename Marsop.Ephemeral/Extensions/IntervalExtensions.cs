// <copyright file="IntervalExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Implementation;
using Marsop.Ephemeral.Interfaces;
using Optional;
using System;

namespace Marsop.Ephemeral.Extensions;

/// <summary>
/// Extension methods for <see cref="IInterval"/> objects
/// </summary>
public static class IntervalExtensions
{
    /// <summary>
    /// Gets the difference between start and end as <see cref="TimeSpan"/>
    /// </summary>
    public static TimeSpan Duration(this IInterval i) => i.End - i.Start;

    /// <summary>
    /// Verify if the interval covers the given <see cref="DateTimeOffset"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <param name="timestamp">the <see cref="DateTimeOffset"/></param>
    /// <returns><code>true</code> if the offset is covered by the interval, <code>false</code> otherwise</returns>
    public static bool Covers(this IInterval interval, DateTimeOffset timestamp)
    {
        if (timestamp < interval.Start)
        {
            return false;
        }

        if (interval.End < timestamp)
        {
            return false;
        }

        if (timestamp == interval.Start && !interval.StartIncluded)
        {
            return false;
        }

        if (timestamp == interval.End && !interval.EndIncluded)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Shifts the start and end of given <see cref="IInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <param name="shiftAmount">the amount to be shifted (positive => shift towards future)</param>
    /// <returns></returns>
    public static Interval Shift(this IInterval interval, TimeSpan shiftAmount) =>
        new(interval.Start + shiftAmount, interval.End + shiftAmount, interval.StartIncluded, interval.EndIncluded);

    /// <summary>
    /// Checks if the interval covers the given <see cref="IInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <param name="other">the <see cref="IInterval"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IInterval"/> is covered, <code>false</code> otherwise</returns>
    public static bool Covers(this IInterval interval, IInterval other) =>
        interval.Intersect(other).Match(x => x.ToInterval().Equals(other), () => false);

    /// <summary>
    /// Calculates the duration of the intersection between intervals
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="j">the <see cref="IInterval"/> instance in intersection</param>
    /// <returns>a <see cref="TimeSpan"/> object representing the duration of the intersection between the intervals, an empty <see cref="TimeSpan"/> if there is no intersection between the given <see cref="IInterval"/> instances</returns>
    public static TimeSpan DurationOfIntersect(this IInterval i, IInterval j) =>
        i.Intersect(j).Map(Duration).ValueOr(TimeSpan.Zero);

    /// <summary>
    /// Calculates duration as difference between actual UTC date time and the <see cref="IInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <returns>a <see cref="TimeSpan"/> object representing the duration if the <see cref="IInterval"/> is not ended, a <see cref="TimeSpan"/> representing the delay compared to the <see cref="IInterval"/> end</returns>
    public static TimeSpan DurationUntilNow(this IInterval interval, TimeProvider timeProvider)
    {
        var utcNow = timeProvider?.GetUtcNow() ?? throw new ArgumentNullException(nameof(timeProvider)); 
        return utcNow < interval.End ? interval.End - utcNow : interval.Duration();
    }

    /// <summary>
    /// Generates a new <see cref="Interval" />, which is the intersection of the two.
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <param name="other">the <see cref="IInterval"/> instance to intersect</param>
    /// <returns>a new <see cref="Interval"/> object representing the intersection between the two <see cref="IInterval"/> if an intersections exists, <code>null</code> otherwise</returns>
    public static Option<Interval> Intersect(this IInterval first, IInterval second)
    {
        if (first is null)
        {
            throw new ArgumentNullException(nameof(first));
        }

        if (second is null)
        {
            throw new ArgumentNullException(nameof(second));
        }

        var maxStart = first.Start < second.Start ? second.Start : first.Start;
        var minEnd = first.End < second.End ? first.End : second.End;

        if (minEnd < maxStart)
        {
            return Option.None<Interval>();
        }

        if (minEnd == maxStart && (!first.Covers(minEnd) || !second.Covers(minEnd)))
        {
            return Option.None<Interval>();
        }

        var startIncluded = first.Covers(maxStart) && second.Covers(maxStart);
        var endIncluded = first.Covers(minEnd) && second.Covers(minEnd);

        return new Interval(maxStart, minEnd, startIncluded, endIncluded).Some();
    }

    /// <summary>
    /// Checks if the interval intersects the given <see cref="IInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="j">the <see cref="IInterval"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IInterval"/> has an intersection with the current one, <code>false</code> otherwise</returns>
    public static bool Intersects(this IInterval i, IInterval j) =>
        i.Intersect(j).HasValue;

    /// <summary>
    /// Checks if the given <see cref="IInterval"/> follows seamlessly and without overlap the current <see cref="IInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="o">the <see cref="IInterval"/> instance to check</param>
    /// <returns><code>true</code> if the given <see cref="IInterval"/> is followed with the current one</returns>
    public static bool IsContiguouslyFollowedBy(this IInterval i, IInterval o) =>
        i.End == o.Start && (i.EndIncluded != o.StartIncluded);

    /// <summary>
    /// Checks if the current <see cref="IInterval"/> follows seamlessly and without overlap the given <see cref="IInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="o">the <see cref="IInterval"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IInterval"/> is preceded the the given <see cref="IInterval"/>, <code>false</code> otherwise</returns>
    public static bool IsContiguouslyPrecededBy(this IInterval i, IInterval o) =>
        o.IsContiguouslyFollowedBy(i);

    /// <summary>
    /// Checks if the current <see cref="IInterval"/> starts before the given <see cref="IInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <param name="other">the <see cref="IInterval"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IInterval"/> starts before the the given <see cref="IInterval"/>, <code>false</code> otherwise</returns>
    public static bool StartsBefore(this IInterval interval, IInterval other) =>
        interval.Start < other.Start || (interval.Start == other.Start && interval.StartIncluded && !other.StartIncluded);

    /// <summary>
    /// Creates an interval based on the information of this object
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <returns>a new <see cref="Interval"/> object</returns>
    public static Interval ToInterval(this IInterval interval) =>
        new(interval.Start, interval.End, interval.StartIncluded, interval.EndIncluded);

    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="first">the first <see cref="IInterval"/> instance</param>
    /// <param name="second">the second <see cref="IInterval"/> instance</param>
    /// <returns>a new <see cref="Interval"/> with joined intervals</returns>
    /// <exception cref="ArgumentException">an exception is thrown if the two intervals are not contiguous or overlapping</exception>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static Interval Join(this IInterval first, IInterval second)
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
            return first.ToInterval();
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
    public static DisjointIntervalSet Subtract(this IInterval source, IInterval subtraction)
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


    /// <summary>
    /// Combines two <see cref="IInterval"/> instances
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="j">the <see cref="IInterval"/> instance with which to merge</param>
    /// <returns>a <see cref="IDisjointIntervalSet"/> representing the list of joined <see cref="IInterval"/> instances</returns>
    public static DisjointIntervalSet Union(this IInterval i, IInterval j) => new DisjointIntervalSet(i, j);
}