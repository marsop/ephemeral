using System;
using Marsop.Ephemeral.Extensions;
using Optional;

namespace Marsop.Ephemeral
{
    /// <summary>
    /// Immutable Interval Base class
    /// </summary>
    public class Interval : IInterval, IEquatable<IInterval>
    {
        public DateTimeOffset Start { get; }

        public DateTimeOffset End { get; }

        public bool StartIncluded { get; }

        public bool EndIncluded { get; }

        

        /// <summary>
        /// Creates an interval with duration 0
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static Interval CreatePoint(DateTimeOffset timestamp) => CreateClosed(timestamp, timestamp);

        /// <summary>
        /// Creates an interval with neither start or end included
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
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
            Start = start;
            End = end;
            
            StartIncluded = startIncluded;
            EndIncluded = endIncluded;


            if (!IsValid)
                throw new InvalidDurationException(ToString());
        }

        public bool IsValid => (Start < End || (Start == End && StartIncluded && EndIncluded));


        public override string ToString()
        {
            var startDelimiter = StartIncluded ? "[" : "(";
            var endDelimiter = EndIncluded ? "]" : ")";
            return $"{startDelimiter}{Start} => {End}{endDelimiter}";
        }

        public bool Equals(IInterval other) =>
            (Start == other.Start && End == other.End &&
            StartIncluded == other.StartIncluded && EndIncluded == other.EndIncluded);

        public static Option<Interval> Intersect(IInterval first, IInterval second)
        {

            var maxStart = first.Start < second.Start ? second.Start : first.Start;
            var minEnd = first.End < second.End ? first.End : second.End;

            if (minEnd < maxStart)
                return Option.None<Interval>();

            if (minEnd == maxStart && (!first.Covers(minEnd) || !second.Covers(minEnd)))
                return Option.None<Interval>();

            var startIncluded = (first.Covers(maxStart) && second.Covers(minEnd));
            var endIncluded = (first.Covers(minEnd) && second.Covers(minEnd));

            return (new Interval(maxStart, minEnd, startIncluded, endIncluded)).Some();
        }

        public static Interval Join(IInterval first, IInterval second)
        {

            if (second.StartsBefore(first))
                return Join(second, first);

            if (first.Covers(second))
                return first.ToInterval();

            if (first.Intersects(second) || first.IsContiguouslyFollowedBy(second))
                return new Interval(first.Start, second.End, first.StartIncluded, second.EndIncluded);

            throw new ArgumentException("the intervals are not overlapping or contiguous");
        }
    }
}