using System;

namespace seasonal
{
    // Simple interface for objects with time information
    public interface ITimestamped
    {
        DateTimeOffset Timestamp { get; }
    }
}
