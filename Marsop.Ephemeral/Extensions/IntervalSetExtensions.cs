// <copyright file="IntervalSetExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Optional.Unsafe;

    /// <summary>
    /// Extensions method for <see cref="IDisjointIntervalSet"/> instances
    /// </summary>
    public static class IntervalSetExtensions
    {
        /// <summary>
        /// Checks if the timestamp is included in the interval set
        /// </summary>
        /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
        /// <param name="timestamp">the <see cref="DateTimeOffset"/> to check</param>
        /// <returns><code>true</code> if the <see cref="DateTimeOffset"/> is covered by at least one interval in the set, <code>false</code> otherwise</returns>
        public static bool Covers(this IDisjointIntervalSet set, DateTimeOffset timestamp) =>
            set.Any(x => x.Covers(timestamp));

        /// <summary>
        /// Joins the given <see cref="IDisjointIntervalSet"/> with the current set
        /// </summary>
        /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
        /// <param name="other">the <see cref="IDisjointIntervalSet"/> to join</param>
        /// <returns>the joined <see cref="IDisjointIntervalSet"/></returns>
        public static IDisjointIntervalSet Join(this IDisjointIntervalSet set, IDisjointIntervalSet other)
        {
            var result = set.Consolidate();
            other.ToList().ForEach(x => result.Join(x));
            return result;
        }

        /// <summary>
        /// Joins the given <see cref="IInterval"/> with the current set
        /// </summary>
        /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
        /// <param name="interval">the <see cref="IInterval"/> to join</param>
        /// <returns>a new <see cref="IDisjointIntervalSet"/> with the joined intervals</returns>
        public static IDisjointIntervalSet Join(this IDisjointIntervalSet set, IInterval interval)
        {
            var groups = set.GroupBy(val => val.Intersects(interval)).ToDictionary(g => g.Key, g => g.ToList());

            var nonOverlaps = groups.ContainsKey(false) ? groups[false] : new List<IInterval>();
            var result = new DisjointIntervalSet(nonOverlaps);

            var overlaps = groups.ContainsKey(true) ? groups[true] : new List<IInterval>();
            var newInterval = interval;
            foreach (var overlap in overlaps)
            {
                newInterval = Interval.Join(newInterval, overlap);
            }

            result.Add(newInterval);

            return result;
        }

        /// <summary>
        /// Intersects the given <see cref="IInterval"/> with the current set
        /// </summary>
        /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
        /// <param name="interval">the <see cref="IInterval"/> to intersect</param>
        /// <returns>a new <see cref="IDisjointIntervalSet"/> with the intersected set</returns>
        public static IDisjointIntervalSet Intersect(this IDisjointIntervalSet set, IInterval interval)
        {
            var intersections = set
                .Select(x => x.Intersect(interval))
                .Where(y => y.HasValue)
                .Select(z => z.ValueOrDefault());
            return new DisjointIntervalSet(intersections);
        }

        /// <summary>
        /// Joins adjacent intervals.
        /// </summary>
        /// <param name="set">the current <see cref="IDisjointIntervalSet"/> instance</param>
        /// <returns>a new <see cref="IDisjointIntervalSet"/> with the minimum amount of intervals</returns>
        public static IDisjointIntervalSet Consolidate(this IDisjointIntervalSet set)
        {
            var result = new DisjointIntervalSet();

            var orderedList = set.OrderBy(x => x.Start);

            var cachedItem = orderedList.FirstOrDefault();

            foreach (var item in orderedList.Skip(1))
            {
                if (cachedItem.IsContiguouslyFollowedBy(item))
                {
                    cachedItem = new Interval(cachedItem.Start, item.End, cachedItem.StartIncluded, item.EndIncluded);
                }
                else
                {
                    result.Add(cachedItem);
                    cachedItem = item;
                }
            }

            result.Add(cachedItem);

            return result;
        }
    }
}