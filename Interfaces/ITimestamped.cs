using System;

namespace Marsop.Ephemeral
{
    /// <summary>
    /// Simple interface for objects with time information
    /// </summary>
    public interface ITimestamped
    {
        DateTimeOffset Timestamp { get; }
    }
}
