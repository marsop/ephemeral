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
    public static bool IsEquivalentIntervalTo<TBoundary>(
        this IBasicInterval<TBoundary> interval,
        IBasicInterval<TBoundary> other)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        if (interval is null)
        {
            throw new ArgumentNullException(nameof(interval));
        }

        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return interval.Start.IsEqualTo(other.Start) &&
               interval.End.IsEqualTo(other.End) &&
               interval.StartIncluded == other.StartIncluded &&
               interval.EndIncluded == other.EndIncluded;
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
    /// Shifts the start and end of given <see cref="IInterval<>"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval<>"/> instance</param>
    /// <param name="shiftLength">the amount to be shifted (positive or negative)</param>
    /// <returns></returns>
    public static BasicInterval<TBoundary> Shift<TBoundary, TLength>(
        this IInterval<TBoundary, TLength> interval,
        TLength shiftLength)
        where TBoundary : notnull, IComparable<TBoundary>
        where TLength : notnull, IComparable<TLength>
    {
        var newStart = interval.LengthOperator.Apply(interval.Start, shiftLength);
        var newEnd = interval.LengthOperator.Apply(interval.End, shiftLength);

        return new BasicInterval<TBoundary>(newStart, newEnd, interval.StartIncluded, interval.EndIncluded);
    }

    /// <summary>
    /// Checks if the interval covers the given interval
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="other">the <see cref="IDateTimeOffsetInterval"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IDateTimeOffsetInterval"/> is covered, <code>false</code> otherwise</returns>
    public static bool Covers<TBoundary>(
        this IBasicInterval<TBoundary> interval,
         IBasicInterval<TBoundary> other)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return interval
        .Intersect(other)
        .Match(
            x => x.IsEquivalentIntervalTo(other),
            () => false);
    }

    /// <summary>
    /// Calculates the length of the intersection between intervals
    /// </summary>
    /// <param name="interval">the current <see cref="IBasicInterval<>"/> instance</param>
    /// <param name="other">the <see cref="IBasicInterval<>"/> instance in intersection</param>
    /// <returns>an object representing the length of the intersection between the intervals, an empty <see cref="TimeSpan"/> if there is no intersection between the given <see cref="IDateTimeOffsetInterval"/> instances</returns>
    public static TLength LengthOfIntersect<TBoundary, TLength>(
        this IBasicInterval<TBoundary> interval,
        IBasicInterval<TBoundary> other,
        ILengthOperator<TBoundary, TLength> lengthOperator)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return interval
        .Intersect(other)
        .Map(lengthOperator.MeasureInterval)
        .ValueOr(lengthOperator.Zero());
    }

    /// <summary>
    /// Generates a new <see cref="BasicInterval<TBoundary>" />, which is the intersection of the two.
    /// </summary>
    /// <param name="interval">the current interval</param>
    /// <param name="other">the <see cref="IBasicInterval<TBoundary>"/> instance to intersect</param>
    /// <returns>a new <see cref="BasicInterval<TBoundary>"/> object representing the intersection between the two <see cref="IBasicInterval<TBoundary>"/> if an intersections exists, <code>null</code> otherwise</returns>
    public static Option<BasicInterval<TBoundary>> Intersect<TBoundary>(
        this IBasicInterval<TBoundary> first,
        IBasicInterval<TBoundary> second)
        where TBoundary : notnull, IComparable<TBoundary>
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
            return Option.None<BasicInterval<TBoundary>>();
        }

        if (minEnd.IsEqualTo(maxStart) && (!first.Covers(minEnd) || !second.Covers(minEnd)))
        {
            return Option.None<BasicInterval<TBoundary>>();
        }

        var startIncluded = first.Covers(maxStart) && second.Covers(maxStart);
        var endIncluded = first.Covers(minEnd) && second.Covers(minEnd);

        return new BasicInterval<TBoundary>(maxStart, minEnd, startIncluded, endIncluded).Some();
    }

    /// <summary>
    /// Checks if the interval intersects the given <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="j">the <see cref="IDateTimeOffsetInterval"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IDateTimeOffsetInterval"/> has an intersection with the current one, <code>false</code> otherwise</returns>
    public static bool Intersects<TBoundary>(
        this IBasicInterval<TBoundary> first,
        IBasicInterval<TBoundary> second)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return first.Intersect(second).HasValue;
    }

    /// <summary>
    /// Checks if the given <see cref="IDateTimeOffsetInterval"/> follows seamlessly and without overlap the current <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="o">the <see cref="IDateTimeOffsetInterval"/> instance to check</param>
    /// <returns><code>true</code> if the given <see cref="IDateTimeOffsetInterval"/> is followed with the current one</returns>
    public static bool IsContiguouslyFollowedBy<TBoundary>(
        this IBasicInterval<TBoundary> first,
        IBasicInterval<TBoundary> second)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return first.End.IsEqualTo(second.Start) && (first.EndIncluded != second.StartIncluded);
    }

    /// <summary>
    /// Checks if the current <see cref="IDateTimeOffsetInterval"/> follows seamlessly and without overlap the given <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="o">the <see cref="IDateTimeOffsetInterval"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IDateTimeOffsetInterval"/> is preceded the the given <see cref="IDateTimeOffsetInterval"/>, <code>false</code> otherwise</returns>
    public static bool IsContiguouslyPrecededBy<TBoundary>(
        this IBasicInterval<TBoundary> first,
        IBasicInterval<TBoundary> second)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return second.IsContiguouslyFollowedBy(first);
    }

    /// <summary>
    /// Checks if the current <see cref="IDateTimeOffsetInterval"/> starts before the given <see cref="IDateTimeOffsetInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="other">the <see cref="IDateTimeOffsetInterval"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IDateTimeOffsetInterval"/> starts before the the given <see cref="IDateTimeOffsetInterval"/>, <code>false</code> otherwise</returns>
    public static bool StartsBefore<TBoundary>(
        this IBasicInterval<TBoundary> interval,
        IBasicInterval<TBoundary> other)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return interval.Start.IsLessThan(other.Start) || (interval.Start.IsEqualTo(other.Start) && interval.StartIncluded && !other.StartIncluded);
    }

    /// <summary>
    /// Creates an interval based on the information of this object
    /// </summary>
    /// <param name="interval">the current <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <returns>a new <see cref="StandardInterval"/> object</returns>
    public static StandardInterval ToInterval(this IDateTimeOffsetInterval interval) =>
        new(interval.Start, interval.End, interval.StartIncluded, interval.EndIncluded);

    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="first">the first <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="second">the second <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <returns>a new <see cref="StandardInterval"/> with joined intervals</returns>
    /// <exception cref="ArgumentException">an exception is thrown if the two intervals are not contiguous or overlapping</exception>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static BasicInterval<TBoundary> Join<TBoundary>(
        this IBasicInterval<TBoundary> first,
        IBasicInterval<TBoundary> second)
        where TBoundary : notnull, IComparable<TBoundary>
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
            return BasicInterval<TBoundary>.From(first);
        }

        if (first.Intersects(second) || first.IsContiguouslyFollowedBy(second))
        {
            return new BasicInterval<TBoundary>(first.Start, second.End, first.StartIncluded, second.EndIncluded);
        }

        throw new ArgumentException("the intervals are not overlapping or contiguous");
    }



    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="source">the source <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <param name="subtraction">the subtraction <see cref="IDateTimeOffsetInterval"/> instance</param>
    /// <returns>a list of <see cref="StandardInterval"/> after subtraction</returns>
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
            result.Add(new StandardInterval(source.Start, subtraction.Start, source.StartIncluded, !subtraction.StartIncluded));
        if (source.End.IsGreaterThan(subtraction.End) ||
            (source.End.IsEqualTo(subtraction.End) && source.EndIncluded && !subtraction.EndIncluded))
            result.Add(new StandardInterval(subtraction.End, source.End, !subtraction.EndIncluded, source.EndIncluded));

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