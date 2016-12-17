using System;

namespace seasonal
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInterval
    {
        DateTimeOffset Start { get; }
        DateTimeOffset End { get; }
        TimeSpan Duration { get; }
    }
}