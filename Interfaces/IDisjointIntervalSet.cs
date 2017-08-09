using System;
using System.Collections.Generic;

namespace ephemeral
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
        /// Start of the first contained Interval
        /// </summary>
        DateTimeOffset Start { get; }
        
        /// <summary>
        /// End of the last contained Interval
        /// </summary>
        DateTimeOffset End { get; }
        
        bool StartIncluded { get; }
        
        bool EndIncluded { get; }
    }
}
