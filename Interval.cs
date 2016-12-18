using System;

namespace seasonal
{
    /// <summary>
    /// Immutable Interval Base class
    /// </summary>
    public class Interval : IInterval
    {
        private readonly DateTimeOffset _start;
        public DateTimeOffset Start => _start;

        private readonly DateTimeOffset _end;
        public DateTimeOffset End => _end;

        private readonly bool _startIncluded;
        public bool StartIncluded => _startIncluded;
        
        private readonly bool _endIncluded;
        public bool EndIncluded => _endIncluded;
        
        private TimeSpan _duration;
        public TimeSpan Duration => _duration;
        

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

        public Interval(DateTimeOffset start, TimeSpan duration, bool startIncluded, bool endIncluded) :
        this(start, start.Add(duration), startIncluded, endIncluded)
        { }

        public Interval(ITimestamped start, ITimestamped end, bool startIncluded, bool endIncluded) :
        this(start.Timestamp, end.Timestamp, startIncluded, endIncluded)
        { }

        private bool IsValid() => (Start < End || (Start == End && StartIncluded && EndIncluded));

        public override string ToString() { 
            var startDelimiter = StartIncluded ? "[" : "(";
            var endDelimiter = EndIncluded ? "]" : ")";
            return $"{startDelimiter}{Start} => {End}{endDelimiter}";
        }
    }
}