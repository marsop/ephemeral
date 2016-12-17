using System;

namespace seasonal
{
    public class SimpleInterval : IInterval
    {
        public TimeSpan Duration => End - Start;

        public DateTimeOffset End { get; private set; }

        public DateTimeOffset Start { get; private set; }

        public SimpleInterval(DateTimeOffset start, DateTimeOffset end)
        {
            if (!IsValid(start, end))
                throw new InvalidDurationException("end before start");

            Start = start;
            End = end;
        }

        public SimpleInterval(DateTimeOffset start, TimeSpan duration) : this(start, start.Add(duration)) { }

        public SimpleInterval(ITimestamped start, ITimestamped end) : this(start.Timestamp, end.Timestamp) { }

        private bool IsValid(DateTimeOffset start, DateTimeOffset end) => start < end;

    }
}