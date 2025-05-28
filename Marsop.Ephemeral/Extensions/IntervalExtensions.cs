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
        i.Intersect(j).Match(x => x.Duration, () => TimeSpan.Zero);

    /// <summary>
    /// Calculates duration as difference between actual UTC date time and the <see cref="IInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <returns>a <see cref="TimeSpan"/> object representing the duration if the <see cref="IInterval"/> is not ended, a <see cref="TimeSpan"/> representing the delay compared to the <see cref="IInterval"/> end</returns>
    public static TimeSpan DurationUntilNow(this IInterval interval, TimeProvider timeProvider)
    {
        var utcNow = timeProvider?.GetUtcNow() ?? throw new ArgumentNullException(nameof(timeProvider)); 
        return utcNow < interval.End ? interval.End - utcNow : interval.Duration;
    }

    /// <summary>
    /// Generates a new <see cref="Interval" />, which is the intersection of the two.
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <param name="other">the <see cref="IInterval"/> instance to intersect</param>
    /// <returns>a new <see cref="Interval"/> object representing the intersection between the two <see cref="IInterval"/> if an intersections exists, <code>null</code> otherwise</returns>
    public static Option<IInterval> Intersect(this IInterval interval, IInterval other) =>
        Interval.Intersect(interval, other).Map(x => (IInterval)x);

    /// <summary>
    /// Checks if the interval intersects the given <see cref="IInterval"/>
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="j">the <see cref="IInterval"/> instance to verify</param>
    /// <returns><code>true</code> if the given <see cref="IInterval"/> has an intersection with the current one, <code>false</code> otherwise</returns>
    public static bool Intersects(this IInterval i, IInterval j) => i.Intersect(j).HasValue;

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
    /// Combines two <see cref="IInterval"/> instances
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="j">the <see cref="IInterval"/> instance with which to merge</param>
    /// <returns>a <see cref="IDisjointIntervalSet"/> representing the list of joined <see cref="IInterval"/> instances</returns>
    public static IDisjointIntervalSet Union(this IInterval i, IInterval j) => new DisjointIntervalSet(i, j);
}