using System;
using System.Collections.Generic;

namespace ephemeral
{
    /// <summary>
    /// Collection of disjoint IIntervals
    /// </summary>
    public interface IDisjointIntervalSet : IList<IInterval>
    {
        bool IsContiguous { get; }

        /// <summary>
        /// Sum of durations of the enclosed intervals 
        /// </summary>
        TimeSpan AggregatedDuration { get; }
        
        /// <summary>
        /// Start of the last contained Interval
        /// </summary>
        DateTimeOffset Start { get; }
        
        /// <summary>
        /// End of the last contained Interval
        /// </summary>
        DateTimeOffset End { get; }
    }
}
