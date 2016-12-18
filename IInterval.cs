using System;

namespace seasonal
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInterval
    {
        DateTimeOffset Start { get; }
        bool StartIncluded { get; }
        DateTimeOffset End { get; }
        bool EndIncluded { get; }
        TimeSpan Duration { get; }
    }
}