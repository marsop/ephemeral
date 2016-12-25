using System;
using System.Collections.Generic;

namespace seasonal
{
    /// <summary>
    /// Collection of IIntervals
    /// </summary>
    public interface IIntervalSet : IList<IInterval>
    {
        bool HasOverlap { get; }
        
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
