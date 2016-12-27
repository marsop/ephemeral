using System;

namespace ephemeral
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
            interval.Intersect(other).ToInterval().Equals(other);

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
        public static IInterval Intersect(this IInterval interval, IInterval other) =>
            Interval.Intersect(interval, other);

        public static IDisjointIntervalSet Union(this IInterval interval, IInterval other) =>
            new DisjointIntervalSet(interval, other);

        public static TimeSpan DurationOfIntersect(this IInterval interval, IInterval other)
        {
            var intersection = interval.Intersect(other);
            return intersection?.Duration ?? TimeSpan.Zero;
        }

        public static bool Intersects(this IInterval interval, IInterval other) =>
            interval.Intersect(other) != null;
    }
}