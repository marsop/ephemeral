using System;

namespace ephemeral
{
    /// <summary>
    /// Interface for classes implementing an interval
    /// </summary>
    public interface IInterval
    {
        /// <summary>
        /// first point in time where the interval is defined
        /// </summary>
        /// <returns></returns>
        DateTimeOffset Start { get; }
        bool StartIncluded { get; }

        /// <summary>
        /// final point in time where the interval is defined
        /// </summary>
        DateTimeOffset End { get; }
        bool EndIncluded { get; }
        TimeSpan Duration { get; }
    }
}