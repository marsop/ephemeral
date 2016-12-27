using System;

namespace ephemeral
{
    /// <summary>
    /// Simple interface for objects with time information
    /// </summary>
    public interface ITimestamped
    {
        DateTimeOffset Timestamp { get; }
    }
}
