using System;
using System.Collections.Generic;
using Marsop.Ephemeral.Extensions;

namespace Marsop.Ephemeral
{
    /// <summary>
    /// Collection of disjoint IIntervals
    /// </summary>
    public interface IDisjointIntervalSet : IList<IInterval>
    {
        /// <summary>
        /// True if all the contained intervals are contiguous or if empty
        /// </summary>
        bool IsContiguous { get; }

        /// <summary>
        /// Sum of durations of each of the enclosed intervals 
        /// </summary>
        TimeSpan AggregatedDuration { get; }

        /// <summary>
        /// Start of the earliest contained Interval
        /// </summary>
        DateTimeOffset Start { get; }

        /// <summary>
        /// End of the latest contained Interval
        /// </summary>
        DateTimeOffset End { get; }

        bool StartIncluded { get; }

        bool EndIncluded { get; }

        /// <summary>
        /// Minimum interval that contais all the intervals of the set.
        /// </summary>
        /// <returns></returns>
        IInterval GetBoundingInterval() => new Interval(Start, End, this.Covers(Start), this.Covers(End));
    }
}
