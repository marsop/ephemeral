// <copyright file="IntervalExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Implementation;
using Marsop.Ephemeral.Interfaces;
using Optional;
using System;

namespace Marsop.Ephemeral.Extensions;


/// <summary>
/// Extension methods for <see cref="IDateTimeOffsetInterval"/> objects
/// </summary>
public static class IntervalExtensions
{
    /// <summary>
    /// Gets the length of the interval
    /// </summary>
    public static TLength Duration<TInterval, TBoundary, TLength>(this TInterval i)
        where TInterval : IHasLength<TLength>, IBasicInterval<TBoundary>
        where TBoundary : notnull, IComparable<TBoundary>
        where TLength : notnull, IComparable<TLength>
    {
        return i.Length();
    }

    /// <summary>
    /// Verify if the interval covers the given boundary
    /// </summary>
    /// <param name="interval">the current interval</param>
    /// <param name="boundary">the boundary</param>
    /// <returns><code>true</code> if the boundary is covered by the interval, <code>false</code> otherwise</returns>
    public static bool Covers<TBoundary>(
        this IBasicInterval<TBoundary> interval,
        TBoundary boundary)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        if (boundary.IsLessThan(interval.Start))
        {
            return false;
        }

        if (interval.End.IsLessThan(boundary))
        {
            return false;
        }

        if (boundary.IsEqualTo(interval.Start) && !interval.StartIncluded)
        {
            return false;
        }

        if (boundary.IsEqualTo(interval.End) && !interval.EndIncluded)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Shifts the start and end of given <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="shiftAmount">the amount to be shifted (positive => shift towards future)</param>
    /// <returns></returns>
    public static TInterval Shift<TInterval, TBoundary, TLength>(
        this TInterval interval,
        TLength shiftAmount)
        where TInterval : IInterval<TBoundary, TLength>, IIntervalFactory<TInterval, TBoundary>
        where TBoundary : notnull, IComparable<TBoundary>
        where TLength : notnull, IComparable<TLength>

    {
        return Shift(interval, shiftAmount, interval);
    }

    public static TOut Shift<TIn, TBoundary, TLength, TOut>(
        this TIn interval,
        TLength shiftAmount,
        IIntervalFactory<TOut, TBoundary> factory)
        where TIn : IInterval<TBoundary, TLength>
        where TOut : IInterval<TBoundary, TLength>
        where TBoundary : notnull, IComparable<TBoundary>
        where TLength : notnull, IComparable<TLength>

    {
        var newStart = interval.Apply(interval.Start, shiftAmount);
        var newEnd = interval.Apply(interval.End, shiftAmount);
        
        BasicInterval<TBoundary> output = new(newStart, newEnd, interval.StartIncluded, interval.EndIncluded);
        return factory.Create(output);
    }

    /// <summary>
    /// Checks if the interval covers the given <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="other">the <see cref="IDateTimeOffsetInterval"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IDateTimeOffsetInterval"/> is covered, <code>false</code> otherwise</returns>
    public static bool Covers(this IDateTimeOffsetInterval interval, IDateTimeOffsetInterval other) =>
        interval.Intersect(other).Match(x => x.ToInterval().Equals(other), () => false);

    /// <summary>
    /// Calculates the duration of the intersection between intervals
    /// </summary>
    /// <param name="i">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="j">the <see cref="IDateTimeOffsetInterval"/> instance in intersection</param>
    /// <returns>a <see cref="TimeSpan"/> object representing the duration of the intersection between the intervals, an empty <see cref="TimeSpan"/> if there is no intersection between the given <see cref="IDateTimeOffsetInterval"/> instances</returns>
    public static TimeSpan DurationOfIntersect(this IDateTimeOffsetInterval i, IDateTimeOffsetInterval j) =>
        i.Intersect(j).Map(x => x.Length()).ValueOr(TimeSpan.Zero);

    /// <summary>
    /// Calculates duration as difference between actual UTC date time and the <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <returns>a <see cref="TimeSpan"/> object representing the duration if the <see cref="IDateTimeOffsetInterval"/> is not ended, a <see cref="TimeSpan"/> representing the delay compared to the <see cref="IDateTimeOffsetInterval"/> end</returns>
    public static TimeSpan DurationUntilNow(this IDateTimeOffsetInterval interval, TimeProvider timeProvider)
    {
        var utcNow = timeProvider?.GetUtcNow() ?? throw new ArgumentNullException(nameof(timeProvider));
        return utcNow.IsLessThan(interval.End) ? interval.End - utcNow : interval.Length();
    }

    /// <summary>
    /// Generates a new <see cref="DateTimeOffsetInterval" />, which is the intersection of the two.
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="other">the <see cref="IDateTimeOffsetInterval"/> instance to intersect</param>
    /// <returns>a new <see cref="DateTimeOffsetInterval"/> object representing the intersection between the two <see cref="IDateTimeOffsetInterval"/> if an intersections exists, <code>null</code> otherwise</returns>
    public static Option<DateTimeOffsetInterval> Intersect(this IDateTimeOffsetInterval first, IDateTimeOffsetInterval second)
    {
        if (first is null)
        {
            throw new ArgumentNullException(nameof(first));
        }

        if (second is null)
        {
            throw new ArgumentNullException(nameof(second));
        }

        var maxStart = first.Start.IsLessThan(second.Start) ? second.Start : first.Start;
        var minEnd = first.End.IsLessThan(second.End) ? first.End : second.End;

        if (minEnd.IsLessThan(maxStart))
        {
            return Option.None<DateTimeOffsetInterval>();
        }

        if (minEnd.IsEqualTo(maxStart) && (!first.Covers(minEnd) || !second.Covers(minEnd)))
        {
            return Option.None<DateTimeOffsetInterval>();
        }

        var startIncluded = first.Covers(maxStart) && second.Covers(maxStart);
        var endIncluded = first.Covers(minEnd) && second.Covers(minEnd);

        return new DateTimeOffsetInterval(maxStart, minEnd, startIncluded, endIncluded).Some();
    }

    /// <summary>
    /// Checks if the interval intersects the given <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="j">the <see cref="IDateTimeOffsetInterval"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IDateTimeOffsetInterval"/> has an intersection with the current one, <code>false</code> otherwise</returns>
    public static bool Intersects(this IDateTimeOffsetInterval i, IDateTimeOffsetInterval j) =>
        i.Intersect(j).HasValue;

    /// <summary>
    /// Checks if the given <see cref="IDateTimeOffsetInterval"/> follows seamlessly and without overlap the current <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="o">the <see cref="IDateTimeOffsetInterval"/> instance to check</param>
    /// <returns><code>true</code> if the given <see cref="IDateTimeOffsetInterval"/> is followed with the current one</returns>
    public static bool IsContiguouslyFollowedBy(this IDateTimeOffsetInterval i, IDateTimeOffsetInterval o) =>
        i.End.IsEqualTo(o.Start) && (i.EndIncluded != o.StartIncluded);

    /// <summary>
    /// Checks if the current <see cref="IDateTimeOffsetInterval"/> follows seamlessly and without overlap the given <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="o">the <see cref="IDateTimeOffsetInterval"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IDateTimeOffsetInterval"/> is preceded the the given <see cref="IDateTimeOffsetInterval"/>, <code>false</code> otherwise</returns>
    public static bool IsContiguouslyPrecededBy(this IDateTimeOffsetInterval i, IDateTimeOffsetInterval o) =>
        o.IsContiguouslyFollowedBy(i);

    /// <summary>
    /// Checks if the current <see cref="IDateTimeOffsetInterval"/> starts before the given <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="other">the <see cref="IDateTimeOffsetInterval"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IDateTimeOffsetInterval"/> starts before the the given <see cref="IDateTimeOffsetInterval"/>, <code>false</code> otherwise</returns>
    public static bool StartsBefore(this IDateTimeOffsetInterval interval, IDateTimeOffsetInterval other) =>
        interval.Start.IsLessThan(other.Start) || (interval.Start.IsEqualTo(other.Start) && interval.StartIncluded && !other.StartIncluded);

    /// <summary>
    /// Creates an interval based on the information of this object
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <returns>a new <see cref="DateTimeOffsetInterval"/> object</returns>
    public static DateTimeOffsetInterval ToInterval(this IDateTimeOffsetInterval interval) =>
        new(interval.Start, interval.End, interval.StartIncluded, interval.EndIncluded);

    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="first">the first <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="second">the second <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <returns>a new <see cref="DateTimeOffsetInterval"/> with joined intervals</returns>
    /// <exception cref="ArgumentException">an exception is thrown if the two intervals are not contiguous or overlapping</exception>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static DateTimeOffsetInterval Join(this IDateTimeOffsetInterval first, IDateTimeOffsetInterval second)
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
            return new DateTimeOffsetInterval(first.Start, second.End, first.StartIncluded, second.EndIncluded);
        }

        throw new ArgumentException("the intervals are not overlapping or contiguous");
    }



    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="source">the source <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="subtraction">the subtraction <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <returns>a list of <see cref="DateTimeOffsetInterval"/> after subtraction</returns>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static DisjointIntervalSet Subtract(this IDateTimeOffsetInterval source, IDateTimeOffsetInterval subtraction)
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

        if (source.Start.IsLessThan(subtraction.Start) ||
            (source.Start.IsEqualTo(subtraction.Start) && source.StartIncluded && !subtraction.StartIncluded))
            result.Add(new DateTimeOffsetInterval(source.Start, subtraction.Start, source.StartIncluded, !subtraction.StartIncluded));
        if (source.End.IsGreaterThan(subtraction.End) ||
            (source.End.IsEqualTo(subtraction.End) && source.EndIncluded && !subtraction.EndIncluded))
            result.Add(new DateTimeOffsetInterval(subtraction.End, source.End, !subtraction.EndIncluded, source.EndIncluded));

        return result;
    }


    /// <summary>
    /// Combines two <see cref="IDateTimeOffsetInterval"/> instances
    /// </summary>
    /// <param name="i">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="j">the <see cref="IDateTimeOffsetInterval"/> instance with which to merge</param>
    /// <returns>a <see cref="IDisjointIntervalSet"/> representing the list of joined <see cref="IDateTimeOffsetInterval"/> instances</returns>
    public static DisjointIntervalSet Union(this IDateTimeOffsetInterval i, IDateTimeOffsetInterval j) => new DisjointIntervalSet(i, j);
}