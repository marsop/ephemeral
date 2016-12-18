using System;

namespace seasonal
{
    public class ClosedInterval : Interval
    {
        public ClosedInterval(DateTimeOffset start, DateTimeOffset end) : base(start, end, true, true) { }

        public ClosedInterval(DateTimeOffset start, TimeSpan duration) : this(start, start.Add(duration)) { }

        public ClosedInterval(ITimestamped start, ITimestamped end) : this(start.Timestamp, end.Timestamp) { }
    }
}