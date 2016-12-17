namespace seasonal
{
    public static class IntervalExtensions
    {
        public static bool Overlaps (this IInterval interval, IInterval other) {
            var maxStart = interval.Start < other.Start ? other.Start : interval.Start;
            var minEnd = interval.End < other.End ? interval.End : other.End;

            return maxStart < minEnd;
        }

        
    }
}