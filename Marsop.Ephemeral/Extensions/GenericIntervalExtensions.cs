// <copyright file="IntervalExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Implementation;
using Marsop.Ephemeral.Interfaces;
using Optional;
using System;

namespace Marsop.Ephemeral.Extensions;

/// <summary>
/// Extension methods for <see cref="IGenericInterval{T}"/> /> objects
/// </summary>
public static class GenericIntervalExtensions
{
    /// <summary>
    /// Verify if the interval covers the given boundary/>
    /// </summary>
    /// <param name="interval">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="boundary">the boundary</param>
    /// <returns><code>true</code> if the offset is covered by the <see cref="IGenericInterval{T}"/>, <code>false</code> otherwise</returns>
    public static bool Covers<T>(this IGenericInterval<T> interval, T boundary)
        where T : IComparable<T>
    {
        if (boundary.IsLessThan(interval.Start))
        {
            return false;
        }

        if (boundary.IsGreaterThan(interval.End))
        {
            return false;
        }

        if (boundary.Equals(interval.Start) && !interval.StartIncluded)
        {
            return false;
        }

        if (boundary.Equals(interval.End) && !interval.EndIncluded)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the interval covers the given <see cref="IGenericInterval{T}"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="other">the <see cref="IGenericInterval{T}"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IGenericInterval{T}"/> is covered, <code>false</code> otherwise</returns>
    public static bool Covers<T>(this IGenericInterval<T> interval, IGenericInterval<T> other)
        where T : IComparable<T> =>
        interval.Intersect(other).Match(x => x.EquivalentTo(other), () => false);

    /// <summary>
    /// Generates a new <see cref="GenericInterval{T}" />, which is the intersection of the two.
    /// </summary>
    /// <param name="interval">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="other">the <see cref="IGenericInterval{T}"/> instance to intersect</param>
    /// <returns>a new <see cref="Interval"/> object representing the intersection between the two <see cref="IGenericInterval{T}"/> if an intersections exists, <code>null</code> otherwise</returns>
    public static Option<IGenericInterval<T>> Intersect<T>(this IGenericInterval<T> interval, IGenericInterval<T> other)
        where T : IComparable<T> =>
        GenericInterval<T>.Intersect(interval, other).Map(x => (IGenericInterval<T>)x);

    /// <summary>
    /// Checks if the interval intersects the given <see cref="IGenericInterval{T}"/>
    /// </summary>
    /// <param name="i">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="j">the <see cref="IGenericInterval{T}"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IGenericInterval{T}"/> has an intersection with the current one, <code>false</code> otherwise</returns>
    public static bool Intersects<T>(this IGenericInterval<T> i, IGenericInterval<T> j)
        where T : IComparable<T> =>
        i.Intersect(j).HasValue;

    /// <summary>
    /// Creates an interval based on the information of this object
    /// </summary>
    /// <param name="interval">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <returns>a new <see cref="Interval"/> object</returns>
    public static GenericInterval<T> ToGenericInterval<T>(this IGenericInterval<T> interval)
        where T : IComparable<T> =>
        new(interval.Start, interval.End, interval.StartIncluded, interval.EndIncluded);

    /// <summary>
    /// Checks if the given <see cref="IGenericInterval{T}"/> follows seamlessly and without overlap the current <see cref="IGenericInterval{T}"/>
    /// </summary>
    /// <param name="i">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="o">the <see cref="IGenericInterval{T}"/> instance to check</param>
    /// <returns><code>true</code> if the given <see cref="IGenericInterval{T}"/> is followed with the current one</returns>
    public static bool IsContiguouslyFollowedBy<T>(this IGenericInterval<T> i, IGenericInterval<T> o)
        where T : IComparable<T> =>
        i.End.Equals(o.Start) && (i.EndIncluded != o.StartIncluded);

    /// <summary>
    /// Checks if the current <see cref="IGenericInterval{T}"/> follows seamlessly and without overlap the given <see cref="IGenericInterval{T}"/>
    /// </summary>
    /// <param name="i">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="o">the <see cref="IGenericInterval{T}"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IGenericInterval{T}"/> is preceded the the given <see cref="IGenericInterval{T}"/>, <code>false</code> otherwise</returns>
    public static bool IsContiguouslyPrecededBy<T>(this IGenericInterval<T> i, IGenericInterval<T> o)
        where T : IComparable<T> =>
        o.IsContiguouslyFollowedBy(i);

    /// <summary>
    /// Checks if the current <see cref="IGenericInterval{T}"/> starts before the given <see cref="IGenericInterval{T}"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="other">the <see cref="IGenericInterval{T}"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IGenericInterval{T}"/> starts before the the given <see cref="IGenericInterval{T}"/>, <code>false</code> otherwise</returns>
    public static bool StartsBefore<T>(this IGenericInterval<T> interval, IGenericInterval<T> other)
        where T : IComparable<T> =>
        interval.Start.IsLessThan(other.Start) || (interval.Start.Equals(other.Start) && interval.StartIncluded && !other.StartIncluded);

    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="first">the first <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="second">the second <see cref="IGenericInterval{T}"/> instance</param>
    /// <returns>a new <see cref="GenericInterval{T}"/> with joined intervals</returns>
    /// <exception cref="ArgumentException">an exception is thrown if the two intervals are not contiguous or overlapping</exception>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static GenericInterval<T> Join<T>(this IGenericInterval<T> first, IGenericInterval<T> second)
        where T : IComparable<T>
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
            return first.ToGenericInterval();
        }

        if (first.Intersects(second) || first.IsContiguouslyFollowedBy(second))
        {
            return new GenericInterval<T>(first.Start, second.End, first.StartIncluded, second.EndIncluded);
        }

        throw new ArgumentException("the intervals are not overlapping or contiguous");
    }
}