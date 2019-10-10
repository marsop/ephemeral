// <copyright file="IDisjointIntervalSet.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral
{
    using System;
    using System.Collections.Generic;
    using Marsop.Ephemeral.Extensions;

    /// <summary>
    /// Collection of disjoint IIntervals
    /// </summary>
    public interface IDisjointIntervalSet : IList<IInterval>
    {
        /// <summary>
        /// Gets a value indicating whether all the contained intervals are contiguous or the set is empty
        /// </summary>
        bool IsContiguous { get; }

        /// <summary>
        /// Gets the sum of durations of each of the enclosed intervals 
        /// </summary>
        TimeSpan AggregatedDuration { get; }

        /// <summary>
        /// Gets the Start of the earliest contained Interval
        /// </summary>
        DateTimeOffset Start { get; }

        /// <summary>
        /// Gets the end of the latest contained Interval
        /// </summary>
        DateTimeOffset End { get; }

        /// <summary>
        /// Gets a value indicating whether the Start is included
        /// </summary>
        bool StartIncluded { get; }

        /// <summary>
        /// Gets a value indicating whether the End is included
        /// </summary>
        bool EndIncluded { get; }

        /// <summary>
        /// Gets the minimum interval that contains all the intervals of the set.
        /// </summary>
        /// <returns></returns>
        IInterval GetBoundingInterval() => new Interval(Start, End, this.Covers(Start), this.Covers(End));
    }
}
