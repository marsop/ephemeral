// <copyright file="IntervalExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Implementation;
using Marsop.Ephemeral.Interfaces;
using System;

namespace Marsop.Ephemeral.Extensions;

/// <summary>
/// Extension methods for <see cref="IInterval"/> objects
/// </summary>
public static class IntervalExtensions
{
    /// <summary>
    /// Shifts the start and end of given <see cref="IInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <param name="shiftAmount">the amount to be shifted (positive => shift towards future)</param>
    /// <returns></returns>
    public static Interval Shift(this IInterval interval, TimeSpan shiftAmount) =>
        new(interval.Start + shiftAmount, interval.End + shiftAmount, interval.StartIncluded, interval.EndIncluded);

    /// <summary>
    /// Calculates the duration of the intersection between intervals
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="j">the <see cref="IInterval"/> instance in intersection</param>
    /// <returns>a <see cref="TimeSpan"/> object representing the duration of the intersection between the intervals, an empty <see cref="TimeSpan"/> if there is no intersection between the given <see cref="IInterval"/> instances</returns>
    public static TimeSpan DurationOfIntersect(this IInterval i, IInterval j) =>
        Interval.Intersect(i, j).Match(x => x.Duration, () => TimeSpan.Zero);

    /// <summary>
    /// Calculates duration as difference between actual UTC date time and the <see cref="IInterval"/>
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <returns>a <see cref="TimeSpan"/> object representing the duration if the <see cref="IInterval"/> is not ended, a <see cref="TimeSpan"/> representing the delay compared to the <see cref="IInterval"/> end</returns>
    public static TimeSpan DurationUntilNow(this IInterval interval) =>
        DateTimeOffset.UtcNow < interval.End ? interval.End - DateTimeOffset.UtcNow : interval.Duration;

    /// <summary>
    /// Creates an interval based on the information of this object
    /// </summary>
    /// <param name="interval">the current <see cref="IInterval"/> instance</param>
    /// <returns>a new <see cref="Interval"/> object</returns>
    public static Interval ToInterval(this IGenericInterval<DateTimeOffset> interval) =>
        new(interval.Start, interval.End, interval.StartIncluded, interval.EndIncluded);

    /// <summary>
    /// Combines two <see cref="IInterval"/> instances
    /// </summary>
    /// <param name="i">the current <see cref="IInterval"/> instance</param>
    /// <param name="j">the <see cref="IInterval"/> instance with which to merge</param>
    /// <returns>a <see cref="IDisjointIntervalSet"/> representing the list of joined <see cref="IInterval"/> instances</returns>
    public static IDisjointIntervalSet Union(this IInterval i, IInterval j) => new DisjointIntervalSet(i, j);

    /// <summary>
    /// Join two intervals
    /// </summary>
    /// <param name="first">the first <see cref="IInterval"/> instance</param>
    /// <param name="second">the second <see cref="IInterval"/> instance</param>
    /// <returns>a new <see cref="Interval"/> with joined intervals</returns>
    /// <exception cref="ArgumentException">an exception is thrown if the two intervals are not contiguous or overlapping</exception>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public static Interval Join(this IGenericInterval<DateTimeOffset> first, IGenericInterval<DateTimeOffset> second) =>
        first.Join(second).ToInterval();
}