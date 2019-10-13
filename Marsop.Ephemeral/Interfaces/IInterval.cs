// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces
{
    using System;

    /// <summary>
    /// Interface for classes implementing an interval
    /// </summary>
    public interface IInterval
    {
        /// <summary>
        /// Gets the starting point of the interval
        /// </summary>
        DateTimeOffset Start { get; }

        /// <summary>
        /// Gets a value indicating whether the start timestamp is included in the interval
        /// </summary>
        bool StartIncluded { get; }

        /// <summary>
        /// Gets the final point of the interval
        /// </summary>
        DateTimeOffset End { get; }

        /// <summary>
        /// Gets a value indicating whether the end timestamp is included in the interval
        /// </summary>
        bool EndIncluded { get; }

        /// <summary>
        /// Gets the difference between start and end as <see cref="TimeSpan"/>
        /// </summary>
        public TimeSpan Duration => this.End - this.Start;
    }
}