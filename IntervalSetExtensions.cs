using System.Linq;

namespace seasonal
{
    public static class IntervalSetExtensions
    {
        /// <summary>
        /// Minimum interval that comprises all the intervals of the set.
        /// </summary>
        /// <returns></returns>
        public static IInterval GetBoundingInterval(this IIntervalSet set) =>
            new SimpleInterval(set.Start, set.End);


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