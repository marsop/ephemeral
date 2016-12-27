using System;

namespace ephemeral
{
    /// <summary>
    /// Immutable Interval Base class
    /// </summary>
    public class Interval : IInterval, IEquatable<IInterval>
    {
        private readonly DateTimeOffset _start;
        public DateTimeOffset Start => _start;

        private readonly DateTimeOffset _end;
        public DateTimeOffset End => _end;

        private readonly bool _startIncluded;
        public bool StartIncluded => _startIncluded;
        
        private readonly bool _endIncluded;
        public bool EndIncluded => _endIncluded;
        
        private readonly TimeSpan _duration;
        public TimeSpan Duration => _duration;
        
        public static Interval CreatePoint(DateTimeOffset timestamp) =>
            Interval.CreateClosed(timestamp, timestamp);

        public static Interval CreateOpen(DateTimeOffset start, DateTimeOffset end) =>
            new Interval(start, end, false, false);

        public static Interval CreateClosed(DateTimeOffset start, DateTimeOffset end) =>
            new Interval(start, end, true, true);

        public Interval(DateTimeOffset start, TimeSpan duration, bool startIncluded, bool endIncluded) :
        this(start, start.Add(duration), startIncluded, endIncluded)
        { }

        public Interval(ITimestamped start, ITimestamped end, bool startIncluded, bool endIncluded) :
        this(start.Timestamp, end.Timestamp, startIncluded, endIncluded)
        { }

        public Interval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded)
        {
            _start = start;
            _end = end;
            _duration = end - start;
            _startIncluded = startIncluded;
            _endIncluded = endIncluded;

            if (!IsValid())
                throw new InvalidDurationException(ToString());
        }

        private bool IsValid() => (Start < End || (Start == End && StartIncluded && EndIncluded));

        public override string ToString() { 
            var startDelimiter = StartIncluded ? "[" : "(";
            var endDelimiter = EndIncluded ? "]" : ")";
            return $"{startDelimiter}{Start} => {End}{endDelimiter}";
        }

        public bool Equals(IInterval other) =>
            (Start == other.Start && End == other.End && 
            StartIncluded == other.StartIncluded && EndIncluded == other.EndIncluded);

        public static Interval Intersect(IInterval first, IInterval second) {

            var maxStart = first.Start < second.Start ? second.Start : first.Start;
            var minEnd = first.End < second.End ? first.End : second.End;

            if (minEnd < maxStart)
                return null;

            if (minEnd == maxStart && (!first.Covers(minEnd) || !second.Covers(minEnd)))
                return null;

            var startIncluded = (first.Covers(maxStart) && second.Covers(minEnd));
            var endIncluded = (first.Covers(minEnd) && second.Covers(minEnd));

            return new Interval(maxStart, minEnd, startIncluded, endIncluded);
        }     
    }
}