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

        bool StartIncluded { get; }

        /// <summary>
        /// final point of the interval
        /// </summary>
        DateTimeOffset End { get; }

        bool EndIncluded { get; }

        public TimeSpan Duration => End - Start;

        
    }
}