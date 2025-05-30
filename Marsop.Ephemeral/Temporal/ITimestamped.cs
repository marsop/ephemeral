// <copyright file="ITimestamped.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Temporal;

using System;

/// <summary>
/// Simple interface for objects with time information
/// </summary>
public interface ITimestamped
{
    /// <summary>
    /// Gets the timestamp
    /// </summary>
    DateTimeOffset Timestamp { get; }
}
