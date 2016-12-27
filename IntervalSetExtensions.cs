using System;
using System.Linq;

namespace ephemeral
{
    public static class IntervalSetExtensions
    {
        /// <summary>
        /// Minimum interval that contais all the intervals of the set.
        /// </summary>
        /// <returns></returns>
        public static IInterval GetBoundingInterval(this IDisjointIntervalSet set) {
            var startIncluded = set.Covers(set.Start);
            var endIncluded = set.Covers(set.End);
            return new Interval(set.Start, set.End, startIncluded, endIncluded);
        }

        /// <summary>
        /// Checks if the timestamp is included in the interval set
        /// </summary>
        public static bool Covers(this IDisjointIntervalSet set, DateTimeOffset timestamp) =>
            set.Any(x => x.Covers(timestamp));


        public static IDisjointIntervalSet Intersect(this IDisjointIntervalSet set, IInterval interval) =>
             null;


        /// <summary>
         /// Joins adjacent intervals.
         /// </summary>
         /// <returns> new set with the minimum amount of intervals</returns>
         public static IDisjointIntervalSet Consolidate(this IDisjointIntervalSet set) {

            // TODO
            return set;
         }
    }
}