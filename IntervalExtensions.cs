using System;

namespace seasonal
{
    public static class IntervalExtensions
    {
        public static TimeSpan DurationUntilNow(this IInterval interval) =>
            DateTimeOffset.UtcNow < interval.End ? interval.End - DateTimeOffset.UtcNow : interval.Duration;

        public static bool Overlaps(this IInterval interval, IInterval other) =>
            interval.GetOverlap(other) > TimeSpan.Zero;

        public static TimeSpan GetOverlap(this IInterval interval, IInterval other)
        {
            var maxStart = interval.Start < other.Start ? other.Start : interval.Start;
            var minEnd = interval.End < other.End ? interval.End : other.End;

            return maxStart < minEnd ? TimeSpan.Zero : minEnd - maxStart;
        }

        public static IIntervalSet ToIntervalSet(this IInterval interval) {
            var intervalSet = new DisjointIntervalSet();
            intervalSet.A
        }


    }
}