using System;

namespace Marsop.Ephemeral
{
    /// <summary>
    /// Interface for classes implementing an interval
    /// </summary>
    public interface IInterval
    {
        /// <summary>
        /// starting point of the interval
        /// </summary>
        DateTimeOffset Start { get; }

        /// <summary>
        /// indicates if the start timestamp is included in the interval
        /// </summary>
        bool StartIncluded { get; }

        /// <summary>
        /// final point of the interval
        /// </summary>
        DateTimeOffset End { get; }

        /// <summary>
        /// indicates if the end timestamp is included in the interval
        /// </summary>
        bool EndIncluded { get; }

        /// <summary>
        /// gets the difference between start and end as timestamp
        /// </summary>
        public TimeSpan Duration => End - Start;

    }
}