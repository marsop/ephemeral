using Optional;
using System;

namespace Marsop.Ephemeral.Extensions
{
    public static class IntervalExtensions
    {
        public static bool Covers(this IInterval interval, DateTimeOffset timestamp)
        {
            if (timestamp < interval.Start)
                return false;

            if (interval.End < timestamp)
                return false;

            if (timestamp == interval.Start && !interval.StartIncluded)
                return false;

            if (timestamp == interval.End && !interval.EndIncluded)
                return false;

            return true;
        }

        public static bool Covers(this IInterval interval, IInterval other) =>
            interval.Intersect(other).Match(x => x.ToInterval().Equals(other), () => false);

        public static TimeSpan DurationUntilNow(this IInterval interval) =>
            DateTimeOffset.UtcNow < interval.End ? interval.End - DateTimeOffset.UtcNow : interval.Duration;


        /// <summary>
        /// Creates an interval based on the information of this object
        /// </summary>
        /// <returns> new interval </returns>
        public static Interval ToInterval(this IInterval interval) =>
            new Interval(interval.Start, interval.End, interval.StartIncluded, interval.EndIncluded);

        /// <summary>
        /// Generates a new Interval, which is the intersection of the two.
        /// WARN: Can be null
        /// </summary>
        /// <param name="interval"> first interval </param>
        /// <param name="other"> second interval </param>
        /// <returns> new interval </returns>
        public static Option<IInterval> Intersect(this IInterval interval, IInterval other) =>
            Interval.Intersect(interval, other).Map(x => (IInterval)x);

        public static IDisjointIntervalSet Union(this IInterval i, IInterval j) => new DisjointIntervalSet(i, j);

        public static TimeSpan DurationOfIntersect(this IInterval i, IInterval j) =>
            i.Intersect(j).Match(x => x.Duration, () => TimeSpan.Zero);


        public static bool Intersects(this IInterval i, IInterval j) => i.Intersect(j).HasValue;

        /// <summary>
        /// Returns true if both intervals join with each other seamlessly and without overlap
        /// </summary>
        /// <param name="i">earlier interval</param>
        /// <param name="o">later interval</param>
        /// <returns></returns>
        public static bool IsContiguouslyFollowedBy(this IInterval i, IInterval o) =>
            i.End == o.Start && (i.EndIncluded != o.StartIncluded);

        public static bool IsContiguouslyPreceededBy(this IInterval i, IInterval o) =>
            o.IsContiguouslyFollowedBy(i);

        public static bool StartsBefore(this IInterval interval, IInterval other) =>
            (interval.Start < other.Start || (interval.Start == other.Start && interval.StartIncluded && !other.StartIncluded));

    }
}