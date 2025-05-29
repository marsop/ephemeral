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
/// Extensions method for <see cref="IDisjointIntervalSet"/> instances
/// </summary>
public static class IntervalSetExtensions
{
    /// <summary>
    /// Joins adjacent intervals.
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
    /// <returns>a new <see cref="IDisjointIntervalSet"/> with the minimum amount of intervals</returns>
    public static DisjointIntervalSet Consolidate(this IDisjointIntervalSet set)
    {
        var result = new DisjointIntervalSet();

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
    /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
    /// <param name="timestamp">the <see cref="DateTimeOffset"/> to check</param>
    /// <returns><code>true</code> if the <see cref="DateTimeOffset"/> is covered by at least one interval in the set, <code>false</code> otherwise</returns>
    public static bool Covers(this IDisjointIntervalSet set, DateTimeOffset timestamp) =>
        set.Any(x => x.Covers(timestamp));

            /// <summary>
    /// Checks if the timestamp is included in the interval set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
    /// <param name="interval">the <see cref="IDateTimeOffsetInterval"/> to check</param>
    /// <returns><code>true</code> if the <see cref="IDateTimeOffsetInterval"/> is covered by the set, <code>false</code> otherwise</returns>
    public static bool Covers(this IDisjointIntervalSet set, IDateTimeOffsetInterval interval) =>
        set.Consolidate().Any(x => x.Covers(interval));

    /// <summary>
    /// Intersects the given <see cref="IDateTimeOffsetInterval"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
    /// <param name="interval">the <see cref="IDateTimeOffsetInterval"/> to intersect</param>
    /// <returns>a new <see cref="IDisjointIntervalSet"/> with the intersected set</returns>
    public static DisjointIntervalSet Intersect(this IDisjointIntervalSet set, IDateTimeOffsetInterval interval)
    {
        var intersections = set
            .Select(x => x.Intersect(interval))
            .Values()
            .Select(StandardInterval.From);
        return [.. intersections];
    }

    /// <summary>
    /// Joins the given <see cref="IDisjointIntervalSet"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
    /// <param name="other">the <see cref="IDisjointIntervalSet"/> to join</param>
    /// <returns>the joined <see cref="IDisjointIntervalSet"/></returns>
    public static DisjointIntervalSet Join(this IDisjointIntervalSet set, IDisjointIntervalSet other)
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
    /// <returns></returns>
    public static StandardInterval GetBoundingInterval(this IDisjointIntervalSet s)
    {
        return new StandardInterval(s.Start, s.End, s.Covers(s.Start), s.Covers(s.End));
    }

    /// <summary>
    /// Joins the given <see cref="IDateTimeOffsetInterval"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
    /// <param name="interval">the <see cref="IDateTimeOffsetInterval"/> to join</param>
    /// <returns>a new <see cref="IDisjointIntervalSet"/> with the joined intervals</returns>
    public static DisjointIntervalSet Join(this IDisjointIntervalSet set, IDateTimeOffsetInterval interval)
    {
        var groups = set.GroupBy(val => val.Intersects(interval)).ToDictionary(g => g.Key, g => g.ToList());

        var nonOverlaps = groups.ContainsKey(false) ? groups[false] : new List<IDateTimeOffsetInterval>();
        var result = new DisjointIntervalSet(nonOverlaps);

        var overlaps = groups.ContainsKey(true) ? groups[true] : new List<IDateTimeOffsetInterval>();
        var newInterval = interval;
        foreach (var overlap in overlaps)
        {
            newInterval = StandardInterval.From(newInterval.Join(overlap));
        }

        result.Add(newInterval);

        return result;
    }
}
