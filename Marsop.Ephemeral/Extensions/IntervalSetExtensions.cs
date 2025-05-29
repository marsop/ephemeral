// <copyright file="IntervalSetExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Implementation;
using Marsop.Ephemeral.Interfaces;
using Optional.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marsop.Ephemeral.Extensions;


/// <summary>
/// Extensions method for <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instances
/// </summary>
public static class IntervalSetExtensions
{
    /// <summary>
    /// Joins adjacent intervals.
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> instance</param>
    /// <returns>a new <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> with the minimum amount of intervals</returns>
    public static DisjointIntervalSet Consolidate(
        this IDisjointIntervalSet<DateTimeOffset, TimeSpan> set)
    {
        var result = new DisjointStandardIntervalSet();

        if (set.Count > 0)
        {
            var orderedList = set.OrderBy(x => x.Start);

            var cachedItem = orderedList.FirstOrDefault();

            foreach (var item in orderedList.Skip(1))
            {
                if (cachedItem.IsContiguouslyFollowedBy(item))
                {
                    cachedItem = new StandardInterval(
                        cachedItem.Start,
                        item.End,
                        cachedItem.StartIncluded,
                        item.EndIncluded);
                }
                else
                {
                    result.Add(cachedItem);
                    cachedItem = item;
                }
            }

            result.Add(cachedItem);
        }

        return result;
    }

    /// <summary>
    /// Checks if the timestamp is included in the interval set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> instance</param>
    /// <param name="timestamp">the <see cref="DateTimeOffset"/> to check</param>
    /// <returns><code>true</code> if the <see cref="DateTimeOffset"/> is covered by at least one interval in the set, <code>false</code> otherwise</returns>
    public static bool Covers(this IDisjointIntervalSet<DateTimeOffset, TimeSpan> set, DateTimeOffset timestamp) =>
        set.Any(x => x.Covers(timestamp));

    /// <summary>
    /// Checks if the interval is included in the interval set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> instance</param>
    /// <param name="interval">the <see cref="IInterval{DateTimeOffset, TimeSpan}"/> to check</param>
    /// <returns><code>true</code> if the <see cref="IInterval{DateTimeOffset, TimeSpan}"/> is covered by the set, <code>false</code> otherwise</returns>
    public static bool Covers(
        this IDisjointIntervalSet<DateTimeOffset, TimeSpan> set,
        IInterval<DateTimeOffset, TimeSpan> interval)
    {
        return set.Consolidate().Any(x => x.Covers(interval));
    }

    /// <summary>
    /// Intersects the given <see cref="IInterval{DateTimeOffset, TimeSpan}"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> instance</param>
    /// <param name="interval">the <see cref="IInterval{DateTimeOffset, TimeSpan}"/> to intersect</param>
    /// <returns>a new <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> with the intersected set</returns>
    public static DisjointStandardIntervalSet Intersect(
        this IDisjointIntervalSet<DateTimeOffset, TimeSpan> set,
        IInterval<DateTimeOffset, TimeSpan> interval)
    {
        var intersections = set
            .Select(x => x.Intersect(interval))
            .Values()
            .Select(StandardInterval.From);
        return [.. intersections];
    }

    /// <summary>
    /// Joins the given <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> instance</param>
    /// <param name="other">the <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> to join</param>
    /// <returns>the joined <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/></returns>
    public static DisjointIntervalSet Join(
        this IDisjointIntervalSet<DateTimeOffset, TimeSpan> set,
        IDisjointIntervalSet<DateTimeOffset, TimeSpan> other)
    {
        var result = set.Consolidate();

        foreach (var interval in other)
        {
            result = result.Join(interval);
        }
        
        return result;
    }

    /// <summary>
    /// Gets the minimum interval that contains all the intervals of the set.
    /// </summary>
    /// <param name="s">the current <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> instance</param>
    /// <returns>a <see cref="StandardInterval"/> containing all intervals in the set</returns>
    public static StandardInterval GetBoundingInterval(
        this IDisjointIntervalSet<DateTimeOffset, TimeSpan> s)
    {
        return new StandardInterval(s.Start, s.End, s.Covers(s.Start), s.Covers(s.End));
    }

    /// <summary>
    /// Joins the given <see cref="IInterval{DateTimeOffset, TimeSpan}"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> instance</param>
    /// <param name="interval">the <see cref="IInterval{DateTimeOffset, TimeSpan}"/> to join</param>
    /// <returns>a new <see cref="IDisjointIntervalSet{DateTimeOffset, TimeSpan}"/> with the joined intervals</returns>
    public static DisjointStandardIntervalSet Join(
        this IDisjointIntervalSet<DateTimeOffset, TimeSpan> set,
        IInterval<DateTimeOffset, TimeSpan> interval)
    {
        var groups = set.GroupBy(val => val.Intersects(interval)).ToDictionary(g => g.Key, g => g.ToList());

        var nonOverlaps = groups.ContainsKey(false) ? groups[false] : new List<IInterval<DateTimeOffset, TimeSpan>>();
        var result = new DisjointStandardIntervalSet(nonOverlaps.ToArray());

        var overlaps = groups.ContainsKey(true) ? groups[true] : new List<IInterval<DateTimeOffset, TimeSpan>>();
        var newInterval = interval;
        foreach (var overlap in overlaps)
        {
            newInterval = StandardInterval.From(newInterval.Join(overlap));
        }

        result.Add(newInterval);

        return result;
    }
}
