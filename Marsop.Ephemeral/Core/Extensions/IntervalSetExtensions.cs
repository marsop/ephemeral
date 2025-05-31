// <copyright file="IntervalSetExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Optional.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marsop.Ephemeral.Core;


/// <summary>
/// Extensions method for <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instances
/// </summary>
public static class IntervalSetExtensions
{
    /// <summary>
    /// Joins adjacent intervals.
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instance</param>
    /// <returns>a new <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> with the minimum amount of intervals</returns>
    public static DisjointIntervalSet<TBoundary, TLength> Consolidate<TBoundary, TLength>(
        this IDisjointIntervalSet<TBoundary, TLength> set)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        var result = new DisjointIntervalSet<TBoundary, TLength>(set.LengthOperator);

        if (set.Count > 0)
        {
            var orderedList = set.OrderBy(x => x.Start);

            var cachedItem = orderedList.FirstOrDefault();

            foreach (var item in orderedList.Skip(1))
            {
                if (cachedItem.IsContiguouslyFollowedBy(item))
                {
                    cachedItem = new BasicInterval<TBoundary>(
                        cachedItem.Start,
                        item.End,
                        cachedItem.StartIncluded,
                        item.EndIncluded)
                        .WithMeaure(set.LengthOperator);
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
    /// <param name="set">the current <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instance</param>
    /// <param name="timestamp">the <see cref="TBoundary"/> to check</param>
    /// <returns><code>true</code> if the <see cref="TBoundary"/> is covered by at least one interval in the set, <code>false</code> otherwise</returns>
    public static bool Covers<TBoundary, TLength>(
        this IDisjointIntervalSet<TBoundary, TLength> set,
        TBoundary timestamp)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return set.Any(x => x.Covers(timestamp));
    }

    /// <summary>
    /// Checks if the interval is included in the interval set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instance</param>
    /// <param name="interval">the <see cref="IInterval{TBoundary, TLength}"/> to check</param>
    /// <returns><code>true</code> if the <see cref="IInterval{TBoundary, TLength}"/> is covered by the set, <code>false</code> otherwise</returns>
    public static bool Covers<TBoundary, TLength>(
        this IDisjointIntervalSet<TBoundary, TLength> set,
        IBasicInterval<TBoundary> interval)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return set.Consolidate().Any(x => x.Covers(interval));
    }

    /// <summary>
    /// Intersects the given <see cref="IInterval{TBoundary, TLength}"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instance</param>
    /// <param name="interval">the <see cref="IInterval{TBoundary, TLength}"/> to intersect</param>
    /// <returns>a new <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> with the intersected set</returns>
    public static DisjointIntervalSet<TBoundary, TLength> Intersect<TBoundary, TLength>(
        this IDisjointIntervalSet<TBoundary, TLength> set,
        IBasicInterval<TBoundary> interval)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        var intersections = set
            .Select(x => x.Intersect(interval))
            .Values()
            .Select(x => x.WithMeaure(set.LengthOperator))
            .ToList();
        return new DisjointIntervalSet<TBoundary, TLength>(set.LengthOperator, intersections);
    }

    /// <summary>
    /// Joins the given <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instance</param>
    /// <param name="other">the <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> to join</param>
    /// <returns>the joined <see cref="IDisjointIntervalSet{TBoundary, TLength}"/></returns>
    public static DisjointIntervalSet<TBoundary, TLength> Join<TBoundary, TLength>(
        this IDisjointIntervalSet<TBoundary, TLength> set,
        IDisjointIntervalSet<TBoundary, TLength> other)
        where TBoundary : notnull, IComparable<TBoundary>
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
    /// <param name="s">the current <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instance</param>
    /// <returns>a <see cref="StandardInterval"/> containing all intervals in the set</returns>
    public static BasicMeasuredInterval<TBoundary, TLength> GetBoundingInterval<TBoundary, TLength>(
        this IDisjointIntervalSet<TBoundary, TLength> s)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return new BasicInterval<TBoundary>(s.Start, s.End, s.Covers(s.Start), s.Covers(s.End))
            .WithMeaure(s.LengthOperator);
    }

    /// <summary>
    /// Joins the given <see cref="IInterval{TBoundary, TLength}"/> with the current set
    /// </summary>
    /// <param name="set">the current <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> instance</param>
    /// <param name="interval">the <see cref="IInterval{TBoundary, TLength}"/> to join</param>
    /// <returns>a new <see cref="IDisjointIntervalSet{TBoundary, TLength}"/> with the joined intervals</returns>
    public static DisjointIntervalSet<TBoundary, TLength> Join<TBoundary, TLength>(
        this IDisjointIntervalSet<TBoundary, TLength> set,
        IBasicInterval<TBoundary> interval)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        var groups = set.GroupBy(val => val.Intersects(interval)).ToDictionary(g => g.Key, g => g.ToList());

        var nonOverlaps = groups.ContainsKey(false) ? groups[false] : new List<IBasicInterval<TBoundary>>();
        var result = new DisjointIntervalSet<TBoundary, TLength>(set.LengthOperator, nonOverlaps.ToArray());

        var overlaps = groups.ContainsKey(true) ? groups[true] : new List<IBasicInterval<TBoundary>>();
        var newInterval = interval;
        foreach (var overlap in overlaps)
        {
            newInterval = newInterval.Join(overlap).WithMeaure(set.LengthOperator);
        }

        result.Add(newInterval);

        return result;
    }
}
