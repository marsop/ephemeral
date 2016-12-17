using System;

namespace seasonal
{
    public interface ITimestamped
    {
        DateTimeOffset Timestamp { get; }
    }
}