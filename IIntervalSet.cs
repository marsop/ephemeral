using System;
using System.Collections.Generic;

namespace seasonal
{
    /// <summary>
    /// An IntervalSet cannot have overlapping intervals inside.
    /// </summary>
    public interface IIntervalSet : IList<IInterval>
    {
        bool HasOverlap { get; }
        TimeSpan AggregatedDuration { get; }
        DateTimeOffset Start { get; }
        DateTimeOffset End { get; }
    }
}