using System;

namespace seasonal
{
    public static class IntervalExtensions
    {
        public static TimeSpan DurationUntilNow(this IInterval interval) =>
            DateTimeOffset.UtcNow < interval.End ? interval.End - DateTimeOffset.UtcNow : interval.Duration;

        public static bool Overlaps(this IInterval interval, IInterval other) =>
            interval.Intersect(other) != null;

        public static bool Contains(this IInterval interval, DateTimeOffset timestamp)
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

        public static IInterval Intersect(this IInterval interval, IInterval other)
        {
            var maxStart = interval.Start < other.Start ? other.Start : interval.Start;
            var minEnd = interval.End < other.End ? interval.End : other.End;

            if (minEnd < maxStart)
                return null;

            if (minEnd == maxStart && (!interval.Contains(minEnd) || !other.Contains(minEnd)))
                return null;

            var startIncluded = (interval.Contains(maxStart) && other.Contains(minEnd));
            var endIncluded = (interval.Contains(minEnd) && other.Contains(minEnd));

            return new Interval(maxStart, minEnd, startIncluded, endIncluded);
        }

        public static TimeSpan GetOverlapDuration(this IInterval interval, IInterval other)
        {
            var intersection = interval.Intersect(other);
            return intersection?.Duration ?? TimeSpan.Zero;
        }
    }
}