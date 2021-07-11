// <copyright file="Interval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using System.Collections.Generic;
using Marsop.Ephemeral.Exceptions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;
using Optional;

namespace Marsop.Ephemeral.Implementation
{
    /// <summary>
    /// Immutable Interval Base class
    /// </summary>
    public class Interval : IInterval, IEquatable<IInterval>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Interval" /> class
        /// </summary>
        /// <param name="start">starting <see cref="DateTimeOffset"/></param>
        /// <param name="duration">the interval duration</param>
        /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
        /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
        public Interval(DateTimeOffset start, TimeSpan duration, bool startIncluded, bool endIncluded) : this(start, start.Add(duration), startIncluded, endIncluded)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval" /> class
        /// </summary>
        /// <param name="start">a <see cref="ITimestamped"/> instance representing the starting point</param>
        /// <param name="end">a <see cref="ITimestamped"/> instance representing the ending point</param>
        /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
        /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
        public Interval(ITimestamped start, ITimestamped end, bool startIncluded, bool endIncluded) : this(start.Timestamp, end.Timestamp, startIncluded, endIncluded)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval" /> class
        /// </summary>
        /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
        /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
        /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
        /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
        public Interval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded)
        {
            this.Start = start;
            this.End = end;

            this.StartIncluded = startIncluded;
            this.EndIncluded = endIncluded;

            if (!this.IsValid)
            {
                throw new InvalidDurationException(this.GetTextualRepresentation());
            }
        }

        /// <inheritdoc cref="IInterval.End"/>
        public DateTimeOffset End { get; }

        /// <inheritdoc cref="IInterval.EndIncluded"/>
        public bool EndIncluded { get; }

        /// <summary>
        /// Checks if the current <see cref="Interval"/> has coherent starting and ending points
        /// </summary>
        /// <returns><code>true</code> if starting and ending points are valid, <code>false</code> otherwise</returns>
        public bool IsValid => this.Start < this.End || (this.Start == this.End && this.StartIncluded && this.EndIncluded);

        /// <inheritdoc cref="IInterval.Start"/>
        public DateTimeOffset Start { get; }

        /// <inheritdoc cref="IInterval.StartIncluded"/>
        public bool StartIncluded { get; }

        /// <summary>
        /// Creates an interval with both start and end included
        /// </summary>
        /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
        /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
        /// <returns>an <see cref="Interval"/> with both start and end included</returns>
        public static Interval CreateClosed(DateTimeOffset start, DateTimeOffset end) => new Interval(start, end, true, true);

        /// <summary>
        /// Creates an interval with neither start or end included
        /// </summary>
        /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
        /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
        /// <returns>an <see cref="Interval"/> with neither start or end included</returns>
        public static Interval CreateOpen(DateTimeOffset start, DateTimeOffset end) => new Interval(start, end, false, false);

        /// <summary>
        /// Creates an interval with duration 0
        /// </summary>
        /// <param name="timestamp">the <see cref="DateTimeOffset"/></param>
        /// <returns>an <see cref="Interval"/> with start and end point set with the given <see cref="DateTimeOffset"/></returns>
        public static Interval CreatePoint(DateTimeOffset timestamp) => CreateClosed(timestamp, timestamp);

        /// <summary>
        /// Intersect two intervals
        /// </summary>
        /// <param name="first">the first <see cref="IInterval"/> instance</param>
        /// <param name="second">the second <see cref="IInterval"/> instance</param>
        /// <returns>a new <see cref="Interval"/> if an intersection exists</returns>
        /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
        public static Option<Interval> Intersect(IInterval first, IInterval second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            var maxStart = first.Start < second.Start ? second.Start : first.Start;
            var minEnd = first.End < second.End ? first.End : second.End;

            if (minEnd < maxStart)
            {
                return Option.None<Interval>();
            }

            if (minEnd == maxStart && (!first.Covers(minEnd) || !second.Covers(minEnd)))
            {
                return Option.None<Interval>();
            }

            var startIncluded = first.Covers(maxStart) && second.Covers(maxStart);
            var endIncluded = first.Covers(minEnd) && second.Covers(minEnd);

            return new Interval(maxStart, minEnd, startIncluded, endIncluded).Some();
        }

        /// <summary>
        /// Join two intervals
        /// </summary>
        /// <param name="first">the first <see cref="IInterval"/> instance</param>
        /// <param name="second">the second <see cref="IInterval"/> instance</param>
        /// <returns>a new <see cref="Interval"/> with joined intervals</returns>
        /// <exception cref="ArgumentException">an exception is thrown if the two intervals are not contiguous or overlapping</exception>
        /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
        public static Interval Join(IInterval first, IInterval second)
        {
            if (first is null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (second.StartsBefore(first))
            {
                return Join(second, first);
            }

            if (first.Covers(second))
            {
                return first.ToInterval();
            }

            if (first.Intersects(second) || first.IsContiguouslyFollowedBy(second))
            {
                return new Interval(first.Start, second.End, first.StartIncluded, second.EndIncluded);
            }

            throw new ArgumentException("the intervals are not overlapping or contiguous");
        }

        /// <summary>
        /// Join two intervals
        /// </summary>
        /// <param name="source">the source <see cref="IInterval"/> instance</param>
        /// <param name="subtraction">the subtraction <see cref="IInterval"/> instance</param>
        /// <returns>a list of <see cref="Interval"/> after subtraction</returns>
        /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
        public static List<IInterval> Subtract(IInterval source, IInterval subtraction)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (subtraction is null)
            {
                throw new ArgumentNullException(nameof(subtraction));
            }

            if (!source.Intersects(subtraction))
            {
                return new List<IInterval>
                {
                    source.ToInterval()
                };
            }

            if (subtraction.Covers(source))
            {
                return new List<IInterval>
                {
                };
            }

            var result = new List<IInterval>();

            if (source.Start < subtraction.Start ||
                (source.Start == subtraction.Start && source.StartIncluded && !subtraction.StartIncluded))
                result.Add(new Interval(source.Start, subtraction.Start, source.StartIncluded, !subtraction.StartIncluded));
            if (source.End > subtraction.End ||
                (source.End == subtraction.End && source.EndIncluded && !subtraction.EndIncluded))
                result.Add(new Interval(subtraction.End, source.End, !subtraction.EndIncluded, source.EndIncluded));

            return result;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(IInterval other) => other != null && (this.Start == other.Start && this.End == other.End && this.StartIncluded == other.StartIncluded && this.EndIncluded == other.EndIncluded);

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return this.GetTextualRepresentation();
        }

        /// <summary>
        /// Get a representation of the interval.
        /// Open intervals are represented with parenthesis (a,b)
        /// Close intervals are represented with brakets [a,b]
        /// </summary>
        /// <returns>a <see cref="String"/> that represent the interval</returns>
        private string GetTextualRepresentation()
        {
            var startDelimiter = this.StartIncluded ? "[" : "(";
            var endDelimiter = this.EndIncluded ? "]" : ")";
            return $"{startDelimiter}{this.Start} => {this.End}{endDelimiter}";
        }
    }
}