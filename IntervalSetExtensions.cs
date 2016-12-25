using System;
using System.Linq;

namespace seasonal
{
    public static class IntervalSetExtensions
    {
        /// <summary>
        /// Minimum interval that contais all the intervals of the set.
        /// </summary>
        /// <returns></returns>
        public static IInterval GetBoundingInterval(this IIntervalSet set) {
            var startIncluded = set.Covers(set.Start);
            var endIncluded = set.Covers(set.End);
            return new Interval(set.Start, set.End, startIncluded, endIncluded);
        }

        public static bool Covers(this IIntervalSet set, DateTimeOffset timestamp) =>
            set.Covers(Interval.CreatePoint(timestamp));

        public static bool Covers(this IIntervalSet set, IInterval interval) =>
            set.Intersect(interval)


        public static IIntervalSet Intersect(this IIntervalSet set, IInterval interval) =>
             null;


        /// <summary>
         /// Joins adjacent intervals.
         /// </summary>
         /// <returns> new set with the minimum amount of intervals</returns>
         public static IIntervalSet Consolidate(this IIntervalSet set) {
             if (set.HasOverlap)
                throw new OverlapException(nameof(set));

            // TODO
            return set;
         }
    }
}