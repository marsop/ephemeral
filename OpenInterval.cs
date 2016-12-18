using System;

namespace seasonal
{
    public class OpenInterval : Interval
    {

        public OpenInterval(DateTimeOffset start, DateTimeOffset end) : base(start, end, false, false) { }

        public OpenInterval(DateTimeOffset start, TimeSpan duration) : this(start, start.Add(duration)) { }

        public OpenInterval(ITimestamped start, ITimestamped end) : this(start.Timestamp, end.Timestamp) { }
    }
}